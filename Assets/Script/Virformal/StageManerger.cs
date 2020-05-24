using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
一
    放置
        巡邏點，狗依序到位
    動畫
        狗聞，吠叫表到達
    結束
        看到狗愛鴨子的相框，拿起旁邊的球，壓球狗不理
二
    放置
        貓在書櫃上喵叫，狗吠
    動畫
        人看到後，貓走
        貓跳窗離開
    結束
        狗衝過去
三
    放置
        狗一直對窗戶叫
        地上鴨子發光，一陣子叫
    結束
        人去拿，壓了狗走過來開心
四（實驗性）
    鴨可以丟，打到邊界會彈回，狗撿 
*/
public class StageManerger : MonoBehaviour
{
    public int stageState = 0;
    public GameObject Plyer, CheckPoint, Dog, Cat, Ball, Duck;
    public bool isInit = false, isCatLooked = false, isCatLeaved = false;
    void Start()
    {
        //Plyer = GameObject.FindWithTag("Player");
        //CheckPoint = GameObject.FindWithTag("CheckPoint");
        //Cat = GameObject.FindWithTag("Cat");
        //Dog = GameObject.FindWithTag("Dog");
        //Ball = GameObject.FindWithTag("Ball");
        //Duck = GameObject.FindWithTag("Duck");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInit)
        {
            switch (stageState)
            {
                case 1:
                    //放置巡邏點，狗依序到位
                    Debug.Log("StageOne");
                    CheckPoint.GetComponent<CheckPoint>().State = 1;
                    Dog.GetComponent<FormalDog>().dogState = 1; //走
                    isInit = true;
                    break;
                case 2:
                    if (!isCatLooked && !isCatLeaved)
                    {
                        //貓在書櫃上喵喵叫
                        Debug.Log("Stagetwo");
                        Cat.GetComponent<Cat>().State = 1; //Meow
                        Cat.SetActive(true);
                        Debug.Log(Cat);
                        //Invoke -> Dog.GetComponent<Dog>().Bark(Cat.transform.position);//一段時間後對貓叫
                        this.Invoke("Bark", 5.0f);
                    }
                    //人看到後，貓開始跑到窗戶                                   
                    if (isCatLooked)
                    {
                        Cat.GetComponent<Cat>().State = 2; //Walk
                    }
                    //貓跳窗離開，狗衝過去
                    if (isCatLeaved) //貓到窗
                    {
                        CheckPoint.GetComponent<CheckPoint>().State = 2;//狗向前衝
                        CheckPoint.GetComponent<CheckPoint>().isAsigned = false;
                        Dog.GetComponent<FormalDog>().dogState = 2;//衝
                    }
                    isInit = true;
                    break;
                case 3:
                    Debug.Log("StageThree");
                    Dog.GetComponent<FormalDog>().dogState = 3;//原地
                    Duck.GetComponent<Duck>().State = 1;//發光
                    isInit = true;
                    break;
                case 4:

                    break;
                default:
                    break;
            }
        }
    }
    public void StageOneEnded() //壓球狗不理
    {
        Debug.Log("StageOneEnded");
        stageState = 2;
        isInit = false;
        GetComponent<Cast>().enabled = true;
    }
    public void StageTwoEnded() //狗衝過去窗戶
    {
        Debug.Log("StageTwoEnded");
        stageState = 3;
        isInit = false;
        GetComponent<Cast>().enabled = false;
    }
    public void StageThereEnded() //狗因鴨靠人
    {
        Debug.Log("StageThereEnded");
        stageState = 4;
        isInit = false;
    }
    public void CatLooked()
    {
        isCatLooked = true; isInit = false;
    }
    public void CatLeaved()
    {
        isCatLeaved = true; isInit = false;
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "PlayerCube" && !isInit)
        {
            Debug.Log("Start!");
            stageState = 1;
            isInit = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }
    private void Bark()
    {
        Dog.GetComponent<moveDog>().Bark(Cat.transform);
    }
}
