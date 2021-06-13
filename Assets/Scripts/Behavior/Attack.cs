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


    public Actor curObject;
    //public GameObject testObject;
    // 射线遮挡物掩码
    public LayerMask obstructionLayerMask = -1;

    float lastAttackTime;
    float lastDiscoverTime;
    // 距离最后一次发现目标多长时间后丢失目标；
    public float discoverInterval = 2;
    // 攻击间隔
    public float shootInterval = 3;
    // 目标中心偏移量
    public Vector3 targetCenterOffset = Vector3.up;

    public float attackPosOffsetRandom = 2;
    public bool isTargetVisible;
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
            //Debug.Log("无攻击目标"+ m_EnemyBehaviorController.curAttackTarget.gameObject.name);
            return TaskStatus.Failure;
        }

        curObject = m_EnemyBehaviorController.curAttackTarget;
        CheckTargetVisible();
        m_EnemyAnimationController.Aim();

        
        if (curObject && curObject.damagePoint != null)
        {
            m_EnemyAnimationController.SetAimIkTarget(curObject.damagePoint);
        } else
        {
            m_EnemyAnimationController.SetAimIkTarget(curObject.transform);
        }
        
        transform.forward = Vector3.Lerp(transform.forward, new Vector3(
                    curObject.transform.position.x - transform.position.x,
                    0,
                    curObject.transform.position.z - transform.position.z
                    ), Time.deltaTime * rotationSpeed);
        // 水平方向夹角小于10度时，攻击
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
            // 如果未超过丢失间隔，返回running
            if (Time.time < lastDiscoverTime + discoverInterval)
            {
                //Debug.Log("未超过丢失间隔"+ lastDiscoverTime + ","+Time.time);
                return TaskStatus.Running;
            }
            Debug.Log("目标不可见且丢失");
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
            //Debug.Log("当前目标" + curObject.name);
            Vector3 eyePos = transform.position + Vector3.up * 1.5f;
            if (Physics.Raycast(eyePos, curObject.transform.position + targetCenterOffset - eyePos, out RaycastHit hit, maxAttackDistabce, obstructionLayerMask))
            {
                if (hit.transform == curObject.transform)
                {
                    //Debug.Log("碰到啥了1" + hit.transform.gameObject.name);
                    isTargetVisible = true;
                    lastDiscoverTime = Time.time;
                } else
                {
                    //Debug.Log("碰到啥了"+hit.transform.gameObject.name);
                    isTargetVisible = false;
                }
            }
        } else
        {
            //Debug.Log("当前目标weikong");
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
            //m_EnemyAnimationController.Shoot();
            m_EnemyWeaponController.Fire(pos);
        }
    }

    //GameObject SelectObject()
    //{
    //    return targets[0];
    //}
}
