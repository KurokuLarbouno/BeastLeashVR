using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOG : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 post_pose;
    Animation anim;
    void Start()
    {
        GameObject tempObj = transform.GetChild(0).gameObject;
        anim = tempObj.GetComponent<Animation>();
        anim.Play();
        Debug.Log(anim);
    }

    // Update is called once per frame
    void Update()
    {
        float temp, length;
        temp = Mathf.Pow(transform.position.x - post_pose.x, 2) + Mathf.Pow(transform.position.z - post_pose.z, 2);
        length = Mathf.Sqrt(temp);
        if(length >= 0.1 && !anim.isPlaying) {
            anim.Play("Default Take");
        }else if(length < 0.1)
        {
            //anim.Stop();
        }
        post_pose = transform.position;
    }
}
