﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public bool isArrived = false;
    private float stayTm = 0.0f, treshold = 3.0f;
    private bool isChecking = false;
    private void Start()
    {
        isArrived = false;
        stayTm = 0.0f;
        isChecking = false;
    }
    void Update()
    {
        if (isChecking)
        {
            stayTm += Time.deltaTime;
            if (stayTm > treshold) isArrived = true;
            //Debug.Log(stayTm);
        }
        Debug.DrawLine(transform.position, transform.position + transform.forward);
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "PlayerCube") isChecking = true;
        stayTm = 0.0f; isArrived = false;
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "PlayerCube") isChecking = false;
        stayTm = 0.0f; isArrived = false;
    }
}
