using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class MissileLockSystem : MonoBehaviour
{
    DamageCounter damageCounter;
    public Actor curTarget;
    public List<GuidanceMsg> targetCollection;

    float maxLifeTime = 20;
    //public ConcurrentQueue<Actor> targetQueue = new ConcurrentQueue<Actor>();
    //public ArrayList targetList = ArrayList.Synchronized(new ArrayList(5));
    // Start is called before the first frame update
    void Start()
    {
        damageCounter = GetComponent<DamageCounter>();
        damageCounter.DamageAction += setCurTarget;
        targetCollection = new List<GuidanceMsg>();
    }

    public Actor GetLatestTarget()
    {
        Actor target = null;
        if (targetCollection.Count > 0)
        {
            GuidanceMsg curMsg = targetCollection[0];
            foreach(GuidanceMsg g in targetCollection)
            {
                if (curMsg.lifeTime < g.lifeTime)
                {
                    curMsg = g;
                }
            }
            target = curMsg.target;
        }
        return target;
    }

    public void AddGuidanceMsg(Actor actor)
    {
        if (targetCollection.Count > 0)
        {
            foreach (GuidanceMsg g in targetCollection)
            {
                if (g.target == actor)
                {
                    g.lifeTime = maxLifeTime;
                    // ½áÊø
                    return;
                }
            }
            if (targetCollection.Count >= 5)
            {
                removeOldestTarget();
            }
            GuidanceMsg guidanceMsg = new GuidanceMsg(actor, maxLifeTime);
            targetCollection.Add(guidanceMsg);
        }
        else
        {
            if (targetCollection.Count >= 5)
            {
                removeOldestTarget();
            }
            GuidanceMsg guidanceMsg = new GuidanceMsg(actor, 5);
            targetCollection.Add(guidanceMsg);
        }
    }

    void removeOldestTarget()
    {
        if (targetCollection.Count > 0)
        {
            GuidanceMsg oldestMsg = targetCollection[0];
            foreach (GuidanceMsg g in targetCollection)
            {
                if (oldestMsg.lifeTime > g.lifeTime)
                {
                    oldestMsg = g;
                }
            }
            targetCollection.Remove(oldestMsg);
        }
    }

    void setCurTarget(DamageMsg damageMsg)
    {
        if (damageMsg.protectileType == ProtectileType.RIFLE)
        {
            curTarget = damageMsg.target;
            AddGuidanceMsg(damageMsg.target);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (Actor ac in targetList) {
        //    Debug.Log(ac.name);
        //}
        if (targetCollection.Count > 0)
        {
            foreach (GuidanceMsg guidanceMsg in targetCollection)
            {
                guidanceMsg.lifeTime -= Time.deltaTime;
                if (guidanceMsg.lifeTime <= 0)
                {
                    targetCollection.Remove(guidanceMsg);
                }
                if (!guidanceMsg.target.gameObject.activeInHierarchy || guidanceMsg.target.health.hp <= 0)
                {
                    targetCollection.Remove(guidanceMsg);
                }
            }
        }
    }

    
}

[Serializable]
public class GuidanceMsg
{
    public Actor target;
    public float lifeTime;

    public GuidanceMsg(Actor target, float lifetime)
    {
        this.target = target;
        this.lifeTime = lifetime;
    }
}