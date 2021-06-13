using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotFootIk : MonoBehaviour
{
    
    int floorLayerMask = 1 << 9;
    Vector3 hitPoint;

    [Header("Attibute")]
    public float rotateSpeed = 40f;

    [Header("IK")]
    public Transform footIkTarget;
    Vector3 footIkTargetInitPos;
    // Start is called before the first frame update
    void Start()
    {
        footIkTargetInitPos = footIkTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        //footIkTarget.position = footIkTargetInitPos;
        footIkTarget.position = transform.position;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit info, 2, floorLayerMask))
        {
            hitPoint = info.point;
            //footIkTarget.position = info.point + Vector3.up * 1f;
            footIkTarget.position = Vector3.Lerp(footIkTarget.position, info.point + Vector3.up * 1f, Time.deltaTime * 30f);
            //hitPoint = info.point;
            //float tAngle = Mathf.Lerp(transform.eulerAngles.x, 110 - Vector3.Angle(info.normal, Vector3.up), Time.deltaTime * rotateSpeed);
            //transform.eulerAngles = new Vector3(tAngle, 0, 0);
        }
    }

    void Ik1()
    {
        footIkTarget.position = footIkTargetInitPos + Vector3.up*5;
        Ray ray = new Ray(transform.position - transform.forward, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit info, 2, floorLayerMask))
        {
            hitPoint = info.point;
            float tAngle = Mathf.Lerp(transform.eulerAngles.x, 110 - Vector3.Angle(info.normal, Vector3.up), Time.deltaTime * rotateSpeed);
            transform.eulerAngles = new Vector3(tAngle, 0, 0);
        }
    }

    private void LateUpdate()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hitPoint, 0.5f);
    }

}
