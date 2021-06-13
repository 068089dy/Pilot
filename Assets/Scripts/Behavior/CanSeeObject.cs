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
    // �������һ�η���Ŀ��೤ʱ���ʧĿ�ꣻ
    public float discoverInterval = 2;
    // �����ڵ�������
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
        // ѡ��Ŀ��
        m_EnemyBehaviorController.curAttackTarget = SelectObject();
        // Ŀ��Ϊ�գ�����ʧ��
        if (!m_EnemyBehaviorController.curAttackTarget)
        {
            return TaskStatus.Failure;
        }
        // ���Ŀ��δ��ʧ������ָ��ʱ���ڷ���Ŀ�꣩�����سɹ�
        if (Time.time < lastSeeTime + discoverInterval)
        {
            return TaskStatus.Success;
        }

        // �Ƿ񿴼�Ŀ��
        if (Vector3.Angle(transform.forward, m_EnemyBehaviorController.curAttackTarget.transform.position+Vector3.up - transform.position) < angleLimit)
        {
            //Debug.Log("�ǶȺ���"+ Vector3.Angle(transform.forward, curObject.transform.position - transform.position));
            if (Physics.Raycast(transform.position+eyePos, (m_EnemyBehaviorController.curAttackTarget.transform.position + Vector3.up) - (transform.position+ eyePos), out RaycastHit hit, distanceLimit, obstructionLayerMask)){
                Debug.DrawLine(transform.position, m_EnemyBehaviorController.curAttackTarget.transform.position + Vector3.up, Color.red);
                //Debug.Log("��ײ");
                if (hit.transform == m_EnemyBehaviorController.curAttackTarget.transform)
                {
                    //Debug.Log("��ײ����");
                    lastSeeTime = Time.time;
                    return TaskStatus.Success;
                } else
                {
                    Debug.Log("��ײ������"+ hit.transform.gameObject.name);
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
