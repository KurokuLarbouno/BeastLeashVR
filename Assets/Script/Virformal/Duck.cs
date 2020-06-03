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
    public GameObject Dog;

    public SteamVR_Action_Single gripSqueeze = SteamVR_Input.GetAction<SteamVR_Action_Single>("Squeeze");

    public int State = 0;
    private AudioSource duckAS;
    private StageManerger STM;
    private bool tagFlag = false;
    private Transform pinPos;

    void Start()
    {

        if (interactable == null)
            interactable = GetComponent<Interactable>();
        duckAS = GetComponent<AudioSource>();
        STM = GameObject.FindGameObjectWithTag("StageManerger").GetComponent<StageManerger>();
        gameObject.SetActive(false);
        pinPos = transform;
    }
    public void quack()
    {
        if (interactable.attachedToHand) //在手上的話拉插銷
        {
            transform.tag = "Untagged";
            duckAS.Play();
            if (STM.stageState == 3)
            {
                STM.StageThereEnded();
                tagFlag = true;
                Dog.GetComponent<moveDog>().Stare();//狗靜止看人
            }
            else if (STM.stageState == 4)
            {
                tagFlag = true;
                Dog.GetComponent<moveDog>().Stare();//狗靜止看人
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Dog") //碰狗即關
        {
            transform.tag = "Untagged";
            tagFlag = false;
        }
        else if (other.tag == "Floor" && tagFlag)//掉地啟動
        {
            transform.tag = "Target";
            tagFlag = false;
            Dog.GetComponent<moveDog>().isStare = false;
        }
    }
}


