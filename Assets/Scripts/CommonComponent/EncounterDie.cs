using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterDie : MonoBehaviour
{
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
        if (collision.transform.gameObject.GetComponent<Health>())
        {
            Health m_Health = collision.transform.gameObject.GetComponent<Health>();
            m_Health.hp = 0;
        }
    }
}
