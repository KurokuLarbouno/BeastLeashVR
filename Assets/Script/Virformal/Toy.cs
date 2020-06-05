using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class Toy : MonoBehaviour
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
        duckAS = GetComponent<AudioSource>();
    }
    public void quack()
    {
            duckAS.Play();
    }
}


