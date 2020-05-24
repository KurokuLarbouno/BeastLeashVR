using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public int State = 0;
    private GameObject StageManager = null;
    private Animator AnimCatAC, modleAC;
    private bool isInit = false;
    private AudioSource catAS;

    void Start()
    {
        StageManager = GameObject.FindWithTag("StageManerger");
        AnimCatAC = GetComponent<Animator>();
        gameObject.SetActive(false);
        modleAC = transform.GetChild(0).GetComponent<Animator>();
        catAS = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isInit)
        {
            switch (State)
            {
                case 1:
                    if (!gameObject.activeSelf) gameObject.SetActive(true); 
                    isInit = true;
                    catAS.Play();
                    break;
                case 2:
                    if (!AnimCatAC.GetBool("isLooked")) AnimCatAC.SetBool("isLooked", true);
                    isInit = true;
                    break;
                default:
                    break;
            }
        }
    }

    public void CatOut()
    {
        StageManager.GetComponent<StageManerger>().CatLeaved();
        State = 3;
        gameObject.SetActive(false);
        //Debug.Log("Cat Out");
        isInit = false;
    }
    public void CatWalk()
    {
        modleAC.SetBool("walk", true);
        modleAC.SetBool("hop", false);
    }
    public void CatHop()
    {
        modleAC.SetBool("walk", false);
        modleAC.SetBool("hop", true);
    }
    public void CatLooked()
    {
        StageManager.GetComponent<StageManerger>().CatLooked();
        State = 2;
        catAS.Stop();
        isInit = false;
    }
}
