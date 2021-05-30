using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(PlayerCharacterController))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(PlayerWeaponManager))]
public class Hook : MonoBehaviour
{
    InputHandler m_InputHandler;
    PlayerCharacterController m_PlayerCharacterController;
    PlayerWeaponManager m_PlayerWeaponManager;
    AudioSource m_AudioSource;
    [Header("Hooking Setting")]
    public float maxHookSpeed = 30f;
    public float lastHookTime;
    public bool hooking = false;
    public GameObject Bullet;
    // 预计时长
    public float expectedHookTime { get; set; }
    public Transform hookTargetTransform { get; set; }
    public GameObject hookTargetObject;
    public float thisHookLength { get; set; }

    public float hookLength = 100f;
    public LayerMask hookLayerMask = -1;
    LineRenderer m_LineRenderer;
    
    public Transform hookRootPos;

    public GameObject activeStar;
    public GameObject deactiveStar;
    public AudioClip recycingHookSFX;
    public AudioClip sendHookSFX;

    // Start is called before the first frame update
    void Start()
    {
        m_InputHandler = GetComponent<InputHandler>();
        m_PlayerCharacterController = GetComponent<PlayerCharacterController>();
        m_PlayerWeaponManager = GetComponent<PlayerWeaponManager>();
        m_LineRenderer = GetComponent<LineRenderer>();
        m_AudioSource = GetComponent<AudioSource>();
        hookTargetObject = Instantiate(Bullet, m_PlayerCharacterController.fpCamera.transform.position, m_PlayerCharacterController.fpCamera.transform.rotation);
        hookTargetObject.GetComponent<HookBullet>().m_Hook = this;
        hookTargetObject.GetComponent<HookBullet>().hookLayerMask = hookLayerMask;
        hookTargetObject.SetActive(false);
        hookTargetObject.transform.SetParent(null);
        hookTargetObject.transform.position = Vector3.zero;
        hookTargetObject.transform.eulerAngles = Vector3.zero;
        hookTargetObject.transform.localScale = Vector3.one;
    }

    void ShowDeactiveStar()
    {
        if (activeStar && deactiveStar)
        {
            activeStar.SetActive(false);
            deactiveStar.SetActive(true);
        }
    }

    void ShowActiveStar()
    {
        if (activeStar && deactiveStar)
        {
            activeStar.SetActive(true);
            deactiveStar.SetActive(false);
        }
    }

    void DisAbleStar()
    {
        if (activeStar && deactiveStar)
        {
            activeStar.SetActive(false);
            deactiveStar.SetActive(false);
        }
    }

    void HandleCharacterHook()
    {
        
        if (Physics.Raycast(m_PlayerCharacterController.fpCamera.transform.position, m_PlayerCharacterController.fpCamera.transform.forward, out RaycastHit hit1, hookLength, hookLayerMask, QueryTriggerInteraction.Ignore))
        {
            ShowActiveStar();
            if (!hooking && m_InputHandler.GetHookInputDown())

            {
                m_AudioSource.PlayOneShot(sendHookSFX);
                if (m_PlayerCharacterController.fpCamera)
                {
                    
                    hookTargetObject.SetActive(true);
                    hookTargetObject.transform.position = m_PlayerCharacterController.fpCamera.transform.position;
                    hookTargetObject.transform.rotation = m_PlayerCharacterController.fpCamera.transform.rotation;
                    hookTargetObject.GetComponent<HookBullet>().direction = (hit1.point - m_PlayerCharacterController.fpCamera.transform.position).normalized;

                    hookTargetObject.GetComponent<HookBullet>().isSend = true;
                    hooking = false;
                    /* 实例化一个抓钩，
                       将抓勾沿着摄像机方向发射出去
                       如果碰到物体:
                          将抓钩设置为物体的子级
                          hookTargetTransform = 抓钩

                    */
                }
            }
        } else
        {
            ShowDeactiveStar();
        }

        if (m_PlayerWeaponManager.isAiming || m_PlayerWeaponManager.isShooting)
        {
            DisAbleStar();
        }
        //if (!hooking && m_InputHandler.GetHookInputDown())
        //{
        //    if (m_PlayerCharacterController.fpCamera)
        //    {
        //        if (Physics.Raycast(m_PlayerCharacterController.fpCamera.transform.position, m_PlayerCharacterController.fpCamera.transform.forward, out RaycastHit hit, hookLength, hookLayerMask, QueryTriggerInteraction.Ignore))
        //        {
        //            hookTargetObject.SetActive(true);
        //            hookTargetObject.transform.position = m_PlayerCharacterController.fpCamera.transform.position;
        //            hookTargetObject.transform.rotation = m_PlayerCharacterController.fpCamera.transform.rotation;
        //            hookTargetObject.GetComponent<HookBullet>().direction = m_PlayerCharacterController.fpCamera.transform.forward;

        //            hookTargetObject.GetComponent<HookBullet>().isSend = true;
        //            hooking = false;
        //            /* 实例化一个抓钩，
        //               将抓勾沿着摄像机方向发射出去
        //               如果碰到物体:
        //                  将抓钩设置为物体的子级
        //                  hookTargetTransform = 抓钩
                          
        //            */
        //        }
        //    }
        //}
        

        if (hooking && m_InputHandler.GetHookInputDown())
        {
            hooking = false;
            hookTargetObject.GetComponent<HookBullet>().isSend = false;
            RecyclingHook();
        }

        // 设置绳索
        if (hooking || hookTargetObject.GetComponent<HookBullet>().isSend)
        {
            m_LineRenderer.enabled = true;
            m_LineRenderer.SetPosition(0, hookTargetObject.transform.position);
            m_LineRenderer.SetPosition(1, hookRootPos.position);
        } else
        {
            m_LineRenderer.enabled = false;
        }
    }

    public void RecyclingHook()
    {
        hookTargetObject.SetActive(false);
        hookTargetObject.transform.SetParent(null);
        hookTargetObject.transform.position = Vector3.zero;
        hookTargetObject.transform.eulerAngles = Vector3.zero;
        hookTargetObject.transform.localScale = Vector3.one;
        m_AudioSource.PlayOneShot(recycingHookSFX);
    }
    // Update is called once per frame
    void Update()
    {
        HandleCharacterHook();
    }

}
