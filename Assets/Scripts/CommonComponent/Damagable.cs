using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Damagable : MonoBehaviour
{
    public Actor parentActor;
    // �������ʣ���ͷ�˺��ȣ�
    public int critRate = 1;


    public void Start()
    {
        gameObject.layer = LayerMask.NameToLayer(parentActor.hitLayer);
        parentActor.setLayerAction += setLayer;
    }

    void setLayer()
    {
        Debug.Log("���ò�" + parentActor.hitLayer + parentActor.name);
        gameObject.layer = LayerMask.NameToLayer(parentActor.hitLayer);
    }

    public DamageMsg BeHurt(AttackMsg attackMsg)
    {
        parentActor.lastBeHurtTime = Time.time;
        DamageMsg damageMsg = new DamageMsg();
        if (parentActor.health && parentActor.health.hp > 0)
        {
            parentActor.health.hp -= attackMsg.damage * critRate;
            damageMsg.damage = attackMsg.damage * critRate;
            if (parentActor.health.hp <= 0)
            {
                damageMsg.isKilled = true;
            }
            damageMsg.shooter = attackMsg.target;
            damageMsg.target = parentActor;
            damageMsg.protectileType = attackMsg.protectileType;
        }
        parentActor.lastDamageMsg = damageMsg;
        return damageMsg;
    }
}

// ������Ϣ
public class AttackMsg
{
    // ���˶���Ѫ
    public float damage;
    public Actor target;
    public ProtectileType protectileType;

    public AttackMsg(float damage, Actor target, ProtectileType protectileType)
    {
        this.damage = damage;
        this.target = target;
        this.protectileType = protectileType;
    }
}

[Serializable]
public class DamageMsg
{
    public float damage;
    public bool isKilled;
    public ProtectileType protectileType;
    public Actor shooter;
    public Actor target;

    public DamageMsg()
    {

    }
}

// ��ҩ/�˺�����
public enum ProtectileType
{
    RIFLE,
    //TITANRIFLE,
    ROCKET,
    LASER,
    TREAD
}
