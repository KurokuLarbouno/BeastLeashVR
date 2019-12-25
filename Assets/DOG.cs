using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOG : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject tracker;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tracker = GameObject.Find("traker");
        this.transform.position = tracker.transform.position;
    }
}
