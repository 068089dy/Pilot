using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AnimationControl))]
public class CharacterControl : MonoBehaviour
{
    public GameObject rotateHelper;
    public float mouse_sensitivity = 50;
    public float speed = 5;
    public float gravity = 10;
    public float jumpSpeed = 10;
    public Camera camera;

    Vector3 moveDir;
    [System.NonSerialized]
    int flootLayerMask = 1 << 9|0<<9;
    public float v_Velocity = 0;
    bool doubleJumpFlag = false;
    CharacterController m_Controller;
    AnimationControl animationControl;
    HookControl hookControl;
    AudioControl audioControl;
    bool isGrounded = false;
    Vector3 planeNormal;
    float lastStartJumpTime;
    
    
    public enum State
    {
        Move,
        Jump,
        Hook,
        Aim,
        Reload,
        Died
    }
    [System.NonSerialized]
    public int curState;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        curState = (int)State.Move;
        m_Controller = GetComponent<CharacterController>();
        animationControl = GetComponent<AnimationControl>();
        hookControl = GetComponent<HookControl>();
        audioControl = GetComponent<AudioControl>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckOnGround();
        HandleMouse();
        if (hookControl != null)
        {
            hookControl.enabled = true;
        }
        
        if (curState == (int)State.Move)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 60f, Time.deltaTime * 7f);
            animationControl.Idle();
            pitchControl();
            transform.Rotate(Vector3.up, mouse_sensitivity * Time.deltaTime * Input.GetAxis("Mouse X"), Space.World);
            HandleMove(speed);
            HandleAimAndShoot();
        } else if (curState == (int)State.Hook)
        {
            animationControl.Hook();
            pitchControl();
        } else if (curState == (int)State.Aim)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 20f, Time.deltaTime * 7f);
            animationControl.Aim();
            pitchControl();
            transform.Rotate(Vector3.up, mouse_sensitivity * Time.deltaTime * Input.GetAxis("Mouse X"), Space.World);
            HandleMove(speed * 0.5f);
            HandleAimAndShoot();
        } else if (curState == (int)State.Reload)
        {
            if (hookControl != null)
            {
                hookControl.enabled = false;
            }
            animationControl.Reload();
            AnimatorStateInfo info = animationControl.animator.GetCurrentAnimatorStateInfo(0);
            if (info.normalizedTime >= 1.0f && info.IsName("FPRifleReload"))
            {
                curState = (int)State.Move;
            }
            pitchControl();
            transform.Rotate(Vector3.up, mouse_sensitivity * Time.deltaTime * Input.GetAxis("Mouse X"), Space.World);
            HandleMove(speed * 0.5f);
            HandleAimAndShoot();
        } else if (curState == (int)State.Died)
        {

        }
        
    }

    void HandleMouse()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    void pitchControl()
    {
        if (rotateHelper != null)
        {

            Vector3 euler = rotateHelper.transform.eulerAngles;
            euler.x += -Input.GetAxis("Mouse Y") * mouse_sensitivity * Time.deltaTime;
            if (euler.x > 180)
            {
                euler.x -= 360;
            }
            euler.x = Mathf.Clamp(euler.x, -90, 90);

            rotateHelper.transform.eulerAngles = euler;
        }
    }

    void HandleAimAndShoot()
    {
        if (Input.GetMouseButtonDown(1))
        {

            if (curState == (int)State.Aim)
            {
                curState = (int)State.Move;
            }
            else
            {
                curState = (int)State.Aim;
                
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            //camera.transform.forward();
            Ray ray = new Ray(camera.transform.position, camera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit info))
            {
                GameObject ob = info.transform.gameObject;
                RagdollControl ragdollControl = ob.GetComponent<RagdollControl>();
                if (ragdollControl != null)
                {
                    ragdollControl.beAttacked(1, ray.direction);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            curState = (int)State.Reload;
        }
    }
    
    void HandleMove(float speed)
    {
        Vector3 velocity = m_Controller.velocity;
        // 除了判断是否在地面，还要判断最后一次按下空格的时间是否在最近
        if (isGrounded && lastStartJumpTime + 0.05f < Time.time)
        {
            bool justDown = false;
            if (v_Velocity < -3)
            {
                justDown = true;
            }
            //if (v_Velocity < -20)
            //{
            //    Debug.Log("摔死了"+v_Velocity);
            //    curState = (int)State.Died;
            //}
            doubleJumpFlag = false;
            v_Velocity = 0;
            moveDir = (Input.GetAxis("Vertical") * Vector3.forward + Input.GetAxis("Horizontal") * Vector3.right).normalized;
            moveDir = transform.TransformDirection(moveDir);
            velocity = moveDir * speed;

            if (moveDir.magnitude > 0)
            {
                animationControl.Walk();
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    velocity *= 1.5f;
                    animationControl.Run();
                    audioControl.Run();
                }
                else
                {
                    audioControl.Walk();
                }
            }
            // 如果刚落下
            if (justDown)
            {
                audioControl.Down();
            }
            
            
            // 惯性只针对xz方向
            //velocity.x = Mathf.Lerp(m_Controller.velocity.x, velocity.x, Time.deltaTime * 30f);
            //velocity.z = Mathf.Lerp(m_Controller.velocity.z, velocity.z, Time.deltaTime * 30f);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                v_Velocity = jumpSpeed;
                velocity.y = v_Velocity;
                lastStartJumpTime = Time.time;
            }
            else
            {
                velocity = Vector3.ProjectOnPlane(velocity, planeNormal);
            }
            
        } else
        {
            //moveDir = (Input.GetAxis("Vertical") * Vector3.forward + Input.GetAxis("Horizontal") * Vector3.right).normalized;
            //moveDir = transform.TransformDirection(moveDir);
            //// 在空中，玩家移动角色时，速度减半
            //Vector3 t_velocity = moveDir * speed * 0.5f;
            velocity = m_Controller.velocity;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!doubleJumpFlag)
                {
                    v_Velocity = jumpSpeed;
                    doubleJumpFlag = true;
                }
            }
            else
            {
                v_Velocity -= gravity * Time.deltaTime;
            }
            velocity.y = v_Velocity;
            Debug.Log("Stop hook1 " + m_Controller.velocity.y);
            m_Controller.Move(velocity * Time.deltaTime);
        }
        m_Controller.Move(velocity * Time.deltaTime);
    }

    Vector3 GetCapsuleBottomHemisphere()
    {
        return transform.position + (transform.up * m_Controller.radius);
    }
    
    Vector3 GetCapsuleTopHemisphere(float atHeight)
    {
        return transform.position + (transform.up * (atHeight - m_Controller.radius));
    }

    void CheckOnGround()
    {
        if (m_Controller.isGrounded)
        {
            isGrounded = true;
            return;
        }
        if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(m_Controller.height), m_Controller.radius + m_Controller.skinWidth, Vector3.down, out RaycastHit hit, 0.1f, flootLayerMask, QueryTriggerInteraction.Ignore))
        {
            
            if (Vector3.Dot(hit.normal, Vector3.up) > 0.2)
            {
                isGrounded = true;
                planeNormal = hit.normal;
            } else
            {
                isGrounded = false;
            }
        } else
        {
            isGrounded = false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        planeNormal = hit.normal;
    }
}

