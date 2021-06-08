using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(TitanStateManager))]
[DisallowMultipleComponent]
public class TitanController : MonoBehaviour
{
    public bool isGrounded;
    bool wasGrounded;
    [Header("Check Ground Setting")]
    // �ӵ��ж�ʱ�����������ľ��룬�ڵ����Ϻ��ڿ���ʱ��һ�����ڿ���ʱҪ��һ�㡣
    public float groundCheckDistance = 0.25f;
    public float groundCheckDistanceInAir = 0.27f;
    public float gravityDownForce = 20f;
    public LayerMask groundCheckLayers = -1;

    [Header("Camera And Player")]
    public Camera titanCamera;
    //public GameObject Player;

    // �»�ʱҪ�õ���
    [Header("Exit Titan Setting")]
    public Transform rotateHandle;
    public Camera lerpCamera;
    //public Camera playerCamera;
    public Transform cockpitCover;
    public Renderer titanBodyRenderer;
    public float lerpSpeed = 5;

    [Header("Landing Tag")]
    // �������
    public GameObject landingLight;
    // robot����
    public ParticleSystem titanLightTagFX;

    CharacterController characterController;
    TitanStateManager titanStateManager;
    TitanAnimationController titanAnimationController;
    AudioSource audioSource;
    PlayerTitanWeaponManager playerWeaponManager;
    

    public AudioClip landSFX;

    Vector3 characterVelocity;

    [Header("Screen")]
    public MeshRenderer screenRenderer;
    Material screenTopMaterial;
    Material screenBottomMaterial;
    Material screenLeftMaterial;
    Material screenRightMaterial;
    Material screenCenterMaterial;

    string materialVarExposion = "Vector1_2599837B";
    string materialVarBlur = "Vector1_4D0B16C4";

    float initTime;
    bool isExiting;

    Actor actor;
    //[System.NonSerialized]
    public Actor lastPilot;
    public bool canFly;

