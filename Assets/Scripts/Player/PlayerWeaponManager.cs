using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(PlayerAnimationManager))]
[RequireComponent(typeof(PlayerCharacterController))]
public class PlayerWeaponManager : MonoBehaviour
{
    
    [SerializeField]
    public List<weaponMsg> weaponList;
    [NonSerialized]
    public int curWeaponIndex;
    //[NonSerialized]
    public weaponMsg curWeaponMsg;
    [NonSerialized]
    public GameObject curWeapon;



    InputHandler m_InputHandler;
    PlayerAnimationManager m_PlayerAnimationManager;
    PlayerCharacterController m_PlayerCharacterController;

    [NonSerialized]
    public bool isAiming;
    [NonSerialized]
    public bool isShooting;
    [NonSerialized]
    public bool isLoading;
    [NonSerialized]
    public bool isHiting;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerAnimationManager = GetComponent<PlayerAnimationManager>();
        m_PlayerCharacterController = GetComponent<PlayerCharacterController>();
        m_InputHandler = GetComponent<InputHandler>();
        setWeapon(0);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_InputHandler.GetSwitchWeaponInput() != 0)
        {
            //Debug.Log("换枪");
            setWeapon(m_InputHandler.GetSwitchWeaponInput() + curWeaponIndex);
        }
        if (m_InputHandler.GetAimInputDown())
        {
            if (isAiming)
            {
                isAiming = false;
            } else
            {
                isAiming = true;
            }
        }
        HandleAimCamera();
        if (!isHiting)
        {
            HandleShoot();
        }
        HandleMeleeAttack();
    }

    void HandleMeleeAttack()
    {
        if (m_InputHandler.GetMeleeAttackInputDown() && !isHiting)
        {
            isLoading = false;
            isShooting = false;
            isAiming = false;
            m_PlayerAnimationManager.SetLayerEnable(AnimatorLayer.MeleeLayer);
            m_PlayerAnimationManager.Melee();
            disablecurWeapon();
            setWeaponIK(-1);
            isHiting = true;
            
        }
        AnimatorStateInfo info = m_PlayerAnimationManager.GetCurrentAnimatorStateInfo(AnimatorLayer.MeleeLayer);
        if (isHiting && info.IsName("FPHit"))
        {
            Debug.Log("当前动作hit");
            if (info.normalizedTime >= 1f)
            {
                isHiting = false;
                m_PlayerAnimationManager.StopMelee();
                
                setWeapon(curWeaponIndex);
            }
        }
    }

    void HandleShoot()
    {
        if (curWeaponIndex == 0)
        {
            if (m_InputHandler.GetFireInputHeld())
            {
                if (curWeaponMsg.body.GetComponent<WeaponRifle>().curMagazineNum > 0)
                {
                    isShooting = true;
                    isLoading = false;
                    curWeaponMsg.body.GetComponent<WeaponRifle>().Shoot();
                    //curWeapon.GetComponent<WeaponRifle>().StopReload();
                } else
                {
                    isShooting = false;
                }
            } else
            {
                isShooting = false;
            }

        } else if (curWeaponIndex == 1)
        {
            if (m_InputHandler.GetFireInputDown() && curWeaponMsg.body.GetComponent<WeaponSniper>().loadingProgress >= 1)
            {
                //isAiming = false;
                isShooting = true;
                curWeaponMsg.body.GetComponent<WeaponSniper>().Shoot();
            } else
            {
                isShooting = false;
            }
        }
    }

    void HandleAimCamera()
    {
        if (curWeaponIndex == 0)
        {
            if (isAiming)
            {
                m_PlayerCharacterController.fpCamera.fieldOfView -= Time.deltaTime * 150;
            }
            else
            {
                m_PlayerCharacterController.fpCamera.fieldOfView += Time.deltaTime * 100;
            }
            m_PlayerCharacterController.fpCamera.fieldOfView = Mathf.Clamp(m_PlayerCharacterController.fpCamera.fieldOfView, 40, 60);
        }
        else if (curWeaponIndex == 1)
        {
            if (isAiming)
            {
                m_PlayerCharacterController.fpCamera.fieldOfView -= Time.deltaTime * 150;
            }
            else
            {
                m_PlayerCharacterController.fpCamera.fieldOfView += Time.deltaTime * 100;
            }
            m_PlayerCharacterController.fpCamera.fieldOfView = Mathf.Clamp(m_PlayerCharacterController.fpCamera.fieldOfView, 18, 60);
        }

    }

    IEnumerator EnableSniperCameraDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        m_PlayerCharacterController.fpCamera.fieldOfView = 20;
        m_PlayerCharacterController.fpCamera.nearClipPlane = 2;
    }

    public void setWeapon(int index)
    {
        if (index < 0)
        {
            index = weaponList.Count - 1;
        }
        if (index >= weaponList.Count)
        {
            index = 0;
        }
        curWeaponIndex = index;

        foreach (weaponMsg w in weaponList)
        {
            if (w.index == index)
            {
                //w.body.SetActive(true);
                //setWeaponIK(index);
                //m_PlayerAnimationManager.SetLayerEnable(w.animatorLayer);
                //isAiming = false;
                //isShooting = false;
                //isLoading = false;
                ////curWeapon = w.body;
                //curWeaponMsg = w;
                //m_PlayerAnimationManager.Idle();
                enableNewWeapon(w);
            }
            else
            {
                disableWeapon(w);
            }
        }
        //disablecurWeapon();
    }

    void enableNewWeapon(weaponMsg cweaponMsg)
    {
        cweaponMsg.body.GetComponent<WeaponController>().EnableWeapon();
        setWeaponIK(cweaponMsg.index);
        m_PlayerAnimationManager.SetLayerEnable(cweaponMsg.animatorLayer);
        isAiming = false;
        isShooting = false;
        isLoading = false;
        curWeaponMsg = cweaponMsg;
        m_PlayerAnimationManager.Idle();
    }

    void disableWeapon(weaponMsg cweaponMsg)
    {
        cweaponMsg.body.GetComponent<WeaponController>().DisableWeapon();
    }

    void disablecurWeapon()
    {
        //curWeaponMsg.body.SetActive(false);
        curWeaponMsg.body.GetComponent<WeaponController>().DisableWeapon();
    }

    void setWeaponIK(int index)
    {

        foreach (weaponMsg w in weaponList)
        {
            if (w.index == index)
            {
                m_PlayerAnimationManager.SetLeftHandPoser(w.leftHandPose);
                m_PlayerAnimationManager.SetLeftHandIk(w.leftHandPose);
                m_PlayerAnimationManager.enableLeftHandIk = true;
                break;
            }
            else
            {
                m_PlayerAnimationManager.enableLeftHandIk = false;
            }
        }
    }
}

[Serializable]
public class weaponMsg
{
    public int index;
    public int animatorLayer;
    public string name;
    public Transform leftHandPose;
    public GameObject body;
}

class AnimatorLayer
{
    public static int RifleLayer = 1;
    public static int SniperLayer = 2;
    public static int MeleeLayer = 3;
}