using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class line : MonoBehaviour
{
    //LineRenderer
    private LineRenderer lineRenderer;
    //定义一个Vector3,用来存储鼠标点击的位置
    private Vector3 pos;
    //用来索引端点
    private int index = 0;
    //端点数
    private int LengthOfLineRenderer = 0;
    private GameObject dog, rope;
    void Start()
    {
        ////添加LineRenderer组件
        //lineRenderer = GetComponent<LineRenderer>();
        ////设置材质
        //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        ////设置颜色
        //lineRenderer.SetColors(Color.red, Color.yellow);
        ////设置宽度
        //lineRenderer.SetWidth(0.02f, 0.02f);
        dog = GameObject.Find("DOG");
        rope = GameObject.Find("rope");
        Debug.Log(dog);
        lineRenderer = GetComponent<LineRenderer>();
        Debug.Log(lineRenderer);

    }

    void Update()
    {
        //pos = dog.transform.position;
        pos = dog.transform.GetChild(1).transform.position;
        lineRenderer.SetPosition(0, pos);
        //Debug.Log(pos);
        pos = rope.transform.position;
        lineRenderer.SetPosition(1, pos);
        //Debug.Log(pos);

    }


}
