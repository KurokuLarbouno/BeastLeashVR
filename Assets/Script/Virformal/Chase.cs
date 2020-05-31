using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditorInternal;

public class Chase : MonoBehaviour
{
    [SerializeField]
    [Range(1, 3)]
    public int dogState = 1;//1:walk, 2:rush, 3:stay
    //空間
    private GameObject Target, Rope;
    private Vector3 carFace;//最終朝向方向, 車體方向
    private Vector3 carPos, tarPos, ropePos;
    private int carSp = 1;
    private bool isTimerSet = false;
    //繩子
    public float leashLength = 1;
    private bool initRope = false;
    //碰牆事件
    private float wallOffset = 1.0f;    //宣告距離
    RaycastHit hit;//射線方向
    //訊息
    private int sendMassage = 0;
    //walk mode
    private int walkMode = 2;//1: 朝target位置, 2:隨機, 3:不須改向, 4:等玩家給繩, 5:拉扯
    private int walkAngle = 0, walkMdTm = 1, walkSp = 0, waitTm = 2;
    //run mode
    private int runSp = 2;
    public bool isFallback = false;
    //stay mode

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.Find("carDog");
        //Debug.Log(this.name);
        Rope = GameObject.FindWithTag("Rope");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //車子定位
        //Vector3 trackPos = transform.position;
        //Quaternion trackRot = Target.transform.rotation;
        //trackRot *= Quaternion.Euler(90, 0, 0);
        //transform.position = Target.transform.position;
        //transform.rotation = Target.transform.rotation;
        if (Target != null)
        {

            //虛擬狗去追車狗，旋轉/dogstate 複製cardog的formal dog
            //
            tarPos = Target.transform.position;
            tarPos = new Vector3(tarPos.x, 0.06f, tarPos.z);
            Vector3 goVec = tarPos - carPos;
        //Debug.Log(goVec.magnitude);
        //繩子方向
            ropePos = Rope.transform.position;
            ropePos = new Vector3(ropePos.x, 0, ropePos.z);
            Vector3 ropeVec = carPos - ropePos;
            carPos = transform.position;
            //Debug.Log("Rope" + ropeVec.magnitude);
            //if (initRope)
            //{
            //    //leashLength = ropeVec.magnitude;
            //    leashLength = 1.0f;
            //    initRope = true;
            //    Debug.Log("Leash length: " + leashLength);
            //}

            if (goVec.magnitude > 0.2f)
            {
                goVec.Normalize();

                if (goVec.magnitude > 0.6f && goVec.magnitude > 1.0f)  //距離1.0~0.6時，會先以反方向繞出一點轉進去
                    goVec = Vector3.Lerp(-Target.transform.forward, goVec, (goVec.magnitude - 0.6f) / 0.4f * 0.3f);

                //拉扯檢查
                Vector3 physicVec;
                physicVec = transform.position - carPos;
                physicVec.Normalize();
                //if ((physicVec + goVec).magnitude < (goVec.magnitude - 0.05))
                //{
                //    isFallback = true;
                //    Debug.Log("formal fallback");
                //}
                //else isFallback = false;
                //預設方向及速度

                float angle = Vector3.SignedAngle(goVec, carFace, Vector3.up);
                int outSp = carSp;

                //各狀態微調
                if (dogState == 1)//1:walk
                {
                    if (ropeVec.magnitude > (leashLength - 0.1))
                    {
                        outSp = 0;
                    }
                }
                else if (dogState == 2)//2:rush
                {
                    //如果狗後退，先切walk
                    carSp = runSp;
                    //Debug.Log("RUN!");
                    //if (isFallback) { dogState = 1; }
                }
                else if (dogState == 3)//3:stay
                {
                    if (Target.name == "Duck")
                    {
                        dogState = 1;
                    }
                    if (goVec.magnitude > 0.1f)
                    {
                        carSp *= (int)(goVec.magnitude / 0.2f);
                        carSp += 2;
                        carSp %= 10;
                    }
                }
                Quaternion lookRot = Quaternion.LookRotation(goVec);
                Vector3 lookElr = lookRot.eulerAngles;
                lookElr = new Vector3(0.0f, lookElr.y, lookElr.z);
                if (!isFallback) transform.Translate(Vector3.forward * carSp * Time.deltaTime);
                transform.rotation = Quaternion.Euler(lookElr);
            }
            else
            {
                Vector3 dogDot = Vector3.Lerp(Target.transform.rotation.eulerAngles, transform.rotation.eulerAngles, 0.7f);
                transform.rotation = Quaternion.Euler(dogDot);
                dogDot = Vector3.Lerp(Target.transform.position, transform.position, 0.7f);
                transform.position = dogDot;
            }
        }
        else
        {
            //Debug.Log(Target.transform.name);
            sendMassage = 100000;
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
