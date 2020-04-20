using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class newdog : MonoBehaviour
{
    private SerialPort sp = new SerialPort("COM6", 115200);
    private GameObject Target;
    private GameObject Tracker;
    private Vector3 finalFace, carFace;//最終朝向方向, 車體方向
    private Vector3 carPos, tarPos;
    private int carSp = 2;
    private bool isTimerSet = false, isReached = false;
    private int sendMassage = 0;

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.FindWithTag("Target");
        Tracker = GameObject.FindWithTag("Tracker");
        
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
        //決定方向
        tarPos = Target.transform.position;
        tarPos = new Vector3(tarPos.x, 0, tarPos.z);
        Vector3 goVec = tarPos - carPos;
        //Debug.Log(goVec.magnitude);
        if (goVec.magnitude > 0.5)
        {
            goVec.Normalize();
            float angle = Vector3.SignedAngle(goVec, carFace, Vector3.up);
            //Debug.Log((int)angle + " degrees.");
            sendMassage = 10 + carSp;
            angle *= 1.7f;
            if (angle >= 0)
            {
                if (angle > 75) angle = 75;//75011
                sendMassage += ((int)angle) * 1000 + 100;
            }
            else
            {
                if (angle < -75) angle = -75;//75111
                sendMassage += -1 * ((int)angle) * 1000;
            }
            sendMassage += 100000;
        }
        else
        {
            sendMassage = 100000;
        }
        try
        {
            String value = sp.ReadLine();
            if (value != "")
            {
                Debug.Log("> " + value);
            }
        }
        catch
        {
            //Debug.Log("ReadLine Timeout!");
        }
    }

    private void setMassage()
    {
        if (sp.IsOpen)
        {
            try
            {
                Debug.Log(sendMassage.ToString());
                sp.Write(sendMassage.ToString());
            }
            catch
            {
                Debug.Log("ReadLine Timeout!");
            }
            
        }
        else
        {
            Debug.Log(" Serial Error.");
        }
    }
    //private void OnTriggerEnter(Collider collider)
    //{
    //    if (collider.transform.parent.tag == "Target") 
    //    {
    //        isReached = true;
    //        Debug.Log("Bang!");
    //    }
    //}
}