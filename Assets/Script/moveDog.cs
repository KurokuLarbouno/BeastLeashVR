using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveDog : MonoBehaviour
{

    int dogState = 1;//1:walk, 2:rush, 3:stay

    private Animator AnimDogAC;
    private GameObject Dogfab, Target, DogChestNd, DogHeadNd, MainCam;
    private bool sit = false, run = false, walk = false, idle = false, isWait = true;
    private int sitState = 0, runState = 0, walkState = 0, idleState = 0;
    private Vector3 dogPos, dogFace, goVec;
    private float  runLength = 2.0f;
    private float sitTime = 15, changeTime = 10;
    private float  playerSp = 0;


    void Start()
    {
        Dogfab = transform.GetChild(0).gameObject;
        DogChestNd = GameObject.FindWithTag("DogChestNd");
        //Debug.Log(DogChestNd);
        DogHeadNd = GameObject.FindWithTag("DogHeadNd");
        //Debug.Log(DogHeadNd);
        Target = GameObject.FindWithTag("Target");
        MainCam = GameObject.FindWithTag("MainCamera");
        //Debug.Log(MainCam);
        AnimDogAC = Dogfab.GetComponent<Animator>();
        Idle();
    }

    void FixedUpdate()
    {
        //更新位置(兩點位置、角度、距離)
        Vector3 curVec = new Vector3(transform.position.x, 0, transform.position.z);
        goVec = dogPos - curVec;
        dogFace = new Vector3(transform.forward.x, 0, transform.forward.z);
        float tarLength = goVec.magnitude;
        playerSp = tarLength / Time.deltaTime;
        //Debug.Log(playerSp);
        goVec.Normalize(); dogFace.Normalize();
        //更新動畫
        if (goVec.magnitude > 0)
        {
            //transform.LookAt(goVec + curVec);
        }
        //if (playerSp > 0.2) transform.rotation = convertRot;//Quaternion.Lerp(transform.rotation, convertRot, chaseTimer);
        dogPos = curVec;


        if (playerSp <= 0.1)
        {
            if (!isWait)
            {
                isWait = true;
                this.Invoke("Idle", 0.8f);
                //Idle();
            }
        }
        else if (!walk) 
        { 
            Walk(); /*Debug.Log("WALK");*/ isWait = false;
        }
 
    }
    private void LateUpdate()
    {
        if (goVec.magnitude > 0)
        {
            Quaternion convertRot = Quaternion.LookRotation(goVec);
            convertRot *= Quaternion.Euler(0, -90, -180 - 8.12f);
            //DogChestNd.transform.rotation = convertRot;//Quaternion.Lerp(DogChestNd.transform.rotation, convertRot, ttt);
        }

    }
    private void Idle()
    {
        this.CancelInvoke();
        sit = false; run = false; walk = false; idle = true;
        AnimDogAC.speed = 1;
        AnimDogAC.SetBool("sit", sit); AnimDogAC.SetBool("run", run); AnimDogAC.SetBool("walk", walk); AnimDogAC.SetBool("idle", idle);
        this.Invoke("ChangeState", changeTime + Random.Range(0, 9) - 4);
        this.Invoke("Sit", sitTime + Random.Range(0, 8) - 4);
        //Debug.Log("Idle");
    }
    private void Walk()
    {
        this.CancelInvoke();
        sit = false; run = false; walk = true; idle = false;
        AnimDogAC.speed = playerSp;
        AnimDogAC.SetBool("sit", sit); AnimDogAC.SetBool("run", run); AnimDogAC.SetBool("walk", walk); AnimDogAC.SetBool("idle", idle);
        //Debug.Log("WALK");
        this.Invoke("ChangeState", changeTime + Random.Range(0, 9) - 4);
    }
    private void Sit()
    {
        this.CancelInvoke();
        sit = true; run = false; walk = false; idle = false;
        AnimDogAC.speed = 1;
        AnimDogAC.SetBool("sit", sit); AnimDogAC.SetBool("run", run); AnimDogAC.SetBool("walk", walk); AnimDogAC.SetBool("idle", idle);
        //Debug.Log("SIT");
        this.Invoke("ChangeState", changeTime + Random.Range(0, 9) - 4);
    }
    private void Run()
    {
        this.CancelInvoke();
        sit = false; run = true; walk = false; idle = false;
        AnimDogAC.SetBool("sit", sit); AnimDogAC.SetBool("run", run); AnimDogAC.SetBool("walk", walk); AnimDogAC.SetBool("idle", idle);
        //Debug.Log("RUN");
        runState = Random.Range(0, 2);
        AnimDogAC.SetInteger("runState", runState);
    }
    private void ChangeState()
    {
        Random.InitState(System.Guid.NewGuid().GetHashCode());

        if (idle)
        {
            idleState = Random.Range(0, 3);
            //Debug.Log("Idle Changed" + idleState);
            AnimDogAC.SetInteger("idleState", idleState);
            this.Invoke("ChangeState", changeTime + Random.Range(0, 9) - 4);
        }
        else if (walk)
        {
            walkState = Random.Range(0, 2);
            this.Invoke("ChangeState", changeTime + Random.Range(0, 9) - 4);
            AnimDogAC.SetInteger("walkState", walkState);
        }
        else if (sit)
        {
            if (Random.Range(0, 6) == 5) Idle();
            sitState = Random.Range(0, 3);
            this.Invoke("ChangeState", changeTime + Random.Range(0, 9) - 4);
            AnimDogAC.SetInteger("sitState", sitState);
        }
    }
}