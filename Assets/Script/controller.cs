using System.Collections;
using System.Collections.Generic;
using System.Net.Configuration;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class controller : MonoBehaviour
{
    public SteamVR_Action_Boolean m_GrabAction = null;
    
    private SteamVR_Behaviour_Pose m_Pose = null;
    private FixedJoint m_Joint = null;

    private Interactable m_CrrentInteractable = null;
    public  List<Interactable> m_CrrentInteractables = new List<Interactable>();

    private GameObject Target;

    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();
    }

    void Start()
    {
        Target = GameObject.FindWithTag("Target");
    }
      
        private void Update()
    {
        if (m_GrabAction.GetStateDown(m_Pose.inputSource)) 
        {
            TrigerUp();

        };
    }
    private void TrigerUp()
    {
        if (Target != null) 
        {
            if (Target.transform.gameObject.tag == "Untagged") Target.transform.gameObject.tag = "Target";
            if (Target.transform.gameObject.tag == "Target") Target.transform.gameObject.tag = "Untagged";
        }
    }
}
