using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerAnimationManager))]
public class PlayerCharacterController : MonoBehaviour
{

    public bool isGrounded;
    bool isAboveSlopeLimit;

    // �ӵط���
    Vector3 m_GroundNormal;
    CharacterController characterController;
    InputHandler m_InputHandler;
    GameFlowManager m_GameFlowManager;
    [Header("Camera Setting")]
    public Camera fpCamera;

    [Header("Check Ground Setting")]
    // �ӵ��ж�ʱ�����������ľ��룬�ڵ����Ϻ��ڿ���ʱ��һ�����ڿ���ʱҪ��һ�㡣
    public float groundCheckDistance = 0.05f;
    public float groundCheckDistanceInAir = 0.07f;
    public LayerMask groundCheckLayers = -1;

    [Header("Rotate Setting")]
    public Transform cameraRotateHelper;
    public float rotationSpeed = 50f;
    public float RotationMultiplier
    {
        get
        {
            if (m_PlayerWeaponManager.isAiming)
            {
                if (m_PlayerWeaponManager.curWeaponIndex == 0)
                    return 0.5f;
                else if (m_PlayerWeaponManager.curWeaponIndex == 1)
                    return 0.3f;
            }

            

            return 1f;
        }
    }
    float m_CameraVerticalAngle = 0f;

    [Header("Move Setting")]
    public float maxSpeedOnGround = 10f;
    public float jumpForce = 10f;
    public float gravityDownForce = 10f;
    [Tooltip("���м��ٶ�")]
    public float accelerationSpeedInAir = 25f;
    [Tooltip("��������ٶ�")]
    public float maxSpeedInAir = 10f;
    public float speedModifier
    {
        get
        {
            if (m_PlayerWeaponManager.isAiming)
            {
                return 0.5f;
            }
            if (m_PlayerWeaponManager.isShooting)
            {
                return 0.5f;
            }
            if (m_PlayerWeaponManager.isLoading)
            {
                return 0.5f;
            }
            if (m_InputHandler.GetRunInputHeld())
            {
                return 1.6f;
            }
            return 1f;
        }
    }

    bool canDoubleJump = true;

    float movementSharpnessHooking = 7f;
    float movementSharpnessOnGround = 30f;
    [Tooltip("��ɫ�ٶȣ�ֻ��")]
    public Vector3 characterVelocity;

    // ���һ������ʱ��
    float m_LastTimeJumped;
    // ����׼��ʱ�䣬������׼��ʱ���ڣ������ӵ��жϣ���Ȼ���ڸ�����ʱ���н�ɫ���ڵ��棬����޷�����
    float k_JumpGroundingPreventionTime = 0.2f;

    // Hook
    Hook m_Hook;
    // Health
    Health m_Health;
    // ����
    PlayerAnimationManager m_PlayerAnimationManager;
    PlayerWeaponManager m_PlayerWeaponManager;

    // ��һ֡�Ƿ�
    bool wasGrounded;
    [System.NonSerialized]
    public bool isRuning;
    bool isHooking;
    bool wasHooked;
    bool isJumped;

