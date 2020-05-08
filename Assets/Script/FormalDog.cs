using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class FormalDog : MonoBehaviour
{
    [SerializeField]
    [Range(1, 3)]
    public int dogState = 1;//1:walk, 2:rush, 3:stay
    //Serial
    private SerialPort sp = new SerialPort("COM6", 115200);
    //空間
    private GameObject Target, Tracker, Rope;
    private Vector3 carFace;//最終朝向方向, 車體方向
    private Vector3 carPos, tarPos, ropePos;
    private int carSp = 3;
    private bool isTimerSet = false;
    //繩子
    private float leashLength = 1;
    private bool initRope = false;
    //訊息
    private int sendMassage = 0;
    //walk mode
    private int walkMode = 2;//1: 朝target位置, 2:隨機, 3:不須改向, 4:等玩家給繩, 5:拉扯
    private int walkAngle = 0, walkMdTm = 1, walkSp = 0, waitTm = 3;
    //run mode
    private int runSp = 5;
    //stay mode

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.Find("Target");
        Tracker = GameObject.FindWithTag("Tracker");
        Rope = GameObject.FindWithTag("Rope");
        //Serial
        sp.Open();
        sp.ReadTimeout = 10;
        sp.Write("0");
    }

    // Update is called once per frame
    void Update()
    {
        if (sp.IsOpen && !isTimerSet)
        {
            //定時傳送
            this.InvokeRepeating("setMassage", 1.0f, 0.15f);//"methodName" in "time" seconds, then repeatedly every "repeatRate" seconds.
            isTimerSet = true;
        }
        //車子定位
        Vector3 trackPos = Tracker.transform.position;
        Quaternion trackRot = Tracker.transform.rotation;
        trackRot *= Quaternion.Euler(90, 0, 0);
        carPos = new Vector3(trackPos.x, 0, trackPos.z);
        transform.position = carPos;
        transform.rotation = new Quaternion(0, trackRot.y, 0, trackRot.w);

        //車子面向
        trackPos = transform.forward;
        carFace = new Vector3(trackPos.x, 0, trackPos.z);
        carFace.Normalize();
        //目標方向
        tarPos = Target.transform.position;
        tarPos = new Vector3(tarPos.x, 0, tarPos.z);
        Vector3 goVec = tarPos - carPos;
        //Debug.Log(goVec.magnitude);
        //繩子方向
        ropePos = Rope.transform.position;
        ropePos = new Vector3(ropePos.x, 0, ropePos.z);
        Vector3 ropeVec = carPos - ropePos;
        //Debug.Log("Rope" + ropeVec.magnitude);
        if (initRope)
        {
            //leashLength = ropeVec.magnitude;
            leashLength = 1.0f;
            initRope = true;
            Debug.Log("Leash length: " + leashLength);
        }

        if (goVec.magnitude > 0.3 && Target.transform.gameObject.tag == "Target")
        {
            goVec.Normalize();
            float angle = Vector3.SignedAngle(goVec, carFace, Vector3.up);
            int outSp = carSp;
            angle *= 1.7f;
            if (dogState == 1)//1:walk
            {
                //繩夠長:1之內隨機走
                //繩不夠:一秒拉一下
                if (ropeVec.magnitude > (leashLength - 0.3))
                {
                    if (walkMode == 4)
                    {
                        outSp = 0;
                    }
                    else if (walkMode == 5)
                    {
                        //outSp = carSp;//保持原狀
                        walkSp = UnityEngine.Random.Range(0, 1);
                        outSp += walkSp;
                    }
                    else
                    {
                        this.Invoke("waitPull", waitTm + UnityEngine.Random.Range(0, 1)-2);
                        walkMode = 4;
                    }
                }
                else if (goVec.magnitude >= 0.6 /*&& goVec.magnitude < leashLength*/)
                {
                    if (walkMode == 2)//變換方向後保持一陣子
                    {
                        walkMode = 3;
                        walkAngle = (UnityEngine.Random.Range(0, 1) - 1) * 30;
                        walkSp = UnityEngine.Random.Range(0, 2);
                        outSp += walkSp;
                        angle += walkAngle;
                        this.Invoke("changeDir", walkMdTm + UnityEngine.Random.Range(0, 1));
                        //Debug.Log("change dir");
                    }
                    else if (walkMode == 3)//保持方向速度
                    {
                        angle += walkAngle;
                        outSp += walkSp;
                    }
                }
                else if (goVec.magnitude <0.7)//正規走
                {
                    walkMode = 1;
                    //CancelInvoke();
                }
            }
            else if (dogState == 2)//2:rush
            {
                //如果狗後退，先切walk
                carSp = runSp;
                //Debug.Log("RUN!");
            }
            else if (dogState == 3)//3:stay
            {

            }

            sendMassage = 10 + outSp;
            if (angle >= 0)
            {
                if (angle > 85) angle = 85;//75011
                sendMassage += ((int)angle) * 1000 + 100;
            }
            else
            {
                if (angle < -85) angle = -85;//75111
                sendMassage += -1 * ((int)angle) * 1000;
            }
            sendMassage += 100000;
        }
        else
        {
            sendMassage = 100000;
        }
        //try
        //{
        //    String value = sp.ReadLine();
        //    if (value != "")
        //    {
        //        Debug.Log("> " + value);
        //    }
        //}
        //catch
        //{
        //    Debug.Log("ReadLine Timeout!");
        //}
    }

    private void setMassage()
    {
        if (sp.IsOpen)
        {
            try
            {
                //Debug.Log(sendMassage.ToString());
                if(sendMassage >= 100000 && sendMassage < 200000) sp.Write(sendMassage.ToString());
                else Debug.Log("massage wrong!" + sendMassage.ToString());
            }
            catch
            {
                Debug.Log("WriteLine Timeout!");
            }

        }
        else
        {
            Debug.Log(" Serial Error.");
        }
    }

    private void changeDir()//走路隨機方向
    {
        walkMode = 2;
        this.Invoke("changeDir", walkMdTm + UnityEngine.Random.Range(0, 1));
    }
    private void waitPull()//走路等待主人
    {
        Debug.Log("wait for u!!!" + walkMode);
        
        if (walkMode == 4)
        {
            walkMode = 5;
            this.Invoke("waitPull", waitTm + UnityEngine.Random.Range(2, 3));
        }
        else if (walkMode == 5)
        {
            walkMode = 4;
            this.Invoke("waitPull", waitTm + UnityEngine.Random.Range(0, 1) - 2);
        }
    }
}