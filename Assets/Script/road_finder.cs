using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class road_finder : MonoBehaviour
{
    private GameObject Target, Dog;
    private bool isFinished = false;
    private Vector3[] wayPoint = new Vector3[3];
    private int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.Find("Target");
        Dog = GameObject.FindWithTag("Dog");
        //Debug.Log(Target); Debug.Log(Dog);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Target.transform.gameObject.tag == "Flag")
        {
            if (Dog.GetComponent<FormalDog>().dogState == 1)//走狀態才會擊發
            {
                Vector3 trackPos = Target.transform.position;
                Vector3 DogPos = Dog.transform.position;
                isFinished = true;
                Vector3 nor_face = trackPos - DogPos;
                Vector3 face = nor_face;
                //Debug.Log("face" + face);
                nor_face.Normalize();
                Vector3 vir_face = new Vector3(-nor_face.z, 0, nor_face.x);
                //Debug.Log("vir_face" + vir_face);
                int length = (int)face.magnitude;
                //Debug.Log("magnitude" + length);
                for (int i = 0; i < 3; i++)
                {
                    Vector3 tmpPoint = new Vector3((i + 1) / 3.0f * face.x, 0, (i + 1) / 3.0f * face.z);
                    if (i == 0 && length > 1.7f)
                    {
                        wayPoint[i] = tmpPoint + UnityEngine.Random.Range(1, 2) * 2.3f * vir_face;
                    }
                    else if (i == 1 && length > 1.0f)
                    {
                        wayPoint[i] = tmpPoint + UnityEngine.Random.Range(-2, -1) * 2.3f * vir_face;
                    }                       
                    else if (i == 2)
                    {
                        wayPoint[i] = tmpPoint;
                    }
                    else
                    {
                        wayPoint[i] = new Vector3(0, -1000, 0); ;
                    } 
                    //Debug.Log("wayPoint" + wayPoint[i]);
                }
                //this.InvokeRepeating("setPose", 0.2f, 0.5f); }
                transform.position = wayPoint[index];
                transform.gameObject.tag = "Target";
                index++;
            }
            else
            {
                transform.position = Target.transform.position;
                transform.gameObject.tag = "Target";
                index = 0;
            }

        }
    }
    void OntrigerOnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Dog")
        {
            if (index < 3)
            {
                transform.position = wayPoint[index];
                index++;
            }
            else
            {
                transform.gameObject.tag = "Untagged";
                index = 0;
            }
        }
    }
}