    // ��Ч���
    [Tooltip("��Ч���")]
    public AudioSource m_AudioSource;
    public AudioClip footStepSFX;
    // �����
    public AudioClip landSFX;
    // �Ų���Ƶ��
    float footstepSFXFrequency = 2.5f;
    float m_footstepDistanceCounter;
    // ������
    public AudioClip jumpSFX;
    public AudioClip doubleJumpSFX;
    // ������
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        m_InputHandler = GetComponent<InputHandler>();
        m_Hook = GetComponent<Hook>();
        m_Health = GetComponent<Health>();
        m_GameFlowManager = FindObjectOfType<GameFlowManager>();
        m_PlayerAnimationManager = GetComponent<PlayerAnimationManager>();
        m_PlayerWeaponManager = GetComponent<PlayerWeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        wasGrounded = isGrounded;
        isJumped = false;
        CheckGround();
        if (m_Health.hp > 0)
        {
            HandleCharacterMovement();
        } else
        {
            HandleCharacterMovement();
            //FreeMovement();
            //m_GameFlowManager.GameOver();
        }
    }

    // ��ȡ������ײ���������
    Vector3 GetCapsuleBottomHemisphere()
    {
        return transform.position + (transform.up * characterController.radius);
    }

    // ��ȡ�����嶥����������
    Vector3 GetCapsuleTopHemisphere()
    {
        return transform.position + (transform.up * (characterController.height - characterController.radius));
    }

    bool CheckGround()
    {
        // �ж��Ƿ��ڵ���ʱ����Ҫ����������������һ�ξ��룬�����ⶶ����
        float curGroundCheckDistance = isGrounded ? (characterController.skinWidth + groundCheckDistance) : groundCheckDistanceInAir;
        isGrounded = false;
        isAboveSlopeLimit = false;
        // ������Ǹ������������ӵ��ж�
        if (Time.time > m_LastTimeJumped + k_JumpGroundingPreventionTime)
        {
            
            if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(), characterController.radius, Vector3.down, out RaycastHit hit, curGroundCheckDistance, groundCheckLayers, QueryTriggerInteraction.Ignore))
            {
                m_GroundNormal = hit.normal;
                
                if (Vector3.Dot(hit.normal, transform.up) > 0)
                {
                    if (Vector3.Angle(m_GroundNormal, transform.up) <= characterController.slopeLimit)
                    {
                        isGrounded = true;
                        // characterController���ܻ�������ײ�����ƶ�һЩ��������Ҫ���������ƶ�һЩ��
                        if (hit.distance > characterController.skinWidth)
                        {
                            characterController.Move(Vector3.down * hit.distance);
                        }
                    } else if (Vector3.Angle(m_GroundNormal, transform.up) < 90f)
                    {
                        // �������б����
                        isAboveSlopeLimit = true;
                        //characterController.Move(Vector3.down * gravityDownForce * Time.deltaTime);
                    }
                    
                    
                }
            }
        } else
        {
            isJumped = true;
        }
        return isGrounded;
    }

    // �����������˶�
    void FreeMovement()
    {
        if (!isGrounded)
        {
            characterVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
            characterController.Move(characterVelocity * Time.deltaTime);
        }
    }

    

    void HandleCharacterMovement()
    {
        TryIdle();
        transform.Rotate(new Vector3(0f, m_InputHandler.GetLookInputHorizontal() * rotationSpeed * RotationMultiplier, 0f));
        {
            m_CameraVerticalAngle += m_InputHandler.GetLookInputVertical() * rotationSpeed * RotationMultiplier;
            m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, -89f, 89f);
            cameraRotateHelper.transform.localEulerAngles = new Vector3(m_CameraVerticalAngle, 0, 0);
        }
        // ���������ƶ�����ˮƽ��
        Vector3 worldspaceMoveInput = transform.TransformVector(m_InputHandler.GetMoveInput());
        wasHooked = isHooking;
        isHooking = m_Hook && m_Hook.hooking && m_Hook.hookTargetObject;
        if (isHooking)
        {
            // �ȼ���ץ���ٶ�
            Vector3 hookRootPos = transform.position;
            Vector3 targetVelocity = (m_Hook.hookTargetObject.transform.position - hookRootPos).normalized * m_Hook.maxHookSpeed;
            targetVelocity = targetVelocity + characterVelocity;
            targetVelocity = Vector3.ClampMagnitude(targetVelocity, m_Hook.maxHookSpeed);
            characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, movementSharpnessHooking * Time.deltaTime);

            // ץ��״̬�£�����ڵ�����Ȼ������
            if (isGrounded)
            {
                
                if (!wasGrounded)
                {
                    m_AudioSource.PlayOneShot(landSFX);
                }
                canDoubleJump = true;
                // ��Ծ
                if (isGrounded && m_InputHandler.GetJumpInputDown())
                {
                    m_AudioSource.PlayOneShot(jumpSFX);
                    characterVelocity = new Vector3(characterVelocity.x, 0f, characterVelocity.z);
                    characterVelocity += Vector3.up * jumpForce;
                    m_LastTimeJumped = Time.time;
                    m_GroundNormal = Vector3.up;
                    isJumped = true;
                }
            }
            // ����ڿ��еĻ�����Ҫ���Ͽ��п����߼�
            else
            {
                Vector3 moveDir = m_InputHandler.GetMoveInput();
                if (Vector3.Dot(characterController.velocity, transform.forward) > maxSpeedInAir)
                {
                    moveDir.z = 0;
                }
                if (Vector3.Dot(characterController.velocity, -transform.forward) > maxSpeedInAir)
                {
                    moveDir.z = 0;
                }
                if (Vector3.Dot(characterController.velocity, transform.right) > maxSpeedInAir)
                {
                    moveDir.x = 0;
                }
                if (Vector3.Dot(characterController.velocity, -transform.right) > maxSpeedInAir)
                {
                    moveDir.x = 0;
                }
                worldspaceMoveInput = transform.TransformVector(moveDir);
                // ===========================================

                /* 
                 * �õ�ǰ�Ľ�ɫ�ٶȷ���characterController����Ľ������ΪcharacterController��������㡣
                 * ������ʱ�򣨱��翨��б�½�ʱ��character Controller����y�᷽����һ��ͻ�䣩
                 */
                //characterVelocity = characterController.velocity;

                characterVelocity += worldspaceMoveInput * accelerationSpeedInAir * Time.deltaTime;
                float verticalVelocity = characterVelocity.y;
                Vector3 horizontalVelocity = Vector3.ProjectOnPlane(characterVelocity, Vector3.up);
                //horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeedInAir * speedModifier);
                characterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);
                characterVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
                // ������
                if (m_InputHandler.GetJumpInputDown() && canDoubleJump)
                {
                    DoubleJump();
                }
            }

            // �����Ե�ǰ�ٶȣ�����Ŀ��λ������ʱ�䣬���ץ��ʱ�䳬�������ʱ����ֹͣhooking״̬
            //m_Hook.expectedHookTime = (m_Hook.hookTargetObject.transform.position - hookRootPos).magnitude / characterVelocity.magnitude;
            if (((hookRootPos - m_Hook.hookTargetObject.transform.position).sqrMagnitude < 1f)
                //(Time.time - m_Hook.lastHookTime > m_Hook.expectedHookTime)
                )
            {
                m_Hook.hooking = false;
                m_Hook.RecyclingHook();
            }
            
        }
        else if (isGrounded)
        {
            
            if (!wasGrounded)
            {
                m_AudioSource.PlayOneShot(landSFX);
            }
            canDoubleJump = true;
            // ����Ŀ���ٶ�
            Vector3 targetVelocity = worldspaceMoveInput * maxSpeedOnGround * speedModifier;
            targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, m_GroundNormal) * targetVelocity.magnitude;
            // ��������ٶ�
            targetVelocity = Vector3.ClampMagnitude(targetVelocity, 30f);
            // ��ɫ�ٶȲ�ֵ
            characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, movementSharpnessOnGround * Time.deltaTime);
            // ��Ծ
            if (isGrounded && m_InputHandler.GetJumpInputDown())
            {
                m_AudioSource.PlayOneShot(jumpSFX);
                characterVelocity = new Vector3(characterVelocity.x, 0f, characterVelocity.z);
                characterVelocity += Vector3.up * jumpForce *1.5f;
                m_LastTimeJumped = Time.time;
                m_GroundNormal = Vector3.up;
                isJumped = true;
            } else
            {
                // �ܶ���
                if (m_InputHandler.GetRunInputHeld() && worldspaceMoveInput.sqrMagnitude > 0)
                {
                    m_PlayerAnimationManager.Run();
                }
                if (m_footstepDistanceCounter >= 1f / footstepSFXFrequency)
                {
                    m_footstepDistanceCounter = 0f;
                    m_AudioSource.PlayOneShot(footStepSFX);
                }

                // keep track of distance traveled for footsteps sound
                m_footstepDistanceCounter += worldspaceMoveInput.magnitude * speedModifier * Time.deltaTime;
            }
        } else
        {
            /* ���иı��ٶ�
            ����
                1.��Ծʱ������΢���ٶ�
                2.���ˮƽ�ٶȹ��󣬺����û�����
            */
            // == ����ˮƽ�ٶȣ�ˮƽ�ٶȹ��󣬺����û����� ==
            Vector3 moveDir = m_InputHandler.GetMoveInput();
            if (Vector3.Dot(characterController.velocity, transform.forward) > maxSpeedInAir)
            {
                moveDir.z = 0;
            }
            if (Vector3.Dot(characterController.velocity, -transform.forward) > maxSpeedInAir)
            {
                moveDir.z = 0;
            }
            if (Vector3.Dot(characterController.velocity, transform.right) > maxSpeedInAir)
            {
                moveDir.x = 0;
            }
            if (Vector3.Dot(characterController.velocity, -transform.right) > maxSpeedInAir)
            {
                moveDir.x = 0;
            }
            worldspaceMoveInput = transform.TransformVector(moveDir);
            // ===========================================

            /* 
             * �õ�ǰ�Ľ�ɫ�ٶȷ���characterController����Ľ������ΪcharacterController��������㡣
             * ������ʱ�򣨱��翨��б�½�ʱ��character Controller����y�᷽����һ��ͻ�䣩
             */
            // �����һ֡��ץ��״̬������character���ٶ�
            if (wasHooked)
            {
                characterVelocity = characterController.velocity;
            }
            // �����ɫ��ʵ���ٶ�С��Ŀ���ٶȣ�˵����ɫ�ڿ�����ײ������
            // y��������ʱҲ��һ���ģ������ɫ�����˿��У���ô����gravityDownForce���¼��ٵĻ�������ɫ��ĳһʱ�����뿨ס��ʱ�򣬽�ɫ��һ�����ĵ�������
            if (characterController.velocity.sqrMagnitude < characterVelocity.sqrMagnitude)
            {
                characterVelocity = characterController.velocity;
            }
            characterVelocity += worldspaceMoveInput * accelerationSpeedInAir * Time.deltaTime;
            float verticalVelocity = characterVelocity.y;
            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(characterVelocity, Vector3.up);
            //horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeedInAir * speedModifier);
            characterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);
            characterVelocity += Vector3.down * gravityDownForce * Time.deltaTime;

            // ����б�ʳ������Ƶ�б���ϣ�����б���»�
            if (isAboveSlopeLimit)
            {
                //characterVelocity
                float l = 1 / Mathf.Cos(Vector3.Angle(m_GroundNormal, Vector3.up));
                characterVelocity += (Vector3.down * l + m_GroundNormal).normalized * Time.deltaTime;
            }

            // ������
            if (m_InputHandler.GetJumpInputDown() && canDoubleJump)
            {
                DoubleJump();
            }
        }
        characterController.Move(characterVelocity * Time.deltaTime);
        // 
        //if (!isGrounded && 
        //    (characterController.velocity.y - characterVelocity.y) / Time.deltaTime <= -100)
        //{
        //    Debug.Log((characterController.velocity.y - characterVelocity.y) / Time.deltaTime + "y�����ٶ��쳣ͻ��1");
        //    characterController.Move(new Vector3(0, -characterController.velocity.y, 0) * Time.deltaTime);
        //}
        // (�ѽ��)����б�½�ʱ��character Controller���ٶȻ���y�᷽����һ��ͻ�䣬������һ���ж�
        //if (wasGrounded && !isGrounded && !isHooking && !isJumped && characterController.velocity.y > 0)
        //{
        //    Debug.Log(characterController.velocity.y + characterVelocity.y + "y�����ٶ��쳣ͻ��");
        //    characterController.Move(new Vector3(0, -characterController.velocity.y, 0) * Time.deltaTime);
        //}
    }

    public void initPlayer()
    {
        Debug.Log("��ʼ��");
        m_CameraVerticalAngle = 0f;
        wasHooked = false;
        isHooking = false;
        characterVelocity = Vector3.zero;
        m_PlayerWeaponManager.isAiming = false;
        m_PlayerWeaponManager.setWeapon(0);
    }


    void DoubleJump()
    {
        m_AudioSource.PlayOneShot(doubleJumpSFX);
        canDoubleJump = false;
        characterVelocity = new Vector3(characterVelocity.x, characterVelocity.y + jumpForce * 1.5f, characterVelocity.z);
        isJumped = true;
    }

    void TryIdle()
    {
        m_PlayerAnimationManager.Idle();
        if (m_PlayerWeaponManager.isLoading)
        {
            m_PlayerAnimationManager.Reload();
        }
        if (m_PlayerWeaponManager.isAiming)
        {
            m_PlayerAnimationManager.Aim();
        }
        if (m_PlayerWeaponManager.isShooting)
        {
            m_PlayerAnimationManager.Shoot();
        } else
        {
            m_PlayerAnimationManager.StopShoot();
        }
        
    }

    // ��ȡ��ɫ�ƶ�������¶ȷ���
    public Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
    {
        Vector3 directionRight = Vector3.Cross(direction, transform.up);
        return Vector3.Cross(slopeNormal, directionRight).normalized;
    }

    public void OnDrawGizmos()
    {
        //Debug.DrawLine(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(), Color.red);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(GetCapsuleTopHemisphere(), 1f);
    }
    
}
