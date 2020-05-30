using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class leash : MonoBehaviour
{
    public GameObject VibrateManerger, Dog, Controller;
    private float leashLimit = 1.2f, oldLength = 0, oldSP = 0;
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
            VibrateManerger.GetComponent<VibrationManager>().Pulse(0.1f, 15, 0.01f * acc, SteamVR_Input_Sources.RightHand);//Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources sources)
            //Debug.Log(acc);
        }
        if (LeashVec.magnitude > leashLimit) //繃直
        {
            float flexpart = (LeashVec.magnitude - leashLimit) / LeashVec.magnitude;
            transform.position = Vector3.Lerp(curPos, curPos + LeashVec* flexpart, flexpart);
            Dog.transform.position += (-LeashVec * 0.1f * flexpart);//狗退，到下次到位置再走
            Dog.GetComponent<Chase>().isFallback = true;
            //震動(0, a*幅度. a*幅度. 0.1sec)
            //Debug.Log(acc);
            VibrateManerger.GetComponent<VibrationManager>().Pulse(0.1f, 250 * flexpart, 100 * flexpart, SteamVR_Input_Sources.RightHand);//Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources sources)

        }
        else
        {
            Dog.GetComponent<Chase>().isFallback = false;
            transform.position = curPos;
        }
        transform.rotation = Controller.transform.GetChild(0).rotation;
        oldPos = curPos;  oldSP = curSP; oldLength = LeashVec.magnitude;
    }
}
