using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyController : MonoBehaviour
{
    public float flightHeight = 20;
    public float takeoffSpeed = 10;

    public GameObject DestroyFXPrefab;
    Health m_Health;
    // Start is called before the first frame update
    void Start()
    {
        m_Health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Health.hp <= 0)
        {
            Death();
        }
        
    }

    void Death()
    {
        if (DestroyFXPrefab)
        {
            GameObject newFX = Instantiate(DestroyFXPrefab, transform.position, Quaternion.identity);
            Destroy(newFX, 10);
        }
        Destroy(gameObject);
    }
}
