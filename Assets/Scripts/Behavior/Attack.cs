using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[RequireComponent(typeof(EnemyAnimationController))]

//[RequireComponent(typeof(EnemyWeaponControl))]
public class Attack : Action
{

    public float maxAttackDistabce = 100;
    public float rotationSpeed = 0.4f;

    EnemyAnimationController m_EnemyAnimationController;
    EnemyBehaviorController m_EnemyBehaviorController;
    public EnemyWeaponControl m_EnemyWeaponController;


    GameObject curObject;
    //public GameObject testObject;
    // �����ڵ�������
    public LayerMask obstructionLayerMask = -1;

    float lastAttackTime;
    float lastDiscoverTime;
    // �������һ�η���Ŀ��೤ʱ���ʧĿ�ꣻ
    public float discoverInterval = 2;
    // �������
    public float shootInterval = 3;
    // Ŀ������ƫ����
    public Vector3 targetCenterOffset = Vector3.up;

    public float attackPosOffsetRandom = 2;
    bool isTargetVisible;
    public override void OnStart()
    {
        m_EnemyAnimationController = GetComponent<EnemyAnimationController>();
        m_EnemyBehaviorController = GetComponent<EnemyBehaviorController>();
        //m_EnemyWeaponController = GetComponent<EnemyWeaponControl>();
    }

    public override TaskStatus OnUpdate()
    {
        if (m_EnemyBehaviorController.curAttackTarget == null)
        {
            Debug.Log("�޹���Ŀ��"+ m_EnemyBehaviorController.curAttackTarget.name);
            return TaskStatus.Failure;
        }

        curObject = m_EnemyBehaviorController.curAttackTarget;
        CheckTargetVisible();
        m_EnemyAnimationController.Aim();

        
        if (curObject.GetComponent<Damagable>() != null && curObject.GetComponent<Damagable>().DamagePoint != null)
        {
            m_EnemyAnimationController.SetAimIkTarget(curObject.GetComponent<Damagable>().DamagePoint);
        } else
        {
            m_EnemyAnimationController.SetAimIkTarget(curObject.transform);
        }
        //transform.LookAt(new Vector3(targets[0].transform.position.x, transform.position.y, targets[0].transform.position.z));
        transform.forward = Vector3.Lerp(transform.forward, new Vector3(
                    curObject.transform.position.x - transform.position.x,
                    0,
                    curObject.transform.position.z - transform.position.z
                    ), Time.deltaTime * rotationSpeed);
        // ˮƽ����н�С��10��ʱ������
        if (Vector3.Angle(transform.forward, Vector3.ProjectOnPlane(curObject.transform.position - transform.position, Vector3.up)) < 10)
        {
            if (isTargetVisible)
            {
                Shoot(curObject.transform.position + targetCenterOffset);
                m_EnemyAnimationController.EnableAimIk();
            }
        } else
        {
            m_EnemyAnimationController.DisableAimIk();
        }

        if (isTargetVisible)
        {
            return TaskStatus.Running;
        } else
        {
            // ���δ������ʧ���������running
            if (Time.time < lastDiscoverTime + discoverInterval)
            {
                //Debug.Log("δ������ʧ���"+ lastDiscoverTime + ","+Time.time);
                return TaskStatus.Running;
            }
            return TaskStatus.Failure;
        }
    }
    public override void OnEnd()
    {
        m_EnemyAnimationController.Idle();
        m_EnemyAnimationController.DisableAimIk();
    }

    bool CheckTargetVisible()
    {
        if (curObject != null) {
            if (Physics.Raycast(transform.position, curObject.transform.position + targetCenterOffset - transform.position, out RaycastHit hit, maxAttackDistabce, obstructionLayerMask))
            {
                if (hit.transform.gameObject == curObject)
                {
                    isTargetVisible = true;
                    lastDiscoverTime = Time.time;
                } else
                {
                    isTargetVisible = false;
                }
            }
        } else
        {
            isTargetVisible = false;
        }
        return isTargetVisible;
    }

    void Shoot(Vector3 pos)
    {
        if (Time.time > lastAttackTime + shootInterval)
        {
            pos += new Vector3(Random.Range(-1f, 1f) * attackPosOffsetRandom, Random.Range(-1f, 1f) * attackPosOffsetRandom, Random.Range(-1f, 1f) * attackPosOffsetRandom);
            lastAttackTime = Time.time;
            m_EnemyAnimationController.Shoot();
            m_EnemyWeaponController.Fire(pos);
        }
    }

    //GameObject SelectObject()
    //{
    //    return targets[0];
    //}
}
