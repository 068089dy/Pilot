using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotIkWalk : MonoBehaviour
{

    [Header("IK")]
    public Transform leftFootIkTarget;
    Vector3 leftFootPoint;
    float nextFoorDistanceSqr;

    int floorLayerMask = 1 << 9;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void IK1()
    {
        Ray ray = new Ray(transform.position + transform.right * 2 + Vector3.up * 4 + transform.forward * 3, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit info, 5, floorLayerMask))
        {

            if (Vector3.Distance(leftFootPoint, transform.position) > 8)
            {
                Debug.Log("碰到地面" + info.point);
                leftFootPoint = info.point;
                nextFoorDistanceSqr = (leftFootPoint - leftFootIkTarget.position).sqrMagnitude;
            }
        }
        Vector3 leftFootY = Vector3.zero;
        leftFootY.y += 0.74f;
        leftFootY.y += Mathf.Sin((leftFootPoint - leftFootIkTarget.position).sqrMagnitude * Mathf.PI / nextFoorDistanceSqr) * 4;
        leftFootIkTarget.position = Vector3.Lerp(leftFootIkTarget.position,
            leftFootPoint + leftFootY,
            Time.deltaTime * 5f);
    }

    

    private void LateUpdate()
    {
        
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(leftFootPoint, 0.5f);
    }
}
