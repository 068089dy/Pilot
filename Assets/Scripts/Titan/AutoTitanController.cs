using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(TitanAnimationController))]
[RequireComponent(typeof(TitanStateManager))]
public class AutoTitanController : MonoBehaviour
{
    public bool isGrounded;
    public bool isRuning;
    bool wasGrounded;

    [Header("Check Ground Setting")]
    // 接地判断时，向地面延申的距离，在地面上和在空中时不一样，在空中时要长一点。
    public float groundCheckDistance = 0.25f;
    public float groundCheckDistanceInAir = 0.27f;
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
        if (titanStateManager.curState == TitanState.AUTO_CONTROL)
        {
            wasGrounded = isGrounded;
            CheckGround();
            AutoCharacterMovement();
        }
    }

    void AutoCharacterMovement()
    {
        titanAnimationController.Idle();
        // 搜索最近敌人

        //
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
