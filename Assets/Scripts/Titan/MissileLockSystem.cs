using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class MissileLockSystem : MonoBehaviour
{
    DamageCounter damageCounter;
    public Actor curTarget;
    //public ConcurrentQueue<Actor> targetQueue = new ConcurrentQueue<Actor>();
    //public ArrayList targetList = ArrayList.Synchronized(new ArrayList(5));
    // Start is called before the first frame update
    void Start()
    {
        damageCounter = GetComponent<DamageCounter>();
        damageCounter.DamageAction += setCurTarget;
    }

    void setCurTarget(DamageMsg damageMsg)
    {
        if (damageMsg.protectileType == ProtectileType.RIFLE)
            curTarget = damageMsg.target;
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (Actor ac in targetList) {
        //    Debug.Log(ac.name);
        //}
    }

    
}
