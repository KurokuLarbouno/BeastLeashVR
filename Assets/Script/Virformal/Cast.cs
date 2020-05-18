using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cast : MonoBehaviour
{
    // Update is called once per frame
    void Start()
    {
    }
    void Update()
    {
        Debug.Log(Camera.main.name);//找到是哪個控制
        Ray eyeray = Camera.main.ScreenPointToRay(new Vector2(Camera.main.pixelWidth / 2.0f, Camera.main.pixelHeight / 2.0f));
        RaycastHit hit;
        if(Physics.Raycast(eyeray, out hit))
        {
            Transform selection = hit.transform;
            if (selection.CompareTag("Cat"))
            {
                selection.GetComponent<Cat>().CatLooked();
            }
        }
    }
}
