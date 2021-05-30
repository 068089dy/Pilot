using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(TitanStateManager))]
public class TitanController : MonoBehaviour
{
    public bool isGrounded;
    bool wasGrounded;
    [Header("Check Ground Setting")]
    // 接地判断时，向地面延申的距离，在地面上和在空中时不一样，在空中时要长一点。
    public float groundCheckDistance = 0.25f;
    public float groundCheckDistanceInAir = 0.27f;
    public float gravityDownForce = 20f;

    public Camera titanCamera;
    public GameObject Player;
    // 下机时要用到的
    public Transform rotateHandle;
    public Camera lerpCamera;
    public Camera playerCamera;
    public Transform cockpitCover;
    public float lerpSpeed = 5;

    CharacterController characterController;
    TitanStateManager titanStateManager;
    TitanAnimationController titanAnimationController;
    AudioSource audioSource;
    PlayerTitanWeaponManager playerWeaponManager;
    public LayerMask groundCheckLayers = -1;

    public AudioClip landSFX;

    Vector3 characterVelocity;

    public MeshRenderer screenRenderer;
    public Material screenTopMaterial;
    Material screenBottomMaterial;
    Material screenLeftMaterial;
    Material screenRightMaterial;
    Material screenCenterMaterial;

    string materialVarExposion = "Vector1_2599837B";
    string materialVarBlur = "Vector1_4D0B16C4";

    float initTime;
    bool isExiting;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        titanStateManager = GetComponent<TitanStateManager>();
        titanAnimationController = GetComponent<TitanAnimationController>();
        audioSource = GetComponent<AudioSource>();
        playerWeaponManager = GetComponent<PlayerTitanWeaponManager>();
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
            //characterController.Move(transform.forward * Time.deltaTime* speed);
            //if (!characterController.isGrounded)
            //{
            //    characterController.Move(-transform.up * Time.deltaTime * speed);
            //}
        } else if (titanStateManager.curState == TitanState.PLAYER_CONTROL)
        {
            //Camera.SetupCurrent(titanCamera);
            //titanCamera.gameObject.SetActive(true);
            screenAdjust();
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Player)
                {
                    isExiting = true;
                    cockpitCover.localEulerAngles = Vector3.zero;
                    titanStateManager.curState = TitanState.EXITING;
                    lerpCamera.gameObject.SetActive(true);
                    titanCamera.gameObject.SetActive(false);
                    //
                    Player.transform.position = transform.position + transform.forward*3f;
                    Player.transform.forward = transform.forward;

                }
            }
        } else if (titanStateManager.curState == TitanState.AUTO_CONTROL)
        {
            
            titanCamera.gameObject.SetActive(false);
            wasGrounded = isGrounded;
            CheckGround();
        } else if (titanStateManager.curState == TitanState.EXITING)
        {
            exitTitan();
        }
    }

    void exitTitan()
    {
        if (isExiting)
        {
            float cockpitXAngle = cockpitCover.localEulerAngles.x;
            cockpitXAngle = Mathf.Lerp(cockpitXAngle, 90, Time.deltaTime * 5f);
            cockpitCover.localEulerAngles = new Vector3(cockpitXAngle, 0, 0);
            
            // 摄像机插值
            lerpCamera.transform.position = Vector3.Lerp(lerpCamera.transform.position, playerCamera.transform.position, Time.deltaTime * lerpSpeed);
            lerpCamera.transform.right = Vector3.Lerp(lerpCamera.transform.right, playerCamera.transform.right, Time.deltaTime * lerpSpeed);
            if (cockpitXAngle > 89)
            {
                rotateHandle.localEulerAngles = Vector3.zero;
                titanStateManager.curState = TitanState.AUTO_CONTROL;
                Player.SetActive(true);
                Player.GetComponent<PlayerCharacterController>().initPlayer();
                cockpitCover.localEulerAngles = new Vector3(-6, 0, 0);
                isExiting = false;
            }
        }
    }

    public void setTitanState(int titanState)
    {
        titanStateManager.curState = titanState;
    }

    public void setPlayerControlMode()
    {
        titanStateManager.curState = TitanState.PLAYER_CONTROL;
        Camera.SetupCurrent(titanCamera);
        titanCamera.gameObject.SetActive(true);
        playerWeaponManager.initWeapon();
        initScreen();
        //titanCamera.enabled = true;
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
            if (!wasGrounded)
            {
                if (characterVelocity.y < -10)
                {
                    // 下落速度过快时
                    audioSource.PlayOneShot(landSFX);
                }
            }
        } else
        {
            characterVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
            if (characterVelocity.y < -10)
            {
                // 下落速度过快时
                titanAnimationController.Air();
            }
            if (Physics.Raycast(transform.position, Vector3.down, 10, groundCheckLayers, QueryTriggerInteraction.Ignore))
            {
                titanAnimationController.Squat();
            }
            characterController.Move(characterVelocity * Time.deltaTime);
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

        // 如果不是刚起跳，才做接地判断
        //if (Time.time > m_LastTimeJumped + k_JumpGroundingPreventionTime)
        //{
        Debug.DrawLine(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere());
        if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(), characterController.radius, Vector3.down, out RaycastHit hit, curGroundCheckDistance, groundCheckLayers, QueryTriggerInteraction.Ignore))
        {
            //m_GroundNormal = hit.normal;

            if (Vector3.Dot(hit.normal, transform.up) > 0 && Vector3.Angle(hit.normal, transform.up) <= characterController.slopeLimit)
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
