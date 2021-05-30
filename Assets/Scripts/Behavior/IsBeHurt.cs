using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsBeHurt : Conditional
{
    Damagable m_Damagable;
    public float damageIntervalLimit = 2;

    public override void OnStart()
    {
        m_Damagable = GetComponent<Damagable>();
    }

    public override TaskStatus OnUpdate()
    {
        if (Time.time < damageIntervalLimit + m_Damagable.lastBeHurtTime)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
