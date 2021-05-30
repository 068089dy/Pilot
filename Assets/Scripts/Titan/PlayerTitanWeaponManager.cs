using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(TitanAnimationController))]
[RequireComponent(typeof(PlayerTitanController))]
[RequireComponent(typeof(TitanStateManager))]
public class PlayerTitanWeaponManager : MonoBehaviour
{
    InputHandler m_InputHandler;
    TitanStateManager titanStateManager;
    PlayerTitanController playerTitanController;
    //public TitanUltimateSkillManager ultimateSkillManager;
    public BulletLaserCore bulletLaserCore;
    public List<TitanWeaponMsg> weaponList;
    public TitanWeaponMsg curWeapon = null;

    public bool isShooting;
    public UnityAction OnShoot;

    bool banWeapon;
    float lastBanWeaponTime;
    // Start is called before the first frame update
    void Start()
    {
        m_InputHandler = GetComponent<InputHandler>();
        titanStateManager = GetComponent<TitanStateManager>();
        playerTitanController = GetComponent<PlayerTitanController>();
        //curWeapon = weaponList[0];
        //curWeapon.titanWeaponController.active = true;
    }

    // Update is called once per frame
    void Update()
    {
        // ���ܻ��ռ�����״̬�½�������
        checkBanWeapon();
        if (curWeapon != null && curWeapon.titanWeaponController)
        {
            if (banWeapon)
            {
                curWeapon.titanWeaponController.active = false;
            }
            else
            {
                curWeapon.titanWeaponController.active = true;
            }
            // �����ָ���Ҫһ��ʱ�䣬����ʱ��������������
            if (Time.time > lastBanWeaponTime + 0.5f)
            {
                curWeapon.titanWeaponController.active = true;
            }
            else
            {
                curWeapon.titanWeaponController.active = false;
            }
        }
        if (titanStateManager.curState == TitanState.PLAYER_CONTROL)
        {
            if (m_InputHandler.GetSwitchWeaponInput() != 0)
            {
                lastBanWeaponTime = Time.time;
                setWeapon(m_InputHandler.GetSwitchWeaponInput() + curWeapon.index);
            }
        }
        
        
    }

    // ����ܷ�ʹ������
    void checkBanWeapon()
    {
        banWeapon = false;
        if (playerTitanController.isRuning)
        {
            banWeapon = true;
        }
        if (bulletLaserCore)
        {
            if (bulletLaserCore.isLaunching)
            {
                banWeapon = true;
            }
        }
        if (banWeapon)
        {
            lastBanWeaponTime = Time.time;
        }
    }

    public void initWeapon()
    {
        curWeapon = weaponList[0];
        curWeapon.titanWeaponController.active = true;
    }

    void setWeapon(int index)
    {
        if (index < 0)
        {
            index = weaponList.Count - 1;
        }
        if (index >= weaponList.Count)
        {
            index = 0;
        }
        for (int i = 0; i < weaponList.Count; i++)
        {
            weaponList[i].titanWeaponController.active = false;
        }
        //if (curWeapon == null)
        //    curWeapon.titanWeaponController.active = false;
        curWeapon = weaponList[index];
        curWeapon.titanWeaponController.active = true;

        //curWeaponIndex = index;
        //weaponList[curWeaponIndex].body.SetActive(true);
    }
}


[Serializable]
public class TitanWeaponMsg
{
    public int index;
    public string name;
    public TitanWeaponController titanWeaponController;
}
