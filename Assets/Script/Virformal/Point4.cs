using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point4 : MonoBehaviour
{
    public bool isArrived = false;
    private float stayTm = 0.0f, treshold = 0.3f;
    private bool isChecking = false;
    private void Start()
    {
        isArrived = false;
        stayTm = 0.0f;
        isChecking = false;
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Dog") isArrived = true;
        //Debug.Log("Dog stay");
    }
    private void OnTriggerExit(Collider col)
    {
    }
}
