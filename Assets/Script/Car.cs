using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Target & CarFace 向量 取前進向量
//車長:400mm
//最大轉角:150/30

public class Car : MonoBehaviour
{
    #region Object Variables
    [SerializeField]
    //private GameObject lineGeneratorPrefab;
    #endregion
    private Vector3[] linePoints;
    private Vector3 tarPos;
    private GameObject Target;
    private GameObject mCar;
    private GameObject newLineGen;
    private LineRenderer lRend;
    private float distance = 2;    //宣告距離
    RaycastHit hit;//射線方向
    Vector3 goVec;
    // Start is called before the first frame update
    private void Start()
    {
        Target = GameObject.FindWithTag("Target");
        newLineGen = GameObject.FindWithTag("Line");
        tarPos = Target.transform.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //tarPos = Target.transform.position;
        Ray forRay = new Ray(transform.position, transform.forward); //射線
        if (Physics.Raycast(forRay, out hit, distance))
        {
            //射線碰到TAG為EVN 觸發
            if (hit.collider.tag == "wall")
            {
                Vector3 wallVec;
                wallVec = Vector3.Reflect(transform.forward, hit.normal);// Vector3 Reflect(Vector3 inDirection, Vector3 inNormal);
                wallVec = Vector3.ProjectOnPlane(wallVec, hit.normal);//Vector3 ProjectOnPlane(Vector3 vector, Vector3 planeNormal);
                if(wallVec.magnitude == 0) { wallVec = Vector3.ProjectOnPlane(transform.right, hit.normal); }
                wallVec.Normalize();
                float  rayDis = Vector3.Distance(hit.point, transform.position);
                goVec = Vector3.Lerp(wallVec, transform.forward, rayDis / distance + 0.1f);
                //Debug.Log("Hit"+ goVec);
            }
        }else
        {
            goVec = transform.forward;
        }
        Debug.DrawLine(transform.position, goVec * 3 + transform.position);
        //linePoints = new Vector3[] { transform.position, goVec + transform.position };
        //// Set positions of LineRenderer using linePoints array.
        //lRend.SetPositions(linePoints);
        //前進/目標的距離變大太多就停止
    }
}
