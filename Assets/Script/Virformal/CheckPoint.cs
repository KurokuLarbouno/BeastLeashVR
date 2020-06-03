using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public int State = 0;
    private int checkPoint = 0;
    private GameObject StageManager = null;
    private Transform[] points;
    public bool isAsigned = false;
    void Start()
    {
        points = GetComponentsInChildren<Transform>();//放置所有小孩，0是自己
        int i = 0;
        foreach (Transform item in points)
        {
            if (i != 0 && i != 7)
            {
                points[i].gameObject.SetActive(false);
            }
            //Debug.Log(item.GetComponent<Point>());
            i++;
        }
        StageManager = GameObject.FindWithTag("StageManerger");
    }

    void Update()
    {
        if (!isAsigned)
        {
            switch (State)
            {
                case 1:

                    if (!points[1].GetComponent<Point>().isArrived)
                    { points[1].gameObject.SetActive(true); checkPoint = 1; }
                    else if (!points[2].GetComponent<Point>().isArrived)
                    { points[1].gameObject.SetActive(false); points[1].gameObject.tag = "Untagged"; points[2].gameObject.SetActive(true); checkPoint = 2; }
                    else if (!points[3].GetComponent<PointTmp>().isArrived)
                    { points[2].gameObject.SetActive(false); points[2].gameObject.tag = "Untagged"; points[3].gameObject.SetActive(true); checkPoint = 3; }
                    else if (!points[4].GetComponent<PointTmp>().isArrived)
                    { points[3].gameObject.SetActive(false); points[3].gameObject.tag = "Untagged"; points[4].gameObject.SetActive(true); checkPoint = 4; }
                    else if (!points[5].GetComponent<Point>().isArrived)
                    { points[4].gameObject.SetActive(false); points[4].gameObject.tag = "Untagged"; points[5].gameObject.SetActive(true); checkPoint = 5; }
                    else if (points[5].GetComponent<Point>().isArrived)
                    { points[5].gameObject.SetActive(false); points[5].gameObject.tag = "Untagged"; checkPoint = 6; StageOneEnd(); }
                    isAsigned = true;
                    break;
                case 2:
                    if (!points[6].GetComponent<Point4>().isArrived) { points[6].gameObject.SetActive(true); checkPoint = 6; }
                    if (points[6].GetComponent<Point4>().isArrived) {/*points[6].tag = "Untagged"; points[6].gameObject.SetActive(false); */checkPoint = 0; StageTwoEnd(); }
                    isAsigned = true;
                    //State = 3;
                    break;
                case 3:
                    points[6].tag = "Untagged"; points[6].gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }
        else if (checkPoint == 3 || checkPoint == 4)
        {
            if (points[checkPoint].GetComponent<PointTmp>().isArrived)
            {
                isAsigned = false;
                Debug.Log(points[checkPoint].gameObject.name);
            }
        }
        else if (checkPoint == 6)
        {
            if (points[checkPoint].GetComponent<Point4>().isArrived)
            {
                isAsigned = false;
                State = 2;
            }
        }
        else if (checkPoint != 0 && points[checkPoint].GetComponent<Point>().isArrived)
        {
            isAsigned = false;
        }
    }
    void StageOneEnd()
    {
        StageManager.GetComponent<StageManerger>().StageOneEnded();
    }
    void StageTwoEnd()
    {
        StageManager.GetComponent<StageManerger>().StageTwoEnded();
    }
}
