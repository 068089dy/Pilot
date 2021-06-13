using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanTread : MonoBehaviour
{
    public Actor parentActor;
    public float damage = 1000;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnTriggerEnter(Collider other)
    //{

    //}

    private void OnTriggerEnter(Collider other)
    {
        
        if (parentActor)
        {
            if (other.gameObject.GetComponent<Damagable>())
            {
                //Debug.Log("踩到了" + other.gameObject.name);
                if (other.gameObject.GetComponent<Damagable>().parentActor.team != parentActor.team
                    && other.gameObject.GetComponent<Damagable>().parentActor.characterType == CharacterType.ROBOT)
                {
                    Debug.Log("踩到了" + other.gameObject.name);
                    AttackMsg attackMsg = new AttackMsg(
                        damage,
                        parentActor,
                        ProtectileType.TREAD
                        );
                    DamageMsg damageMsg = other.gameObject.GetComponent<Damagable>().BeHurt(attackMsg);
                    parentActor.damageCounter.AddAmount(damageMsg);
                }
            }
        }
    }
    
}
