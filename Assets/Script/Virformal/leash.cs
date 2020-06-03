using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class leash : MonoBehaviour
{
    public GameObject VibrateManerger, VirtualDog, Dog, Controller;
    public bool ControllerMode = true;
    private float oldLength = 0, oldSP = 0;
    public float leashLimit = 1.2f;
    private Vector3 oldPos;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 curPos = Controller.transform.position;
        Vector3 LeashVec = Dog.transform.position - curPos;
        float curSP = (LeashVec.magnitude - oldLength) / Time.deltaTime;
        float acc = (curSP - oldSP) / Time.deltaTime;
        //計算加速度
        if(acc > 100.0f)
        {
            //震動(0, a*幅度. a*幅度. 0.1sec)
            //VibrateManerger.GetComponent<VibrationManager>().Pulse(0.1f, 15, 0.02f * acc, SteamVR_Input_Sources.RightHand);//Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources sources)
            //Debug.Log(acc);

        }
        if (LeashVec.magnitude > leashLimit) //繃直
        {
            if (ControllerMode == true)
            {
                float flexpart = (LeashVec.magnitude - leashLimit) / LeashVec.magnitude;
                transform.position = Vector3.Lerp(curPos, curPos + LeashVec * flexpart, flexpart * 2.0f);
                Vector3 tmpVec = -LeashVec * 0.07f * flexpart;
                Dog.transform.position += new Vector3(tmpVec.x, 0, tmpVec.z);//狗退
                //震動(0, a*幅度. a*幅度. 0.1sec)
                //Debug.Log(acc);
                //抓cardog狀態判定震大震小
                if (VirtualDog.GetComponent<FormalDog>().dogState == 2) VibrateManerger.GetComponent<VibrationManager>().Pulse(0.1f, 450 * flexpart, 100 * flexpart * 5, SteamVR_Input_Sources.RightHand);//Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources sources)
                if (VirtualDog.GetComponent<FormalDog>().dogState != 2) VibrateManerger.GetComponent<VibrationManager>().Pulse(0.1f, 250 * flexpart, 100 * flexpart * 2, SteamVR_Input_Sources.RightHand);//Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources sources)                                                                                                                                                               
                Controller.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                transform.position = curPos;
            }
            Dog.GetComponent<moveDog>().isPulling = true;
        }
        else
        {
            Controller.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            transform.position = curPos;
            Dog.GetComponent<moveDog>().StopPull();
        }
        transform.rotation = Controller.transform.rotation;
        oldPos = curPos;  oldSP = curSP; oldLength = LeashVec.magnitude;
    }
}
