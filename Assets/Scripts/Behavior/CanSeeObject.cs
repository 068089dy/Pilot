using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSeeObject : Conditional
{
    //public string targetTag;
    public GameObject[] targets;
    EnemyBehaviorController m_EnemyBehaviorController;
    public float angleLimit = 90;
    public float distanceLimit = 1000;

    float lastSeeTime = -3;
    // 距离最后一次发现目标多长时间后丢失目标；
    public float discoverInterval = 2;
    // 射线遮挡物掩码
    public LayerMask obstructionLayerMask = -1;

    Vector3 eyePos = Vector3.up * 1.5f;
    public override void OnStart()
    {
        //if (GetComponent<Actor>().team == Team.TEAM1)
        //{
        //    targets = GameObject.FindGameObjectsWithTag("group2");
        //} else if (GetComponent<Actor>().team == Team.TEAM2)
        //{
        //    targets = GameObject.FindGameObjectsWithTag("group1");
        //}
        m_EnemyBehaviorController = GetComponent<EnemyBehaviorController>();
    }

    public override TaskStatus OnUpdate()
    {
        // 选择目标
        m_EnemyBehaviorController.curAttackTarget = SelectObject();
        // 目标为空，返回失败
        if (!m_EnemyBehaviorController.curAttackTarget)
        {
            return TaskStatus.Failure;
        }
        // 如果目标未丢失（即在指定时间内法线目标），返回成功
        if (Time.time < lastSeeTime + discoverInterval)
        {
            return TaskStatus.Success;
        }

        // 是否看见目标
        if (Vector3.Angle(transform.forward, m_EnemyBehaviorController.curAttackTarget.transform.position+Vector3.up - transform.position) < angleLimit)
        {
            //Debug.Log("角度合适"+ Vector3.Angle(transform.forward, curObject.transform.position - transform.position));
            if (Physics.Raycast(transform.position+eyePos, (m_EnemyBehaviorController.curAttackTarget.transform.position + Vector3.up) - (transform.position+ eyePos), out RaycastHit hit, distanceLimit, obstructionLayerMask)){
                Debug.DrawLine(transform.position, m_EnemyBehaviorController.curAttackTarget.transform.position + Vector3.up, Color.red);
                //Debug.Log("碰撞");
                if (hit.transform == m_EnemyBehaviorController.curAttackTarget.transform)
                {
                    //Debug.Log("碰撞合适");
                    lastSeeTime = Time.time;
                    return TaskStatus.Success;
                } else
                {
                    Debug.Log("碰撞不合适"+ hit.transform.gameObject.name);
                }
            }
        }
        return TaskStatus.Failure;
    }

    Actor SelectObject()
    {
        Actor target = null;
        if (m_EnemyBehaviorController.targets.Count > 0)
        {
            float minD = distanceLimit;
            foreach (Actor ob in m_EnemyBehaviorController.targets)
            {
                if (ob)
                {
                    if (Vector3.Distance(ob.transform.position, transform.position) < minD)
                    {
                        minD = Vector3.Distance(ob.transform.position, transform.position);
                        target = ob.GetComponent<Actor>();
                    }
                }
            }
        }
        //return targets[0];
        return target;
    }
}
