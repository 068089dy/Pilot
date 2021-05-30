using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(TitanAnimationController))]
[RequireComponent(typeof(TitanStateManager))]
public class PlayerTitanController : MonoBehaviour
{
    public bool isGrounded;
    public bool isRuning;
    bool wasGrounded;

    [Header("Check Ground Setting")]
    // 接地判断时，向地面延申的距离，在地面上和在空中时不一样，在空中时要长一点。
    public float groundCheckDistance = 0.05f;
    public float groundCheckDistanceInAir = 0.07f;
    public LayerMask groundCheckLayers = -1;

    [Header("Rotate Setting")]
    public Transform cameraRotateHelper;
    public float rotationSpeed = 50f;
    public float gravityDownForce = 20f;
    public float RotationMultiplier
    {
        get
        {
            return 1f;
        }
    }

    [Tooltip("角色速度，只读")]
    public Vector3 characterVelocity;

    public float speedModifier
    {
        get
        {
            if (m_InputHandler.GetRunInputHeld())
            {
                return 1.6f;
            }
            return 1f;
        }
    }
    [Header("Move Setting")]
    public float maxSpeedOnGround = 10f;
    public float accelerationSpeedInAir = 30f;
    float movementSharpnessOnGround = 30f;

    float m_CameraVerticalAngle = 0f;

    float m_footstepDistanceCounter;
    [Header("Foot Audio Setting")]
    public float footstepSFXFrequency = 1.3f;
    public AudioClip footStepSFX;
    public AudioClip landSFX;
    public AudioClip jumpSFX;
    AudioSource m_AudioSource;
    float m_footstepTimeCounter;
    float footstepInterval
    {
        get
        {
            return 0.7f / speedModifier;
        }
    }

    // 接地法线
    Vector3 m_GroundNormal;
    CharacterController characterController;
    TitanAnimationController titanAnimationController;
    PlayerTitanWeaponManager m_PlayerTitanWeaponManager;
    InputHandler m_InputHandler;
    TitanStateManager titanStateManager;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        titanAnimationController = GetComponent<TitanAnimationController>();
        m_PlayerTitanWeaponManager = GetComponent<PlayerTitanWeaponManager>();
        m_InputHandler = GetComponent<InputHandler>();
        m_AudioSource = GetComponent<AudioSource>();
        titanStateManager = GetComponent<TitanStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (titanStateManager.curState == TitanState.PLAYER_CONTROL)
        {
            wasGrounded = isGrounded;
            CheckGround();
            HandleCharacterMovement();
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
        if (isGrounded)
        {

            if (!wasGrounded)
            {
                if (characterVelocity.y < -10)
                {
                    // 下落速度过快时
                    m_AudioSource.PlayOneShot(landSFX);
                }
            }
            // 计算目标速度
            Vector3 targetVelocity = worldspaceMoveInput * maxSpeedOnGround * speedModifier;
            targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, m_GroundNormal) * targetVelocity.magnitude;
            // 限制最大速度
            targetVelocity = Vector3.ClampMagnitude(targetVelocity, 60f);
            // 角色速度插值
            characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, movementSharpnessOnGround * Time.deltaTime);

            if (m_InputHandler.GetJumpInputDown())
            {
                characterVelocity = characterVelocity * 10f;
                m_AudioSource.PlayOneShot(jumpSFX);
            }

            if (worldspaceMoveInput.sqrMagnitude > 0)
            {
                TryWalk();
                //titanAnimationController.Walk();
            }
            
            // 跑动画
            if (m_InputHandler.GetRunInputHeld() 
                && worldspaceMoveInput.sqrMagnitude > 0)
            {
                isRuning = true;
                titanAnimationController.Run();
            } else
            {
                isRuning = false;
            }
            //if (worldspaceMoveInput.sqrMagnitude > 0)
            //{

            //    if (m_footstepTimeCounter >= footstepInterval)
            //    {
            //        m_footstepTimeCounter = 0;

            //    }
            //    if (m_footstepTimeCounter == 0)
            //    {
            //        m_AudioSource.PlayOneShot(footStepSFX);
            //    }
            //    m_footstepTimeCounter += Time.deltaTime;
            //}
            // keep track of distance traveled for footsteps sound

