using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyController : MonoBehaviour
{
    public float flightHeight = 20;
    public float takeoffSpeed = 10;

    public GameObject destroyFX;
    Actor actor;
    TeamManager teamManager;
    Health m_Health;
    // Start is called before the first frame update
    void Start()
    {
        teamManager = FindObjectOfType<TeamManager>();
        actor = GetComponent<Actor>();
        if (destroyFX)
            destroyFX.SetActive(false);
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
        if (destroyFX)
        {
            //GameObject newFX = Instantiate(DestroyFXPrefab, transform.position, Quaternion.identity);
            destroyFX.SetActive(true);
            destroyFX.transform.SetParent(null, true);
            Destroy(destroyFX, 10);
        }
        if (teamManager)
        {
            teamManager.UnRegisterActor(actor);
        }
        Destroy(gameObject);
    }
}
