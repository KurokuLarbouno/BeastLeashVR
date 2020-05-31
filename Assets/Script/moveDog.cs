using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class moveDog : MonoBehaviour
{

    int dogState = 1;//1:walk, 2:rush, 3:stay
    public bool isbarking = false, isPulling = false;
    private Animator AnimDogAC;
    private GameObject Dogfab, Target, DogChestNd, DogHeadNd, MainCam, Cat, Dog;
    private bool sit = false, run = false, walk = false, idle = false, isWait = true, isFallback = false;
    private int sitState = 0, runState = 0, walkState = 0, idleState = 0;
    private Vector3 dogPos, dogFace, goVec, oldVec;
    private float  runLength = 2.0f;
    private float sitTime = 15, changeTime = 10;
    private float  playerSp = 0;
    public AudioSource dogAS;


    void Start()
    {
        Dogfab = transform.GetChild(0).gameObject;
        //DogChestNd = GameObject.FindWithTag("DogChestNd");
        ////Debug.Log(DogChestNd);
        //DogHeadNd = GameObject.FindWithTag("DogHeadNd");
        ////Debug.Log(DogHeadNd);
        //Target = GameObject.FindWithTag("Target");
        //MainCam = GameObject.FindWithTag("MainCamera");
        ////Debug.Log(MainCam);
        AnimDogAC = Dogfab.GetComponent<Animator>();
        dogAS = GetComponent<AudioSource>();
        Idle();
    }

    void FixedUpdate()
    {
        //dogState = transform.GetComponentInParent<FormalDog>().dogState;
        //更新位置(兩點位置、角度、距離)
        Vector3 curVec = new Vector3(transform.position.x, 0, transform.position.z);
        goVec = dogPos - curVec;
        dogFace = new Vector3(transform.forward.x, 0, transform.forward.z);
        float tarLength = goVec.magnitude;
        playerSp = tarLength / Time.deltaTime;
        //Debug.Log(playerSp);
        goVec.Normalize(); dogFace.Normalize();

        //govec * forward y < 0
        Vector3 crossVec = Vector3.Cross(goVec, transform.forward);
        //Debug.Log(crossVec.y);
        if (crossVec.y > 0.01f) isFallback = true;
        else isFallback = false;

        dogPos = curVec;
        oldVec = goVec;

        //更新動畫
        if (isFallback)
        {
            AnimDogAC.speed = 1;
            if (playerSp > 0.2f) Pull();
        }
        else if (GetComponent<Chase>().dogState == 1 && crossVec.y < 0) 
        {
            AnimDogAC.speed = playerSp;
            isPulling = false;
            Walk(); /*Debug.Log("WALK");*/ isWait = false;
        }
        else if (GetComponent<Chase>().dogState == 2 && crossVec.y < 0)
        {
            isPulling = false;
            Run(); /*Debug.Log("WALK");*/ isWait = false;
        }
        else
        {

            AnimDogAC.speed = 1;
            isPulling = false;
            if (!isWait)
            {
                isWait = true;
                this.Invoke("Idle", 0.8f);
                //Idle();
            }
        }
    }
    private void LateUpdate()
    {
        if (goVec.magnitude > 0.1f)
        {
            Quaternion convertRot = Quaternion.LookRotation(goVec);
            convertRot *= Quaternion.Euler(0, -90, -180 - 8.12f);
            //DogChestNd.transform.rotation = convertRot;//Quaternion.Lerp(DogChestNd.transform.rotation, convertRot, ttt);
        }
        if (isbarking)
        {
            Quaternion lookRot = Quaternion.LookRotation(Cat.transform.position - transform.position);
            Vector3 lookElr = lookRot.eulerAngles;
            transform.rotation = Quaternion.Euler(0.0f, lookElr.y, lookElr.z);
            string nowAnim = AnimDogAC.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            if (nowAnim == "BeagleIdleBarking")
            {
                AnimDogAC.SetBool("bark", false);
                isbarking = false;
                sit = false; run = false; walk = false; idle = false;
            }
            else
            {
                sit = false; run = false; walk = false; idle = false;
                AnimDogAC.SetBool("bark", true);
            }
                
        }

    }
    public void Idle()
    {
        this.CancelInvoke();
        sit = false; run = false; walk = false; idle = true;
        AnimDogAC.speed = 1;
        AnimDogAC.SetBool("sit", sit); AnimDogAC.SetBool("run", run); AnimDogAC.SetBool("walk", walk); AnimDogAC.SetBool("idle", idle); AnimDogAC.SetBool("pull", false); AnimDogAC.SetBool("bark", false);
        this.Invoke("ChangeState", changeTime + Random.Range(0, 9) - 4);
        this.Invoke("Sit", sitTime + Random.Range(0, 8) - 4);
        //Debug.Log("Idle");
    }
    private void Walk()
    {
        this.CancelInvoke();
        sit = false; run = false; walk = true; idle = false;
        AnimDogAC.SetBool("sit", sit); AnimDogAC.SetBool("run", run); AnimDogAC.SetBool("walk", walk); AnimDogAC.SetBool("idle", idle); AnimDogAC.SetBool("pull", isPulling); AnimDogAC.SetBool("bark", false);
        //Debug.Log("WALK");
        this.Invoke("ChangeState", changeTime + Random.Range(0, 9) - 4);
    }
    private void Sit()
    {
        this.CancelInvoke();
        sit = true; run = false; walk = false; idle = false;
        AnimDogAC.SetBool("sit", sit); AnimDogAC.SetBool("run", run); AnimDogAC.SetBool("walk", walk); AnimDogAC.SetBool("idle", idle); AnimDogAC.SetBool("pull", isPulling) ; AnimDogAC.SetBool("bark", false);
        //Debug.Log("SIT");
        this.Invoke("ChangeState", changeTime + Random.Range(0, 9) - 4);
    }
    public void Run()
    {
        this.CancelInvoke();
        sit = false; run = true; walk = false; idle = false;
        AnimDogAC.SetBool("sit", sit); AnimDogAC.SetBool("run", run); AnimDogAC.SetBool("walk", walk); AnimDogAC.SetBool("idle", idle); AnimDogAC.SetBool("pull", isPulling) ; AnimDogAC.SetBool("bark", false);
        //Debug.Log("RUN");
        runState = Random.Range(0, 2);
        AnimDogAC.SetInteger("runState", runState);
    }
    public void Pull()
    {
        this.CancelInvoke();
        sit = false; run = false; walk = false; idle = false;
        AnimDogAC.SetBool("sit", sit); AnimDogAC.SetBool("run", run); AnimDogAC.SetBool("walk", walk); AnimDogAC.SetBool("idle", idle); AnimDogAC.SetBool("bark", false);
        AnimDogAC.SetBool("pull", true);
        isPulling = true;
        //Debug.Log("Anim fallback");
    }
    public void Bark(Transform obj)
    {
        this.CancelInvoke();
        //Debug.Log("Bark At " + obj);
        Cat = obj.gameObject;
        sit = false; run = false; walk = false; idle = false;
        AnimDogAC.SetBool("sit", sit); AnimDogAC.SetBool("run", run); AnimDogAC.SetBool("walk", walk); AnimDogAC.SetBool("idle", idle); AnimDogAC.SetBool("pull", isPulling);
        AnimDogAC.SetBool("bark", true);
        dogAS.Play();
        isbarking = true;
    }
    public void StopBark()
    {
        AnimDogAC.SetBool("bark", false);
        dogAS.Stop();
        isbarking = false;
        Idle();
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