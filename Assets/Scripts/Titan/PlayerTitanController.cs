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
    // �ӵ��ж�ʱ�����������ľ��룬�ڵ����Ϻ��ڿ���ʱ��һ�����ڿ���ʱҪ��һ�㡣
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

    [Tooltip("��ɫ�ٶȣ�ֻ��")]
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

    // �ӵط���
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
        // ���������ƶ�����ˮƽ��
        Vector3 worldspaceMoveInput = transform.TransformVector(m_InputHandler.GetMoveInput());
        if (isGrounded)
        {

            if (!wasGrounded)
            {
                if (characterVelocity.y < -10)
                {
                    // �����ٶȹ���ʱ
                    m_AudioSource.PlayOneShot(landSFX);
                }
            }
            // ����Ŀ���ٶ�
            Vector3 targetVelocity = worldspaceMoveInput * maxSpeedOnGround * speedModifier;
            targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, m_GroundNormal) * targetVelocity.magnitude;
            // ��������ٶ�
            targetVelocity = Vector3.ClampMagnitude(targetVelocity, 60f);
            // ��ɫ�ٶȲ�ֵ
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
            
            // �ܶ���
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
            /* ���иı��ٶ�
            ����
                1.��Ծʱ������΢���ٶ�
                2.���ˮƽ�ٶȹ��󣬺����û�����
            */
            // == ����ˮƽ�ٶȣ�ˮƽ�ٶȹ��󣬺����û����� ==
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
             * �õ�ǰ�Ľ�ɫ�ٶȷ���characterController����Ľ������ΪcharacterController��������㡣
             * ������ʱ�򣨱��翨��б�½�ʱ��character Controller����y�᷽����һ��ͻ�䣩
             */
            if (characterVelocity.y < -10)
            {
                // �����ٶȹ���ʱ
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
        //    Debug.Log((characterController.velocity.y - characterVelocity.y) / Time.deltaTime + "y�����ٶ��쳣ͻ��1");
        //    characterController.Move(new Vector3(0, -characterController.velocity.y, 0) * Time.deltaTime);
        //}
        // ����б�½�ʱ��character Controller���ٶȻ���y�᷽����һ��ͻ�䣬������һ���ж�
        //if (wasGrounded && !isGrounded && !isHooking && !isJumped && characterController.velocity.y > 0)
        //{
        //    Debug.Log(characterController.velocity.y + characterVelocity.y + "y�����ٶ��쳣ͻ��");
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

    // ��ȡ��ɫ�ƶ�������¶ȷ���
    public Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
    {
        Vector3 directionRight = Vector3.Cross(direction, transform.up);
        return Vector3.Cross(slopeNormal, directionRight).normalized;
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

        // ������Ǹ������������ӵ��ж�
        //if (Time.time > m_LastTimeJumped + k_JumpGroundingPreventionTime)
        //{
        Debug.DrawLine(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere());
            if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(), characterController.radius, Vector3.down, out RaycastHit hit, curGroundCheckDistance, groundCheckLayers, QueryTriggerInteraction.Ignore))
            {
                m_GroundNormal = hit.normal;

                if (Vector3.Dot(hit.normal, transform.up) > 0 && Vector3.Angle(m_GroundNormal, transform.up) <= characterController.slopeLimit)
                {
                    isGrounded = true;

                    // characterController���ܻ�������ײ�����ƶ�һЩ��������Ҫ���������ƶ�һЩ��
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
