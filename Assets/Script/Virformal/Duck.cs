using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class Duck : MonoBehaviour
{

    public Interactable interactable;
    public new SkinnedMeshRenderer renderer;

    public bool affectMaterial = true;

    public SteamVR_Action_Single gripSqueeze = SteamVR_Input.GetAction<SteamVR_Action_Single>("Squeeze");

    public int State = 0;
    private AudioSource duckAS;
    private StageManerger STM;

    void Start()
    {

        if (interactable == null)
            interactable = GetComponent<Interactable>();
        duckAS = GetComponent<AudioSource>();
        STM = GameObject.FindGameObjectWithTag("StageManerger").GetComponent<StageManerger>();
    }
    public void quack()
    {
        if (State == 1)
        {
            GetComponent<Interactable>().enabled = true;
            State = 2;
        }
        if (interactable.attachedToHand)
        {
            duckAS.Play();
            if (STM.stageState == 3)
            {
                STM.StageThereEnded();
                transform.tag = "Target";
            }
            else if (STM.stageState == 4)
            {
                transform.tag = "Target";
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Dog")
        {
            transform.tag = "Untagged";
        }
    }
}


