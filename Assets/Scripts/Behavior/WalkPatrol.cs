using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkPatrol : Action
{
    EnemyAnimationController m_EnemyAnimationController;
    EnemyBehaviorController m_EnemyBehaviorController;
    CharacterController m_CharacterController;
    List<Transform> patrolPoints;
    // 距离目标点多远就算到达
    public float pointDistanceLimit = 0.5f;

    // 移动速度
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
        }
        else
        {
            target2D = new Vector2(transform.position.x, transform.position.z);
        }

    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        if (m_CharacterController.velocity.sqrMagnitude > 0)
        {
            m_EnemyAnimationController.Walk();
        }
        self2D = new Vector2(transform.position.x, transform.position.z);
        // 水平方向到达目标点，则切换下一个目标
        if (Vector2.Distance(self2D, target2D) < pointDistanceLimit)
        {
            if (m_EnemyBehaviorController.curPatrolTargetIndex + 2 > patrolPoints.Count)
            {
                m_EnemyBehaviorController.curPatrolTargetIndex = 0;
            }
            else
            {
                m_EnemyBehaviorController.curPatrolTargetIndex++;
            }
            target2D = new Vector2(patrolPoints[m_EnemyBehaviorController.curPatrolTargetIndex].position.x, patrolPoints[m_EnemyBehaviorController.curPatrolTargetIndex].position.z);
        }

        // 转向
        transform.forward = Vector3.Lerp(transform.forward, new Vector3(
                    target2D.x - transform.position.x,
                    0,
                    target2D.y - transform.position.z
                    ), Time.deltaTime * rotationSpeed);
        // 接近面向目标时，才移动
        if (Vector3.Angle(transform.forward,
            new Vector3(
                    target2D.x - transform.position.x,
                    0,
                    target2D.y - transform.position.z
                    )) < 10)
        {
            m_CharacterController.Move(transform.forward * speed * Time.deltaTime);
        }
        // 落地
        if (!m_CharacterController.isGrounded)
        {
            m_CharacterController.Move(Vector3.down * speed * Time.deltaTime);
        }
        //foreach (Transform point in patrolPoints)
        //{

        //    Vector2 self2D = new Vector2(transform.position.x, transform.position.z);

        //}
        return TaskStatus.Running;
    }
}
