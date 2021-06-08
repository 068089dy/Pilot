using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class DamageCounter : MonoBehaviour
{
    public float damageAmount;
    public float killedAmount;
    public UnityAction<DamageMsg> DamageAction;
    public UnityAction<DamageMsg> KilledAction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddAmount(DamageMsg damageMsg)
    {
        damageAmount += damageMsg.damage;
        if (damageMsg.isKilled)
        {
            killedAmount++;
            KilledAction?.Invoke(damageMsg);
        }
        if (damageMsg.damage > 0)
        {
            DamageAction?.Invoke(damageMsg);
        }
    }
}
