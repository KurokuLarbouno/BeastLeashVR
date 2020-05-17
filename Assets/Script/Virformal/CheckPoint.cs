﻿using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public int State = 0;
    private int checkPoint = 0;
    private GameObject StageManager = null;
    private Transform[] points;
    private bool isAsigned = false;
    void Start()
    {
        points = GetComponentsInChildren<Transform>();//放置所有小孩，0是自己
        int i = 0;
        foreach (Transform item in points)
        {
            if (i != 0 && i != 5)
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
                    { points[1].gameObject.SetActive(false); points[2].gameObject.SetActive(true); checkPoint = 2; }
                    else if (!points[3].GetComponent<Point>().isArrived)
                    { points[2].gameObject.SetActive(false); points[3].gameObject.SetActive(true); checkPoint = 3; }
                    else if (points[3].GetComponent<Point>().isArrived)
                    { points[3].gameObject.SetActive(false); StageOneEnd(); checkPoint = 4; }
                    isAsigned = true;
                    break;
                case 2:
                    if (!points[4].GetComponent<Point>().isArrived) { points[4].gameObject.SetActive(true);}
                    else { points[4].tag = "Untagged"; StageTwoEnd(); }
            isAsigned = true;
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }else if (checkPoint != 0 && points[checkPoint].GetComponent<Point>().isArrived)
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