            if (m_footstepDistanceCounter >= 1f / footstepSFXFrequency)
            {
                m_footstepDistanceCounter = 0f;
                m_AudioSource.PlayOneShot(footStepSFX);

            }
            m_footstepDistanceCounter += worldspaceMoveInput.magnitude * speedModifier * Time.deltaTime;



        }
        else
        {
            isRuning = false;
            /* 空中改变速度
            需求：
                1.跳跃时，可以微调速度
                2.如果水平速度过大，忽略用户输入
            */
            // == 限制水平速度，水平速度过大，忽略用户输入 ==
            //Vector3 moveDir = m_InputHandler.GetMoveInput();
            //if (Vector3.Dot(characterController.velocity, transform.forward) > maxSpeedInAir)
            //{
            //    moveDir.z = 0;
            //}
            //if (Vector3.Dot(characterController.velocity, -transform.forward) > maxSpeedInAir)
            //{
            //    moveDir.z = 0;
            //}
            //if (Vector3.Dot(characterController.velocity, transform.right) > maxSpeedInAir)
            //{
            //    moveDir.x = 0;
            //}
            //if (Vector3.Dot(characterController.velocity, -transform.right) > maxSpeedInAir)
            //{
            //    moveDir.x = 0;
            //}
            //worldspaceMoveInput = transform.TransformVector(moveDir);
            // ===========================================

            /* 
             * 让当前的角色速度符合characterController计算的结果，因为characterController由物理计算。
             * 但是有时候（比如卡在斜坡角时，character Controller会在y轴方向有一个突变）
             */
            if (characterVelocity.y < -10)
            {
                // 下落速度过快时
                titanAnimationController.Air();
            }
            characterVelocity += worldspaceMoveInput * accelerationSpeedInAir * Time.deltaTime;
            float verticalVelocity = characterVelocity.y;
            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(characterVelocity, Vector3.up);
            //horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeedInAir * speedModifier);
            characterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);
            characterVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
            
        }
        characterController.Move(characterVelocity * Time.deltaTime);
        // 
        //if (!isGrounded && 
        //    (characterController.velocity.y - characterVelocity.y) / Time.deltaTime <= -100)
        //{
        //    Debug.Log((characterController.velocity.y - characterVelocity.y) / Time.deltaTime + "y方向速度异常突变1");
        //    characterController.Move(new Vector3(0, -characterController.velocity.y, 0) * Time.deltaTime);
        //}
        // 卡在斜坡角时，character Controller的速度会在y轴方向有一个突变，这里做一个判断
        //if (wasGrounded && !isGrounded && !isHooking && !isJumped && characterController.velocity.y > 0)
        //{
        //    Debug.Log(characterController.velocity.y + characterVelocity.y + "y方向速度异常突变");
        //    characterController.Move(new Vector3(0, -characterController.velocity.y, 0) * Time.deltaTime);
        //}
    }

    public void TryIdle()
    {
        titanAnimationController.Idle();
        if (m_PlayerTitanWeaponManager.curWeapon.name == "laser")
        {
            titanAnimationController.Weapon1();
        } else if (m_PlayerTitanWeaponManager.curWeapon.name == "rifle")
        {
            titanAnimationController.Weapon2();
        }

        if (m_PlayerTitanWeaponManager.isShooting)
        {
            titanAnimationController.Shoot();
        }
    }

    public void TryWalk()
    {
        titanAnimationController.Walk();
        if (m_PlayerTitanWeaponManager.curWeapon.name == "laser")
        {
            titanAnimationController.Weapon1();
        }
        else if (m_PlayerTitanWeaponManager.curWeapon.name == "rifle")
        {
            titanAnimationController.Weapon2();
        }
        if (m_PlayerTitanWeaponManager.isShooting)
        {
            titanAnimationController.Shoot();
        }
    }

    // 获取角色移动方向的坡度方向
    public Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
    {
        Vector3 directionRight = Vector3.Cross(direction, transform.up);
        return Vector3.Cross(slopeNormal, directionRight).normalized;
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

        // 如果不是刚起跳，才做接地判断
        //if (Time.time > m_LastTimeJumped + k_JumpGroundingPreventionTime)
        //{
        Debug.DrawLine(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere());
            if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(), characterController.radius, Vector3.down, out RaycastHit hit, curGroundCheckDistance, groundCheckLayers, QueryTriggerInteraction.Ignore))
            {
                m_GroundNormal = hit.normal;

                if (Vector3.Dot(hit.normal, transform.up) > 0 && Vector3.Angle(m_GroundNormal, transform.up) <= characterController.slopeLimit)
                {
                    isGrounded = true;

                    // characterController可能会由于碰撞往上移动一些，这里需要往下稍稍移动一些。
                    if (hit.distance > characterController.skinWidth)
                    {
                        characterController.Move(Vector3.down * hit.distance);
                    }
                }
            }
        //}
        //else
        //{
        //    isJumped = true;
        //}
        return isGrounded;
    }
}
