using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public int State = 0;
    private GameObject StageManager = null;
    private Animator AnimCatAC;

    void Start()
    {
        StageManager = GameObject.FindWithTag("StageManerger");
        AnimCatAC = GetComponent<Animator>();
        GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        switch (State)
        {
            case 1:
                if (!GetComponent<MeshRenderer>().enabled) GetComponent<MeshRenderer>().enabled = true;
                break;
            case 2:
                if (!AnimCatAC.GetBool("isLooked")) AnimCatAC.SetBool("isLooked",true);
                break;
            default:
                break;
        }
    }

    public void CatOut()
    {
        StageManager.GetComponent<StageManerger>().CatLeaved();
        State = 3;
        enabled = false;
        Debug.Log("Cat Out");
    }
    public void CatLooked()
    {
        StageManager.GetComponent<StageManerger>().CatLooked();
        State = 2;
    }
}
