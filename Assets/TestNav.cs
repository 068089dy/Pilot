using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNav : MonoBehaviour
{
    NavMeshAgent nav;    //都在寻路者身上加（组件，脚本）
    CharacterController m_Controller;
    public Transform targetTransForm;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        m_Controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        
        
        //Debug.Log(nav.velocity);
        //if (Input.GetMouseButton(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hitInfo;
        //    if (Physics.Raycast(ray, out hitInfo, 100))
        //    {
        //        Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
        //        Debug.Log(hitInfo.point);
        //        nav.SetDestination(hitInfo.point);
        //    }
        //}

        //if (targetTransForm)
        //{
        //    nav.SetDestination(targetTransForm.position);
        //}
    }
}
