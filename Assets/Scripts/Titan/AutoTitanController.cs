using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(TitanAnimationController))]
[RequireComponent(typeof(TitanStateManager))]
[DisallowMultipleComponent]
public class AutoTitanController : MonoBehaviour
{
    public bool isGrounded;
    public bool isRuning;
    bool wasGrounded;

    [Header("Check Ground Setting")]
    // �ӵ��ж�ʱ�����������ľ��룬�ڵ����Ϻ��ڿ���ʱ��һ�����ڿ���ʱҪ��һ�㡣
    public float groundCheckDistance = 0.25f;
    public float groundCheckDistanceInAir = 0.27f;
    public LayerMask groundCheckLayers = -1;

    [Header("Rotate Setting")]
    //public Transform cameraRotateHelper;
    public float rotationSpeed = 0.5f;
    public float boosterForce = 30f;
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
    

    [Header("find enemy")]
    // �ܿ����Ĳ�
    public LayerMask obstructionLayerMask;
    //Actor actor;
    // 
    float lastSeeTime;
    // �����������
    public float distanceLimit = 500;
    // ��󹥻�����
    public float distanceAttack = 10;
    public float shootAngleLimit = 5;

    // 
    public int lastFramTitanState;
    Actor actor;
    public Actor targetActor;
    TeamManager teamManager;

    public Transform rightUpperArm;
    bool isAimTarget;

    // ��¼��������Ϣ
    Actor lastFrameTarget;
    public float lastChangeTargetTime;
    float lastEludeTime;
    Vector3 eludeDir;
    float eludeDuration = 2;

    float verticalSpeed;
    TitanController titanController;

