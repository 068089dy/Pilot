using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IsBeHurt : Conditional
{
    Actor actor;
    EnemyBehaviorController m_EnemyBehaviorController;
    public float damageIntervalLimit = 2;

    public override void OnStart()
    {
        m_EnemyBehaviorController = GetComponent<EnemyBehaviorController>();
        actor = GetComponent<Actor>();
    }

    public override TaskStatus OnUpdate()
    {
        if (Time.time < damageIntervalLimit + actor.lastBeHurtTime)
        {
            //Debug.Log("最后攻击目标" + m_Damagable.lastDamageMsg.target.gameObject.name);
            m_EnemyBehaviorController.curAttackTarget = actor.lastDamageMsg.shooter;
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
