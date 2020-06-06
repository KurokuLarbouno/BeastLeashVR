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

    public GameObject Target, Toy;

    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();
    }

    void Start()
    {
        //Target = GameObject.Find("Duck");
    }
    void Update()
    {
        if (m_GrabAction.GetStateDown(m_Pose.inputSource)) 
        {
            TrigerUp();
        };
        if (m_GrabAction.GetStateUp(m_Pose.inputSource))
        {
            TrigerDown();
        };
    }
    private void TrigerUp()
    {
        //Target.GetComponent<Duck>().quack();
        //if (Target != null) 
        //{
        //    if (Target.transform.gameObject.tag == "Untagged") Target.transform.gameObject.tag = "Flag";
        //    if (Target.transform.gameObject.tag == "Untagged") Target.transform.gameObject.tag = "Target";
        //}
    }
    private void TrigerDown()
    {
        if (Target.GetComponent<Duck>().State > 0)
        {
            Target.GetComponent<Duck>().quack();
        }
        if(Toy != null) Toy.transform.parent.GetComponent<Toy>().quack();
        //if (Target != null)
        //{
        //    if (Target.transform.gameObject.tag == "Flag") Target.transform.gameObject.tag = "Untagged";
        //    if (Target.transform.gameObject.tag == "Target") Target.transform.gameObject.tag = "Untagged";
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "toy") { Toy = other.gameObject;/* Debug.Log(other.transform.parent.name + "toy in");*/ }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "toy") { Toy = null ; /*Debug.Log(other.transform.parent.name + "toy out"); */}
    }
}