    public Actor traceTarget;
    int[] intRandomArray = new int[] { -1, 1 };
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        titanAnimationController = GetComponent<TitanAnimationController>();
        m_PlayerTitanWeaponManager = GetComponent<PlayerTitanWeaponManager>();
        m_InputHandler = GetComponent<InputHandler>();
        m_AudioSource = GetComponent<AudioSource>();
        titanController = GetComponent<TitanController>();
        titanStateManager = GetComponent<TitanStateManager>();
        actor = GetComponent<Actor>();
        teamManager = FindObjectOfType<TeamManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastFramTitanState != TitanState.AUTO_CONTROL && titanStateManager.curState == TitanState.AUTO_CONTROL)
        {
            m_PlayerTitanWeaponManager.initWeapon();
        }
        if (titanStateManager.curState == TitanState.AUTO_CONTROL)
        {
            if (titanController.canFly)
            {
                titanAnimationController.SetLayerEnable(1);
                FlyNPCMovement();
            }
            else
            {
                wasGrounded = isGrounded;
                CheckGround();
                titanAnimationController.SetLayerEnable(0);
                WalkNPCMovement();
            }
        }
        lastFramTitanState = titanStateManager.curState;
    }

    void FlyNPCMovement()
    {
        //titanAnimationController.DisableRightHandIK();
        // ����������20m�ڣ����ϣ���������
        if (Physics.Raycast(transform.position + Vector3.up*3, Vector3.down, out RaycastHit hit, 25, groundCheckLayers, QueryTriggerInteraction.Ignore))
        {
            Debug.Log(Time.time + "�����");
            
            //transform.Translate(Vector3.up * Time.deltaTime * 5f);
            characterController.Move(Vector3.up * Time.deltaTime * 5f);
        } else
        {
            transform.Translate(Vector3.up * Time.deltaTime * Mathf.Sin(Time.time));
            isAimTarget = false;
            // �ܵ����������
            if (actor.lastBeHurtTime >= 0
                && Time.time < actor.lastBeHurtTime + 0.1f)
            {
                // ֻ�г����˶����ȴ���ż�¼�ϴζ��ʱ��
                if (Time.time > lastEludeTime + eludeDuration)
                {
                    // ���㷽��(��/��)
                    eludeDir = Vector3.right * intRandomArray[Random.Range(0, 2)];
                    lastEludeTime = Time.time;
                }
            }

            // �ܵ�����5s�ڣ�����������Ϊ����Ŀ��
            if (actor.lastBeHurtTime >= 0
                && Time.time < actor.lastBeHurtTime + 5f
                && actor.lastDamageMsg.shooter
                && actor.lastDamageMsg.shooter.gameObject.activeInHierarchy)
            {
                // Ϊ�˱���Ƶ������Ŀ�꣬�������2s����һ��
                if (actor.lastDamageMsg.shooter != targetActor && Time.time > lastChangeTargetTime + 2f)
                {
                    targetActor = actor.lastDamageMsg.shooter;
                    lastChangeTargetTime = Time.time;
                }
            }
            else
            {
                Actor newTarget = findEnemy();
                if (newTarget != targetActor && Time.time > lastChangeTargetTime + 2f)
                {
                    targetActor = newTarget;
                    lastChangeTargetTime = Time.time;
                }
            }
            
            // ���
            if (lastEludeTime > 0 && Time.time < lastEludeTime + 0.3f)
            {
                characterController.Move(transform.TransformDirection(eludeDir) * Time.deltaTime * 20f);
                //transform.Translate(transform.TransformDirection(eludeDir) * Time.deltaTime * 20);
            }

            /*
          ���������Χ��ǰ���������е���
            �����Ŀ���ˮƽ�Ƕ�С��5�ȣ�
                ����
            ����
                ת�����
          �������������Χ���е���
            ������ƶ�
            */

            if (targetActor)
            {
                // ����ڹ�����Χ��
                if (Vector3.Distance(targetActor.transform.position, transform.position) < distanceAttack)
                {
                    //Debug.Log(Time.time + gameObject.name + "����������" + targetActor.name);
                    Vector3 vDir = targetActor.transform.position + Vector3.up - transform.position;
                    vDir.y = 0;
                    if (Vector3.Angle(transform.forward, vDir) < shootAngleLimit)
                    {
                        lastSeeTime = Time.time;
                        // ����
                        //titanAnimationController.Shoot();
                        isAimTarget = true;
                        // ����ik
                        titanAnimationController.SetRightHandIk(targetActor.transform);
                        // ���
                        m_PlayerTitanWeaponManager.curWeapon.titanWeaponController.shootAction?.Invoke();
                    }
                    else
                    {
                        titanAnimationController.DisableLeftHandIK();
                        //Debug.Log(gameObject.name + "�ǶȲ�����" + Vector3.Angle(transform.forward, vDir));
                        Vector3 vec = (targetActor.transform.position - transform.position);
                        vec.y = 0;
                        Quaternion rotate = Quaternion.LookRotation(vec);
                        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotate, rotationSpeed);
                        //TryWalk();
                    }
                }
                // ������ڹ�����Χ�ڣ�����Ŀ��
                else
                {
                    Vector3 vec = (targetActor.transform.position - transform.position);
                    vec.y = 0;
                    Quaternion rotate = Quaternion.LookRotation(vec);
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, rotate, rotationSpeed);
                    //transform.Translate(transform.forward * Time.deltaTime * maxSpeedOnGround);
                    characterController.Move(transform.forward * Time.deltaTime * maxSpeedOnGround);
                }
                lastFrameTarget = targetActor;
            } else
            {
                // ���û�пɹ�����Ŀ�꣬�ҵ������Ŀ��׷��
                traceTarget = findEnemyXRay();
                if (traceTarget)
                {
                    Vector3 vec = (traceTarget.transform.position - transform.position);
                    vec.y = 0;
                    Quaternion rotate = Quaternion.LookRotation(vec);
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, rotate, rotationSpeed);
                    //transform.Translate(transform.forward * Time.deltaTime * maxSpeedOnGround);
                    characterController.Move(transform.forward * Time.deltaTime * maxSpeedOnGround);
                }
            }
        }
    }

    void WalkNPCMovement()
    {
        TryIdle();
        //if (!isGrounded)
        //{
        //    characterController.Move(Vector3.down * Time.deltaTime);
        //}
        titanAnimationController.DisableRightHandIK();
        isAimTarget = false;
        //m_PlayerTitanWeaponManager.curWeapon.titanWeaponController.shootAction?.Invoke();
        if (isGrounded)
        {

            if (!wasGrounded)
            {
                if (characterVelocity.y < -30)
                {
                    // �����ٶȹ���ʱ
                    m_AudioSource.PlayOneShot(landSFX);
                }
            }
            /* ������ܹ�����ѡ��Ŀ��Ϊ������(��Ҫ��¼һ���ϴ�ת��Ŀ���ʱ��)
             * ����Ѱ����Ұ������ĵ�����ΪĿ��
             */
            // ���ܼ���
            Vector3 targetVelocity = Vector3.zero;
            if (lastEludeTime != 0 && Time.time < lastEludeTime + 1f)
            {
                targetVelocity = 10f * transform.TransformDirection(eludeDir);
                TryWalk();
            }
            characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, movementSharpnessOnGround * Time.deltaTime);
            //Debug.Log("���һ�ι�����" + actor.lastDamageMsg.shooter.name);
            if (actor.lastBeHurtTime >= 0
                && Time.time < actor.lastBeHurtTime + 5f
                && actor.lastDamageMsg.shooter
                && actor.lastDamageMsg.shooter.gameObject.activeInHierarchy)
            {
                // 
                if (actor.lastDamageMsg.shooter != targetActor && Time.time > lastChangeTargetTime + 2f)
                {
                    targetActor = actor.lastDamageMsg.shooter;
                    lastChangeTargetTime = Time.time;
                }
                // �ϴζ��ʱ��
                if (Time.time > lastEludeTime + eludeDuration)
                {
                    //characterController.Move(transform.right * Time.deltaTime * maxSpeedOnGround * 10f);
                    lastEludeTime = Time.time;
                    // ���㷽��(��/��)
                    eludeDir = Vector3.right * intRandomArray[Random.Range(0, 2)];
                }

            }
            else
            {
                targetActor = findEnemy();
            }

            /*
              ���������Χ���������е���
                ���������ǰ�����Һ�Ŀ���ˮƽ�Ƕ�С��5�ȣ�
                    ����
                ����
                    ת�����
              �������������Χ���е���
                ������ƶ�
                */

            if (targetActor)
            {
                // ����ڹ�����Χ��
                if (Vector3.Distance(targetActor.transform.position, transform.position) < distanceAttack)
                {
                    //Debug.Log(Time.time + gameObject.name + "����������" + targetActor.name);
                    Vector3 vDir = targetActor.transform.position + Vector3.up - transform.position;
                    vDir.y = 0;
                    if (Vector3.Angle(transform.forward, vDir) < shootAngleLimit)
                    {
                        lastSeeTime = Time.time;
                        // ����
                        //titanAnimationController.Shoot();
                        isAimTarget = true;
                        // ����ik
                        titanAnimationController.SetRightHandIk(targetActor.transform);
                        // ���
                        m_PlayerTitanWeaponManager.curWeapon.titanWeaponController.shootAction?.Invoke();
                    }
                    else
                    {
                        titanAnimationController.DisableLeftHandIK();
                        //Debug.Log(gameObject.name + "�ǶȲ�����" + Vector3.Angle(transform.forward, vDir));
                        Vector3 vec = (targetActor.transform.position - transform.position);
                        vec.y = 0;
                        Quaternion rotate = Quaternion.LookRotation(vec);
                        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotate, rotationSpeed);
                        TryWalk();
                    }
                }
                // ������ڹ�����Χ�ڣ�����Ŀ��
                else
                {
                    //titanAnimationController.DisableLeftHandIK();
                    //Debug.Log(gameObject.name + "�ǶȲ�����" + Vector3.Angle(transform.forward, vDir));
                    Vector3 vec = (targetActor.transform.position - transform.position);
                    vec.y = 0;
                    Quaternion rotate = Quaternion.LookRotation(vec);
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, rotate, rotationSpeed);
                    TryWalk();
                    characterVelocity += transform.forward * maxSpeedOnGround;

                }
                lastFrameTarget = targetActor;
            }
            characterController.Move(characterVelocity * Time.deltaTime);
        } else
        {
            if (characterController.velocity.sqrMagnitude < characterVelocity.sqrMagnitude)
            {
                characterVelocity = characterController.velocity;
            }
            if (characterController.velocity.y < -30)
            {
                // �����ٶȹ���ʱ
                titanAnimationController.Air();
            }
            //characterVelocity += worldspaceMoveInput * accelerationSpeedInAir * Time.deltaTime;
            float verticalVelocity = characterVelocity.y;
            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(characterVelocity, Vector3.up);
            characterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);
            characterVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
        }
    }

    public void TryIdle()
    {
        titanAnimationController.Idle();
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

    public void TryWalk()
    {
        titanAnimationController.Walk();
        //if (m_PlayerTitanWeaponManager.curWeapon.name == "laser")
        //{
        //    titanAnimationController.Weapon1();
        //}
        //else if (m_PlayerTitanWeaponManager.curWeapon.name == "rifle")
        //{
        //    titanAnimationController.Weapon2();
        //}
        //if (m_PlayerTitanWeaponManager.isShooting)
        //{
        //    titanAnimationController.Shoot();
        //}
    }

    Actor findEnemy()
    {
        if (!teamManager)
        {
            return null;
        }
        List<Actor> targets = null;
        if (actor.team == Team.TEAM1)
        {
            targets = teamManager.team2Actors;
        }
        else if (actor.team == Team.TEAM2)
        {
            targets = teamManager.team1Actors;
        }
        Actor target = null;
        if (targets.Count > 0)
        {
            float minD = distanceLimit;
            foreach (Actor ob in targets)
            {
                if (ob)
                {
                    if (Vector3.Distance(ob.transform.position, transform.position) < minD)
                    {
                        Vector3 targetPos = ob.transform.position;
                        if (ob.damagePoint)
                        {
                            targetPos = ob.damagePoint.position;
                        }
                        if (Physics.Raycast(transform.position, targetPos - transform.position, out RaycastHit hit, distanceLimit, actor.canHitLayerMask))
                        {
                            if (hit.transform.gameObject.GetComponent<Damagable>() &&
                                hit.transform.gameObject.GetComponent<Damagable>().parentActor == ob)
                            {
                                minD = Vector3.Distance(ob.transform.position, transform.position);
                                target = ob;
                            }
                        }

                    }
                }
            }
        }
        return target;
    }

    Actor findEnemyXRay()
    {
        if (!teamManager)
        {
            return null;
        }
        List<Actor> targets = null;
        if (actor.team == Team.TEAM1)
        {
            targets = teamManager.team2Actors;
        }
        else if (actor.team == Team.TEAM2)
        {
            targets = teamManager.team1Actors;
        }
        Actor target = null;
        if (targets.Count > 0)
        {
            float minD = distanceLimit;
            foreach (Actor ob in targets)
            {
                if (ob)
                {
                    if (Vector3.Distance(ob.transform.position, transform.position) < minD)
                    {
                        target = ob;
                        minD = Vector3.Distance(ob.transform.position, transform.position);
                        //Vector3 targetPos = ob.transform.position;
                        //if (ob.damagePoint)
                        //{
                        //    targetPos = ob.damagePoint.position;
                        //}
                        //if (Physics.Raycast(transform.position, targetPos - transform.position, out RaycastHit hit, distanceLimit, actor.canHitLayerMask))
                        //{
                        //    if (hit.transform.gameObject.GetComponent<Damagable>() &&
                        //        hit.transform.gameObject.GetComponent<Damagable>().parentActor == ob)
                        //    {
                        //        minD = Vector3.Distance(ob.transform.position, transform.position);
                        //        target = ob;
                        //    }
                        //}

                    }
                }
            }
        }
        return target;
    }

    Actor findEnemyInSight()
    {
        if (!teamManager)
        {
            return null;
        }
        List<Actor> targets = null;
        if (actor.team == Team.TEAM1)
        {
            targets = teamManager.team2Actors;
        } else if (actor.team == Team.TEAM2)
        {
            targets = teamManager.team1Actors;
        }
        Actor target = null;
        if (targets.Count > 0)
        {
            float minD = distanceLimit;
            foreach (Actor ob in targets)
            {
                if (ob.gameObject.activeInHierarchy)
                {
                    if (Vector3.Distance(ob.transform.position, transform.position) < minD 
                        && Vector3.Dot(ob.transform.position-transform.position, transform.forward) > 0)
                    {
                        if (Physics.Raycast(transform.position, ob.damagePoint.position - transform.position, out RaycastHit hit, distanceLimit, actor.canHitLayerMask))
                        {
                            if (hit.transform.gameObject.GetComponent<Damagable>() &&
                                hit.transform.gameObject.GetComponent<Damagable>().parentActor == ob)
                            {
                                minD = Vector3.Distance(ob.transform.position, transform.position);
                                target = ob;
                            }
                        }
                        
                    }
                }
            }
        }
        return target;
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
