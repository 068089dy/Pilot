using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class FindObject : Action
{
    public Transform targetTransform;
    public float distanceLimit = 2;
    NavMeshAgent nav;
    public override void OnStart()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    public override TaskStatus OnUpdate()
    {
        if (Vector3.Distance(targetTransform.position, transform.position) > distanceLimit)
        {
            nav.SetDestination(targetTransform.position);
        } else
        {
            nav.SetDestination(transform.position);
        }
        return TaskStatus.Success;
    }
}
