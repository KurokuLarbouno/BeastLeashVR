using UnityEngine;
using System.Collections;

public class Script1 : MonoBehaviour
{
    //LineRenderer
    private LineRenderer lineRenderer;
    //定义一个Vector3,用来存储鼠标点击的位置
    private Vector3 position;
    //用来索引端点
    private int index = 0;
    //端点数
    private int LengthOfLineRenderer = 0;
    private GameObject dog;
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
        lineRenderer.SetPosition(0, transform.position);
        dog = GameObject.Find("DOG");
        Debug.Log(dog);
    }

    void Update()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Debug.Log(lineRenderer);
        position = dog.transform.position;
        //lineRenderer.SetPosition(1, dog.transform.position);
        Debug.Log(position);

    }


}
