using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Damagable : MonoBehaviour
{
    public Transform DamagePoint;
    // ���һ�α����ʱ��
    [System.NonSerialized]
    public float lastBeHurtTime;

    // ���һ�α�˭���
    [System.NonSerialized]
    public GameObject lastBeHurtObject;
    public Health health;
    // �������ʣ���ͷ�˺��ȣ�
    public int critRate = 1;

    public bool BeHurt(int amount)
    {
        lastBeHurtTime = Time.time;
        if (health)
        {
            health.hp -= amount * critRate;
            return true;
        }
        return false;
    }

    public DamageMsg BeHurt(AttackMsg attackMsg)
    {
        lastBeHurtTime = Time.time;
        DamageMsg damageMsg = new DamageMsg();
        if (health)
        {
            health.hp -= attackMsg.damage * critRate;
            damageMsg.damage = attackMsg.damage * critRate;
            if (health.hp <= 0)
            {
                damageMsg.isKilled = true;
            }
            damageMsg.target = health.transform.gameObject;
        }
        return damageMsg;
    }

    public bool BeHurt(int amount, GameObject gObject)
    {
        lastBeHurtObject = gObject;
        lastBeHurtTime = Time.time;
        if (health)
        {
            health.hp -= amount * critRate;
            return true;
        }
        return false;
    }
}

// ������Ϣ
public class AttackMsg
{
    // ���˶���Ѫ
    public float damage;
    public GameObject target;

    public AttackMsg(float damage, GameObject target)
    {
        this.damage = damage;
        this.target = target;
    }
}

public class DamageMsg
{
    public float damage;
    public bool isKilled;
    public GameObject target;

    public DamageMsg()
    {

    }
}
