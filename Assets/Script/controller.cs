using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class controller : MonoBehaviour
{
    public SteamVR_Action_Single m_GrabAction = null;
    private SteamVR_Behaviour_Pose m_Pose = null;

    void Start()
    {
        m_Pose = GetComponentInParent<SteamVR_Behaviour_Pose>();
        m_GrabAction[m_Pose.inputSource].onChange -= Grab;
    }

    // Update is called once per frame
    void Update()
    {
        //if(m_GrabAction.GetStateDown(m_Pose.inputSource))
    }
    private void Grab(SteamVR_Action_Single action, SteamVR_Input_Sources source, float axis, float delta)
    {

    }
}
