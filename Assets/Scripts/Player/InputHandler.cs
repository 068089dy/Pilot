using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class InputHandler : MonoBehaviour
{
    public float mouseSensitivity = 4f;
    // ������¼��һ֡�Ƿ�ס�����
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

    // ������Ƿ�����һ֡�ոհ��£���ǹ/����ǹʹ�ã�
    public bool GetFireInputDown()
    {
        return GetFireInputHeld() && !m_FireInputWasHeld;
    }

    // ��Ⱥ
    public bool GetMissileBatchInputDown()
    {
        return Input.GetKeyDown(KeyCode.V);
    }

    public bool GetUltimateSkillInputDown()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }

    // ����
    public bool GetReloadInputDown()
    {
        return Input.GetKeyDown(KeyCode.R);
    }

    // ������Ƿ�����һ֡�ͷţ�����ǹ����Ҫ����������ʹ�ã�
    public bool GetFireInputRelease()
    {
        return !GetFireInputHeld() && m_FireInputWasHeld;
    }

    // ��ǰ�Ƿ񿪻𣨿�����Ƿ�ס���Զ���ǹ/���ǹʹ�ã�
    public bool GetFireInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetButton(GameConstants.k_ButtonNameFire);
        }
        return false;
    }

    // ��׼
    public bool GetAimInputDown()
    {
        return Input.GetButtonDown(GameConstants.k_ButtonNameAim);
    }

    // �Ƿ����
    public bool GetRunInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetKey(KeyCode.LeftShift);
        }
        return false;
    }
    // �Ƿ����
    public bool GetRunInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.LeftShift);
        }
        return false;
    }

    // �Ƿ�������Ծ
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

    // ����ƶ����
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

    // ץ��
    public bool GetHookInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonDown(GameConstants.k_ButtonNameHook);
        }
        return false;
    }

    // ��ɫ�ƶ�
    public Vector3 GetMoveInput()
    {
        if (CanProcessInput())
        {
            Vector3 move = new Vector3(Input.GetAxisRaw(GameConstants.k_AxisNameHorizontal), 0f, Input.GetAxisRaw(GameConstants.k_AxisNameVertical));
            return move.normalized;
        }
        return Vector3.zero;
    }

    // �����л�
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

    // ���ư���
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

    // ����ing
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

    // �����ͷ�
    public bool GetOperateInputRelease()
    {
        return !GetOperateInputHeld() && m_OperateInputWasHeld;
    }

    // ��ս����
    public bool GetMeleeAttackInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.F);
        }
        return false;
    }
}
