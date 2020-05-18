using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCube : MonoBehaviour
{
    private GameObject HMD;
    // Start is called before the first frame update
    void Start()
    {
        GameObject Obj = GameObject.Find("VRCamera");
        HMD = Obj;
        Debug.Log(Obj.name);
    }

    // Update is called once per frame
    void Update()
    {
        if(HMD != null)
        {
            Vector3 trackPos = HMD.transform.position;
            transform.position = new Vector3(trackPos.x, 0, trackPos.z);
        }
    }
}
