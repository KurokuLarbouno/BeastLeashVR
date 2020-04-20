using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fusionDog : MonoBehaviour
{
    private Animator AnimDogAC;
    private GameObject Dogfab, Target, DogChestNd, DogHeadNd, MainCam;
    private bool sit = false, run = false, walk = false, idle = false;
    private int sitState = 0, runState = 0, walkState = 0, idleState = 0;
    private Vector3 dogPos, tarPos, dogFace, goVec;
    private float actLength = 0.05f, runLength = 2.0f;
    private float sitTime = 15, changeTime = 10;
    private float chestAngle = 0.0f, headAngle = 0.0f, bodyAngle = 0.0f;
    private float chaseTimer = 0, playerSp = 1;


    void Start()
    {
        Dogfab = transform.GetChild(0).gameObject;
        DogChestNd = GameObject.FindWithTag("DogChestNd");
        //Debug.Log(DogChestNd);
        DogHeadNd = GameObject.FindWithTag("DogHeadNd");
        //Debug.Log(DogHeadNd);
        //Target = GameObject.FindWithTag("Target");
        Target = GameObject.FindWithTag("Tracker");
        MainCam = GameObject.FindWithTag("MainCamera");
        //Debug.Log(MainCam);
        AnimDogAC = Dogfab.GetComponent<Animator>();
        Idle();
    }

    void FixedUpdate()
    {
        //更新位置(兩點位置、角度、距離)
        Quaternion trackRot = Target.transform.rotation;
        trackRot *= Quaternion.Euler(90, 0, 0);

        Vector3 curVec = new Vector3(Target.transform.position.x, 0, Target.transform.position.z);
        if ((tarPos - curVec).magnitude < 0.1) chaseTimer = 0;
        tarPos = curVec;
        dogPos = new Vector3(transform.position.x, 0, transform.position.z);
        dogFace = new Vector3(transform.forward.x, 0, transform.forward.z);
        Vector3 tmpVec = tarPos - dogPos; 
        goVec = new Vector3(tmpVec.x, 0, tmpVec.z);
        float tarLength = goVec.magnitude;
        //Debug.Log(tarLength);
        goVec.Normalize(); dogFace.Normalize();
        //float angle = -1 * Vector3.SignedAngle(MainCam.transform.position, dogFace, Vector3.up);
        //更新動畫
        Quaternion convertRot = Quaternion.LookRotation(goVec);
        chaseTimer += Time.deltaTime;
        if (tarLength > actLength) transform.rotation = new Quaternion(0, trackRot.y, 0, trackRot.w);//Quaternion.Lerp(transform.rotation, new Quaternion(0, trackRot.y, 0, trackRot.w), chaseTimer + 0.2f);
        //if (Mathf.Abs(angle) < 50) DogChestNd.transform.Rotate(Vector3.up, angle);

            //DogHeadNd.transform.rotation = Quaternion.Euler(DogHeadNd.transform.rotation.x, DogHeadNd.transform.rotation.y, DogHeadNd.transform.rotation.z + 10.931f);
            //if (Mathf.Abs(angle) < 50) DogChestNd.transform.rotation = Quaternion.Euler(DogChestNd.transform.rotation.x, DogChestNd.transform.rotation.y + angle, DogChestNd.transform.rotation.z);

        if (tarLength > actLength && tarLength < runLength)//起步距離
        {
            if (!walk) { Walk(); /*Debug.Log("WALK");*/ }
            //transform.Translate(new Vector3(0, 0, playerSp * Time.deltaTime), Space.Self);//移動
            transform.Translate(goVec * playerSp * Time.deltaTime, Space.Self);
        }
        else if(tarLength >= runLength)
        {
            if (!run) { Run(); /*Debug.Log("RUN");*/ }
            //transform.Translate(new Vector3(0, 0, playerSp * 2.0f * Time.deltaTime), Space.Self);//移動
            transform.Translate(goVec * playerSp * Time.deltaTime, Space.Self);
        }
        else
        {
            if (!idle && !sit) { Idle(); /*Debug.Log("IDLE");*/ }
        }
        //移動
        //transform.Translate(new Vector3(0, 0, playerSp * Time.deltaTime), Space.Self);
    }
    private void LateUpdate()
    {
        Quaternion convertRot = Quaternion.LookRotation(goVec);
        convertRot *= Quaternion.Euler(0, -90, -180 - 8.12f);
        //DogChestNd.transform.rotation = convertRot;//Quaternion.Lerp(DogChestNd.transform.rotation, convertRot, ttt);
    }
    private void Idle()
    {
        this.CancelInvoke();
        sit = false; run = false; walk = false; idle = true;
        AnimDogAC.SetBool("sit", sit); AnimDogAC.SetBool("run", run); AnimDogAC.SetBool("walk", walk); AnimDogAC.SetBool("idle", idle);
        this.Invoke("ChangeState", changeTime + Random.Range(0, 9) - 4);
        this.Invoke("Sit", sitTime + Random.Range(0, 8) - 4);
        //Debug.Log("Idle");
    }
    private void Walk()
    {
        this.CancelInvoke();
        sit = false; run = false; walk = true; idle = false;
        AnimDogAC.SetBool("sit", sit); AnimDogAC.SetBool("run", run); AnimDogAC.SetBool("walk", walk); AnimDogAC.SetBool("idle", idle);
        //Debug.Log("WALK");
        this.Invoke("ChangeState", changeTime + Random.Range(0, 9) - 4);
    }
    private void Sit()
    {
        this.CancelInvoke();
        sit = true; run = false; walk = false; idle = false;
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
