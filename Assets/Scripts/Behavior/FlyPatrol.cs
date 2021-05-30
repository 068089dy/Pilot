using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[RequireComponent(typeof(EnemyAnimationController))]
[RequireComponent(typeof(CharacterController))]
public class FlyPatrol : Action
{
    EnemyAnimationController m_EnemyAnimationController;
    EnemyBehaviorController m_EnemyBehaviorController;
    CharacterController m_CharacterController;
    List<Transform> patrolPoints;
    public float pointDistanceLimit = 0.5f;

    // ÒÆ¶¯ËÙ¶È
    public float speed = 3;
    public float rotationSpeed = 0.5f;
    
    Vector2 target2D;
    Vector2 self2D;
    // Start is called before the first frame update
    public override void OnStart()
    {
        m_EnemyAnimationController = GetComponent<EnemyAnimationController>();
        m_EnemyBehaviorController = GetComponent<EnemyBehaviorController>();
        patrolPoints = m_EnemyBehaviorController.patrolPoints;
        m_CharacterController = GetComponent<CharacterController>();
        if (patrolPoints.Count > 0)
        {
            target2D = new Vector2(patrolPoints[m_EnemyBehaviorController.curPatrolTargetIndex].position.x, patrolPoints[m_EnemyBehaviorController.curPatrolTargetIndex].position.z);
        } else
        {
            target2D = new Vector2(transform.position.x, transform.position.z);
        }
        
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        m_EnemyAnimationController.Fly();
        self2D = new Vector2(transform.position.x, transform.position.z);
        if (Vector2.Distance(self2D, target2D) < pointDistanceLimit)
        {
            if (m_EnemyBehaviorController.curPatrolTargetIndex + 2 > patrolPoints.Count)
            {
                m_EnemyBehaviorController.curPatrolTargetIndex = 0;
            } else
            {
                m_EnemyBehaviorController.curPatrolTargetIndex++;
            }
            target2D = new Vector2(patrolPoints[m_EnemyBehaviorController.curPatrolTargetIndex].position.x, patrolPoints[m_EnemyBehaviorController.curPatrolTargetIndex].position.z);
        }
        
        if (transform.position.y < 10)
        {
            m_CharacterController.Move(transform.up * speed * Time.deltaTime);
        }
        //transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
        transform.forward = Vector3.Lerp(transform.forward, new Vector3(
                    target2D.x - transform.position.x,
                    0,
                    target2D.y - transform.position.z
                    ), Time.deltaTime * rotationSpeed);
        if (Vector3.Angle(transform.forward,
            new Vector3(
                    target2D.x - transform.position.x,
                    0,
                    target2D.y - transform.position.z
                    )) <  10)
        {
            m_CharacterController.Move(transform.forward * speed * Time.deltaTime);
        }
        //foreach (Transform point in patrolPoints)
        //{

        //    Vector2 self2D = new Vector2(transform.position.x, transform.position.z);

        //}
        return TaskStatus.Running;
    }
}
