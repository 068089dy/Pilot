using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class RobotMovement : MonoBehaviour
{
    CharacterController m_Controller;
    public float speed = 10f;

    
    public Transform patrolTarget1;
    public Transform patrolTarget2;
    public float distance = 1;
    private Transform curTarget;
    private float v_Velocity = -1;
    private Animator animator;

    private bool isGrounded;
    private Vector3 planeNormal;
    // Start is called before the first frame update
    void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        curTarget = patrolTarget1;
        animator.SetBool("walk", true);
        animator.SetBool("down", false);
        animator.SetBool("inAir", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CheckOnGround();
        patrol();
    }

   
    

    private void patrol()
    {
        if (patrolTarget1 == null || patrolTarget2 == null)
        {
            return;
        }
        Vector3 velocity = Vector3.zero;
        if (isGrounded)
        {
            //if (v_Velocity < -5)
            //{
            //    animator.SetBool("down", true);
            //    animator.SetBool("walk", false);
            //    animator.SetBool("inAir", false);
            //} else
            //{
            //    animator.SetBool("walk", true);
            //    animator.SetBool("down", false);
            //    animator.SetBool("inAir", false);
            //}
            
            v_Velocity = -1;
            if (Vector3.Distance(transform.position, curTarget.position) > distance)
            {
                velocity = transform.forward * speed;
                transform.forward = Vector3.Lerp(transform.forward, new Vector3(
                    (curTarget.position - transform.position).x,
                    0,
                    (curTarget.position - transform.position).z
                    ), Time.deltaTime * 0.5f);

            }
            else
            {
                
                if (curTarget == patrolTarget1)
                {
                    curTarget = patrolTarget2;
                }
                else
                {
                    curTarget = patrolTarget1;
                }
            }
        } else
        {
            //animator.SetBool("inAir", true);
            //animator.SetBool("walk", false);
            //animator.SetBool("down", false);
            velocity = m_Controller.velocity;
            v_Velocity -= 10 * Time.deltaTime;
        }
        velocity.y = v_Velocity;
        m_Controller.Move(velocity * Time.deltaTime);
    }

    Vector3 GetCapsuleBottomHemisphere()
    {
        return transform.position + (transform.up * m_Controller.radius);
    }

    Vector3 GetCapsuleTopHemisphere()
    {
        return transform.position + (transform.up * (m_Controller.height - m_Controller.radius));
    }

    void CheckOnGround()
    {
        if (m_Controller.isGrounded)
        {
            isGrounded = true;
            return;
        }
        if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(), m_Controller.radius + m_Controller.skinWidth, Vector3.down, out RaycastHit hit, 0.1f, -1, QueryTriggerInteraction.Ignore))
        {

            if (Vector3.Dot(hit.normal, Vector3.up) > 0.2)
            {
                isGrounded = true;
                planeNormal = hit.normal;
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = false;
        }
    }
}
