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

    // 接地法线
    Vector3 m_GroundNormal;
    CharacterController characterController;
    InputHandler m_InputHandler;
    GameFlowManager m_GameFlowManager;
    [Header("Camera Setting")]
    public Camera fpCamera;

    [Header("Check Ground Setting")]
    // 接地判断时，向地面延申的距离，在地面上和在空中时不一样，在空中时要长一点。
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
    [Tooltip("空中加速度")]
    public float accelerationSpeedInAir = 25f;
    [Tooltip("空中最大速度")]
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
    [Tooltip("角色速度，只读")]
    public Vector3 characterVelocity;

    // 最后一次起跳时间
    float m_LastTimeJumped;
    // 起跳准备时间，在起跳准备时间内，不做接地判断，不然会在刚起跳时误判角色还在地面，造成无法起跳
    float k_JumpGroundingPreventionTime = 0.2f;

    // Hook
    Hook m_Hook;
    // Health
    Health m_Health;
    // 动作
    PlayerAnimationManager m_PlayerAnimationManager;
    PlayerWeaponManager m_PlayerWeaponManager;

    // 上一帧是否
    bool wasGrounded;
    [System.NonSerialized]
    public bool isRuning;
    bool isHooking;
    bool wasHooked;
    bool isJumped;

    // 音效相关
    [Tooltip("音效相关")]
    public AudioSource m_AudioSource;
    public AudioClip footStepSFX;
    // 落地声
    public AudioClip landSFX;
    // 脚步声频率
    float footstepSFXFrequency = 2.5f;
    float m_footstepDistanceCounter;
    // 起跳声
    public AudioClip jumpSFX;
    public AudioClip doubleJumpSFX;
    // 二段跳
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

    // 获取胶囊体底部半球球心
    Vector3 GetCapsuleBottomHemisphere()
    {
        return transform.position + (transform.up * characterController.radius);
    }

    // 获取胶囊体顶部半球球心
    Vector3 GetCapsuleTopHemisphere()
    {
        return transform.position + (transform.up * (characterController.height - characterController.radius));
    }

    bool CheckGround()
    {
        // 判断是否在地面时，需要将胶囊体向下延申一段距离，来避免抖动。
        float curGroundCheckDistance = isGrounded ? (characterController.skinWidth + groundCheckDistance) : groundCheckDistanceInAir;
        isGrounded = false;
        isAboveSlopeLimit = false;
        // 如果不是刚起跳，才做接地判断
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
                        // characterController可能会由于碰撞往上移动一些，这里需要往下稍稍移动一些。
                        if (hit.distance > characterController.skinWidth)
                        {
                            characterController.Move(Vector3.down * hit.distance);
                        }
                    } else if (Vector3.Angle(m_GroundNormal, transform.up) < 90f)
                    {
                        // 如果卡在斜坡上
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

    // 死亡后自由运动
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
        // 世界坐标移动方向（水平）
        Vector3 worldspaceMoveInput = transform.TransformVector(m_InputHandler.GetMoveInput());
        wasHooked = isHooking;
        isHooking = m_Hook && m_Hook.hooking && m_Hook.hookTargetObject;
        if (isHooking)
        {
            // 先计算抓钩速度
            Vector3 hookRootPos = transform.position;
            Vector3 targetVelocity = (m_Hook.hookTargetObject.transform.position - hookRootPos).normalized * m_Hook.maxHookSpeed;
            targetVelocity = targetVelocity + characterVelocity;
            targetVelocity = Vector3.ClampMagnitude(targetVelocity, m_Hook.maxHookSpeed);
            characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, movementSharpnessHooking * Time.deltaTime);

            // 抓钩状态下，如果在地面依然可以跳
            if (isGrounded)
            {
                
                if (!wasGrounded)
                {
                    m_AudioSource.PlayOneShot(landSFX);
                }
                canDoubleJump = true;
                // 跳跃
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
            // 如果在空中的话还需要加上空中控制逻辑
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
                 * 让当前的角色速度符合characterController计算的结果，因为characterController由物理计算。
                 * 但是有时候（比如卡在斜坡角时，character Controller会在y轴方向有一个突变）
                 */
                //characterVelocity = characterController.velocity;

                characterVelocity += worldspaceMoveInput * accelerationSpeedInAir * Time.deltaTime;
                float verticalVelocity = characterVelocity.y;
                Vector3 horizontalVelocity = Vector3.ProjectOnPlane(characterVelocity, Vector3.up);
                //horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeedInAir * speedModifier);
                characterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);
                characterVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
                // 二段跳
                if (m_InputHandler.GetJumpInputDown() && canDoubleJump)
                {
                    DoubleJump();
                }
            }

            // 计算以当前速度，到达目标位置所需时间，如果抓钩时间超过了这个时长，停止hooking状态
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
            // 计算目标速度
            Vector3 targetVelocity = worldspaceMoveInput * maxSpeedOnGround * speedModifier;
            targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, m_GroundNormal) * targetVelocity.magnitude;
            // 限制最大速度
            targetVelocity = Vector3.ClampMagnitude(targetVelocity, 30f);
            // 角色速度插值
            characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, movementSharpnessOnGround * Time.deltaTime);
            // 跳跃
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
                // 跑动画
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
            /* 空中改变速度
            需求：
                1.跳跃时，可以微调速度
                2.如果水平速度过大，忽略用户输入
            */
            // == 限制水平速度，水平速度过大，忽略用户输入 ==
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
             * 让当前的角色速度符合characterController计算的结果，因为characterController由物理计算。
             * 但是有时候（比如卡在斜坡角时，character Controller会在y轴方向有一个突变）
             */
            // 如果上一帧是抓钩状态，续上character的速度
            if (wasHooked)
            {
                characterVelocity = characterController.velocity;
            }
            // 如果角色的实际速度小于目标速度，说明角色在空中碰撞减速了
            // y方向下落时也是一样的，如果角色卡在了空中，那么按照gravityDownForce向下加速的话，当角色在某一时刻脱离卡住的时候，角色会一下子拍到地面上
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

            // 卡在斜率超过限制的斜坡上，沿着斜坡下滑
            if (isAboveSlopeLimit)
            {
                //characterVelocity
                float l = 1 / Mathf.Cos(Vector3.Angle(m_GroundNormal, Vector3.up));
                characterVelocity += (Vector3.down * l + m_GroundNormal).normalized * Time.deltaTime;
            }

            // 二段跳
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
        //    Debug.Log((characterController.velocity.y - characterVelocity.y) / Time.deltaTime + "y方向速度异常突变1");
        //    characterController.Move(new Vector3(0, -characterController.velocity.y, 0) * Time.deltaTime);
        //}
        // (已解决)卡在斜坡角时，character Controller的速度会在y轴方向有一个突变，这里做一个判断
        //if (wasGrounded && !isGrounded && !isHooking && !isJumped && characterController.velocity.y > 0)
        //{
        //    Debug.Log(characterController.velocity.y + characterVelocity.y + "y方向速度异常突变");
        //    characterController.Move(new Vector3(0, -characterController.velocity.y, 0) * Time.deltaTime);
        //}
    }

    public void initPlayer()
    {
        Debug.Log("初始化");
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

    // 获取角色移动方向的坡度方向
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
