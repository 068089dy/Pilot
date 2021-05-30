using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSeeObject : Conditional
{
    public string targetTag;
    public GameObject[] targets;
    EnemyBehaviorController m_EnemyBehaviorController;
    public float angleLimit = 90;
    public float distanceLimit = 1000;

    float lastSeeTime = -3;
    // �������һ�η���Ŀ��೤ʱ���ʧĿ�ꣻ
    public float discoverInterval = 2;
    // �����ڵ�������
    public LayerMask obstructionLayerMask = -1;
    public override void OnStart()
    {
        targets = GameObject.FindGameObjectsWithTag(targetTag);
        m_EnemyBehaviorController = GetComponent<EnemyBehaviorController>();
    }

    public override TaskStatus OnUpdate()
    {
        // ѡ��Ŀ��
        m_EnemyBehaviorController.curAttackTarget = SelectObject();
        // ���Ŀ��δ��ʧ������ָ��ʱ���ڷ���Ŀ�꣩�����سɹ�
        if (Time.time < lastSeeTime + discoverInterval)
        {
            return TaskStatus.Success;
        }

        // �Ƿ񿴼�Ŀ��
        if (Vector3.Angle(transform.forward, m_EnemyBehaviorController.curAttackTarget.transform.position+Vector3.up - transform.position) < angleLimit)
        {
            //Debug.Log("�ǶȺ���"+ Vector3.Angle(transform.forward, curObject.transform.position - transform.position));
            if (Physics.Raycast(transform.position, m_EnemyBehaviorController.curAttackTarget.transform.position + Vector3.up - transform.position, out RaycastHit hit, distanceLimit, obstructionLayerMask)){
                Debug.DrawLine(transform.position, m_EnemyBehaviorController.curAttackTarget.transform.position + Vector3.up, Color.red);
                //Debug.Log("��ײ");
                if (hit.transform.gameObject == m_EnemyBehaviorController.curAttackTarget)
                {
                    //Debug.Log("��ײ����");
                    lastSeeTime = Time.time;
                    return TaskStatus.Success;
                } else
                {
                    Debug.Log("��ײ������");
                }
            }
        }
        return TaskStatus.Failure;
    }

    GameObject SelectObject()
    {
        return targets[0];
    }
}
