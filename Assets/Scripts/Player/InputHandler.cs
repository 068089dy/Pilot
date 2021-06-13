using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class InputHandler : MonoBehaviour
{
    public float mouseSensitivity = 4f;
    // 用来记录上一帧是否按住开火键
    bool m_FireInputWasHeld;
    bool m_OperateInputWasHeld;

    UIManager uIManager;

    public void Start()
    {
        //Screen.fullScreen = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        uIManager = FindObjectOfType<UIManager>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if (uIManager)
                    uIManager.hideESCPanel();
            } else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                if (uIManager)
                    uIManager.showESCPanel();
            }
        }
        if (!uIManager.isActiveAndEnabled)
        {
            if (Cursor.lockState == CursorLockMode.None && Input.GetMouseButton(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void LateUpdate()
    {
        m_FireInputWasHeld = GetFireInputHeld();
        m_OperateInputWasHeld = GetOperateInputHeld();
    }

    public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }

    // 开火键是否在这一帧刚刚按下（手枪/霰弹枪使用）
    public bool GetFireInputDown()
    {
        return GetFireInputHeld() && !m_FireInputWasHeld;
    }

    // 弹群
    public bool GetMissileBatchInputDown()
    {
        return Input.GetKeyDown(KeyCode.V);
    }

    public bool GetUltimateSkillInputDown()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }

    // 换弹
    public bool GetReloadInputDown()
    {
        return Input.GetKeyDown(KeyCode.R);
    }

    // 开火键是否在这一帧释放（充能枪等需要蓄力的武器使用）
    public bool GetFireInputRelease()
    {
        return !GetFireInputHeld() && m_FireInputWasHeld;
    }

    // 当前是否开火（开火键是否按住，自动步枪/冲锋枪使用）
    public bool GetFireInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetButton(GameConstants.k_ButtonNameFire);
        }
        return false;
    }

    // 瞄准
    public bool GetAimInputDown()
    {
        return Input.GetButtonDown(GameConstants.k_ButtonNameAim);
    }

    // 是否加速
    public bool GetRunInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetKey(KeyCode.LeftShift);
        }
        return false;
    }
    // 是否加速
    public bool GetRunInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.LeftShift);
        }
        return false;
    }

    // 是否输入跳跃
    public bool GetJumpInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetButton(GameConstants.k_ButtonNameJump);
        }
        return false;
    }

    public bool GetJumpInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonDown(GameConstants.k_ButtonNameJump);
        }
        return false;
    }

    // 鼠标移动相关
    public float GetLookInputHorizontal()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis(GameConstants.k_MouseAxisNameHorizontal);
        }
        return 0f;
    }
    public float GetLookInputVertical()
    {
        if (CanProcessInput())
        {
            return -Input.GetAxis(GameConstants.k_MouseAxisNameVertical);
        }
        return 0f;
    }

    // 抓勾
    public bool GetHookInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonDown(GameConstants.k_ButtonNameHook);
        }
        return false;
    }

    // 角色移动
    public Vector3 GetMoveInput()
    {
        if (CanProcessInput())
        {
            Vector3 move = new Vector3(Input.GetAxisRaw(GameConstants.k_AxisNameHorizontal), 0f, Input.GetAxisRaw(GameConstants.k_AxisNameVertical));
            return move.normalized;
        }
        return Vector3.zero;
    }

    // 武器切换
    public int GetSwitchWeaponInput()
    {
        if (CanProcessInput())
        {
           if (Input.GetAxis(GameConstants.k_ButtonNameSwitchWeapon) > 0f)
                return -1;
            else if (Input.GetAxis(GameConstants.k_ButtonNameSwitchWeapon) < 0f)
                return 1;
        }

        return 0;
    }

    // 控制按下
    public bool GetOperateInputDown()
    {
        if (CanProcessInput())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                return true;
            }
        }
        return false;
    }

    // 控制ing
    public bool GetOperateInputHeld()
    {
        if (CanProcessInput())
        {
            if (Input.GetKey(KeyCode.E))
            {
                return true;
            }
        }
        return false;
    }

    // 控制释放
    public bool GetOperateInputRelease()
    {
        return !GetOperateInputHeld() && m_OperateInputWasHeld;
    }

    // 近战攻击
    public bool GetMeleeAttackInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.F);
        }
        return false;
    }
}
