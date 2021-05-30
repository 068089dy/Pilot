using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorController : MonoBehaviour
{
    public List<Transform> patrolPoints;
    public int curPatrolTargetIndex;
    public GameObject curAttackTarget;
    // Start is called before the first frame update
    void Start()
    {
        if (patrolPoints.Count < 2)
        {
            Debug.Log("Ñ²Âßµã¹ýÉÙ");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
