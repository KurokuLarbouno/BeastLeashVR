using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VibrationManager : MonoBehaviour
{
    public static SteamVR_Action_Vibration hapticAction;
    //public static SteamVR_Action_Boolean trackpadAction;
    // Start is called before the first frame update

    void Update()
    {
        //if (trackpadAction.GetLastStateDown(SteamVR_Input_Sources.LeftHand))
        //{
        //    Pulse(1, 150, 75, SteamVR_Input_Sources.LeftHand);
        //}
        //if (trackpadAction.GetLastStateDown(SteamVR_Input_Sources.LeftHand))
        //{
        //    Pulse(1, 150, 75, SteamVR_Input_Sources.RightHand);

        //}
    }
    public void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources sources)
    {
        //hapticAction.Execute(0, duration, frequency, amplitude, sources);
    }
}
