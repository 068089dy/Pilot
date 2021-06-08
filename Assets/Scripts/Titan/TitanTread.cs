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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("≤»µΩ¡À" + collision.gameObject.name);
        if (parentActor)
        {
            if (collision.gameObject.GetComponent<Damagable>())
            {
                if (collision.gameObject.GetComponent<Damagable>().parentActor.team != parentActor.team 
                    && parentActor.characterType == CharacterType.ROBOT)
                {
                    AttackMsg attackMsg = new AttackMsg(
                        damage,
                        parentActor,
                        ProtectileType.TREAD
                        );
                    DamageMsg damageMsg = collision.gameObject.GetComponent<Damagable>().BeHurt(attackMsg);
                    parentActor.damageCounter.AddAmount(damageMsg);
                }
            }
        }
    }
}