    int lastFrameState;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        titanStateManager = GetComponent<TitanStateManager>();
        titanAnimationController = GetComponent<TitanAnimationController>();
        audioSource = GetComponent<AudioSource>();
        playerWeaponManager = GetComponent<PlayerTitanWeaponManager>();
        actor = GetComponent<Actor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (titanStateManager.curState == TitanState.LANDING)
        {
            titanCamera.gameObject.SetActive(false);
            wasGrounded = isGrounded;
            CheckGround();
            HandleLand();
            characterController.Move(characterVelocity * Time.deltaTime);
            //characterController.Move(transform.forward * Time.deltaTime* speed);
            //if (!characterController.isGrounded)
            //{
            //    characterController.Move(-transform.up * Time.deltaTime * speed);
            //}
        } else if (titanStateManager.curState == TitanState.PLAYER_CONTROL)
        {
            //Camera.SetupCurrent(titanCamera);
            //titanCamera.gameObject.SetActive(true);
            if (lastFrameState != titanStateManager.curState)
            {
                if (lastPilot.characterType == CharacterType.PLAYER)
                    lastPilot.GetComponent<PlayerCharacterController>().hidePlayer();
            }
            screenAdjust();
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (lastPilot)
                {
                    titanBodyRenderer.enabled = true;
                    isExiting = true;
                    cockpitCover.localEulerAngles = Vector3.zero;
                    titanStateManager.curState = TitanState.EXITING;
                    lerpCamera.gameObject.SetActive(true);
                    titanCamera.gameObject.SetActive(false);
                    //
                    lastPilot.transform.position = transform.position + transform.forward*3f + Vector3.up * 5f;
                    lastPilot.transform.forward = transform.forward;
                    
                }
            }
        } else if (titanStateManager.curState == TitanState.AUTO_CONTROL)
        {
            titanCamera.gameObject.SetActive(false);
            //wasGrounded = isGrounded;
            //CheckGround();
        } else if (titanStateManager.curState == TitanState.EXITING)
        {
            exitTitan();
        }
        lastFrameState = titanStateManager.curState;
    }



    void exitTitan()
    {
        if (isExiting)
        {
            float cockpitXAngle = cockpitCover.localEulerAngles.x;
            cockpitXAngle = Mathf.Lerp(cockpitXAngle, 90, Time.deltaTime * 5f);
            cockpitCover.localEulerAngles = new Vector3(cockpitXAngle, 0, 0);
            
            // �������ֵ
            lerpCamera.transform.position = Vector3.Lerp(lerpCamera.transform.position, lastPilot.actorMainCamera.transform.position, Time.deltaTime * lerpSpeed);
            lerpCamera.transform.right = Vector3.Lerp(lerpCamera.transform.right, lastPilot.actorMainCamera.transform.right, Time.deltaTime * lerpSpeed);
            if (cockpitXAngle > 89)
            {
                rotateHandle.localEulerAngles = Vector3.zero;
                titanStateManager.curState = TitanState.AUTO_CONTROL;
                //lastPilot.gameObject.SetActive(true);
                titanCamera.gameObject.SetActive(false);
                lerpCamera.gameObject.SetActive(false);
                
                if (lastPilot.characterType == CharacterType.PLAYER)
                {
                    lastPilot.GetComponent<PlayerCharacterController>().initPlayer();
                    lastPilot.actorMainCamera.gameObject.SetActive(true);
                }
                cockpitCover.localEulerAngles = new Vector3(-6, 0, 0);
                isExiting = false;
            }
        }
    }

    public void setTitanState(int titanState)
    {
        titanStateManager.curState = titanState;
    }

    public void setPlayerControlMode(Actor actor)
    {
        titanStateManager.curState = TitanState.PLAYER_CONTROL;
        titanAnimationController.DisableRightHandIK();
        Camera.SetupCurrent(titanCamera);
        // ���ͷ
        titanCamera.gameObject.SetActive(true);
        // ��ʼ������
        playerWeaponManager.initWeapon();
        initScreen();
        // ��������
        titanBodyRenderer.enabled = false;
        //
        lastPilot = actor;
    }

    void initScreen()
    {
        foreach (Material material in screenRenderer.materials)
        {
            if (material.name.StartsWith("ScreenTop"))
            {
                screenTopMaterial = material;
                material.SetFloat(materialVarBlur, 1f);
                material.SetFloat(materialVarExposion, 0f);
            }
            else if (material.name.StartsWith("ScreenBottom"))
            {
                screenBottomMaterial = material;
                material.SetFloat(materialVarBlur, 1f);
                material.SetFloat(materialVarExposion, 0f);
            }
            else if (material.name.StartsWith("ScreenLeft"))
            {
                screenLeftMaterial = material;
                material.SetFloat(materialVarBlur, 1f);
                material.SetFloat(materialVarExposion, 0f);
            }
            else if (material.name.StartsWith("ScreenRight"))
            {
                screenRightMaterial = material;
                material.SetFloat(materialVarBlur, 1f);
                material.SetFloat(materialVarExposion, 0f);
            }
            else if (material.name.StartsWith("ScreenCenter"))
            {
                screenCenterMaterial = material;
                material.SetFloat(materialVarBlur, 1f);
                material.SetFloat(materialVarExposion, 0f);
            }
        }
        initTime = Time.time;
    }


    void screenAdjust()
    {
        if (Time.time < initTime + 6f)
        {
            if (Time.time > initTime + 0.1f)
            {
                if (screenTopMaterial)
                {
                    screenTopMaterial.SetFloat(materialVarBlur, Mathf.Lerp(screenTopMaterial.GetFloat(materialVarBlur), 0f, Time.deltaTime * Random.Range(0.1f, 3f)));
                    screenTopMaterial.SetFloat(materialVarExposion, Mathf.Lerp(screenTopMaterial.GetFloat(materialVarExposion), 1f, Time.deltaTime * Random.Range(0.1f, 3f)));
                }
            }

            if (Time.time > initTime + 0.2f)
            {
                if (screenBottomMaterial)
                {
                    screenBottomMaterial.SetFloat(materialVarBlur, Mathf.Lerp(screenBottomMaterial.GetFloat(materialVarBlur), 0f, Time.deltaTime * Random.Range(0.1f, 3f)));
                    screenBottomMaterial.SetFloat(materialVarExposion, Mathf.Lerp(screenBottomMaterial.GetFloat(materialVarExposion), 1f, Time.deltaTime * Random.Range(0.1f, 3f)));
                }
            }

            if (Time.time > initTime + 0.3f)
            {
                if (screenLeftMaterial)
                {
                    screenLeftMaterial.SetFloat(materialVarBlur, Mathf.Lerp(screenLeftMaterial.GetFloat(materialVarBlur), 0f, Time.deltaTime * Random.Range(0.1f, 3f)));
                    screenLeftMaterial.SetFloat(materialVarExposion, Mathf.Lerp(screenLeftMaterial.GetFloat(materialVarExposion), 1f, Time.deltaTime * Random.Range(0.1f, 3f)));
                }
            }

            if (Time.time > initTime + 0.4f)
            {
                if (screenRightMaterial)
                {
                    screenRightMaterial.SetFloat(materialVarBlur, Mathf.Lerp(screenRightMaterial.GetFloat(materialVarBlur), 0f, Time.deltaTime * Random.Range(0.1f, 3f)));
                    screenRightMaterial.SetFloat(materialVarExposion, Mathf.Lerp(screenRightMaterial.GetFloat(materialVarExposion), 1f, Time.deltaTime * Random.Range(0.1f, 3f)));
                }
            }

            if (Time.time > initTime + 0.5f)
            {
                if (screenCenterMaterial)
                {
                    screenCenterMaterial.SetFloat(materialVarBlur, Mathf.Lerp(screenCenterMaterial.GetFloat(materialVarBlur), 0f, Time.deltaTime * Random.Range(0.1f, 3f)));
                    screenCenterMaterial.SetFloat(materialVarExposion, Mathf.Lerp(screenCenterMaterial.GetFloat(materialVarExposion), 1f, Time.deltaTime * Random.Range(0.1f, 3f)));
                }
            }
        }
    }

    void HandleLand()
    {
        if (isGrounded)
        {
            // �رս������
            landingLight.SetActive(false);
            // �رո�����˸
            var ems = titanLightTagFX.emission;
            ems.enabled = false;
            if (!wasGrounded)
            {
                if (characterVelocity.y < -10)
                {
                    // �����ٶȹ���ʱ
                    audioSource.PlayOneShot(landSFX);
                }
                // ����ǵо���ֱ��תΪ�Զ�ģʽ
                if (actor.team == Team.TEAM2)
                {
                    titanStateManager.curState = TitanState.AUTO_CONTROL;
                    titanCamera.gameObject.SetActive(false);
                    lerpCamera.gameObject.SetActive(false);
                    
                } else if (actor.team == Team.TEAM1)
                {
                    // ������Լ��ˣ��򿪼�ʻ��
                    //float cockpitXAngle = cockpitCover.localEulerAngles.x;
                    //cockpitXAngle = Mathf.Lerp(cockpitXAngle, 90, Time.deltaTime * 5f);
                    cockpitCover.localEulerAngles = new Vector3(90, 0, 0);
                }
            }
        } else
        {
            characterVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
            if (characterVelocity.y < -10)
            {
                // �����ٶȹ���ʱ
                titanAnimationController.Air();
            }
            // �������10mʱ���¶�����
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1000,  groundCheckLayers, QueryTriggerInteraction.Ignore))
            {
                if (landingLight)
                {
                    // ���ù���λ��
                    landingLight.SetActive(true);
                    landingLight.transform.position = hit.point;
                }
                if (Vector3.Distance(hit.point, transform.position) < 50)
                {
                    titanAnimationController.Squat();
                }
            }
            characterController.Move(characterVelocity * Time.deltaTime);
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

        // ������Ǹ������������ӵ��ж�
        //if (Time.time > m_LastTimeJumped + k_JumpGroundingPreventionTime)
        //{
        Debug.DrawLine(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere());
        if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(), characterController.radius, Vector3.down, out RaycastHit hit, curGroundCheckDistance, groundCheckLayers, QueryTriggerInteraction.Ignore))
        {
            //m_GroundNormal = hit.normal;

            if (Vector3.Dot(hit.normal, transform.up) > 0 && Vector3.Angle(hit.normal, transform.up) <= characterController.slopeLimit)
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
