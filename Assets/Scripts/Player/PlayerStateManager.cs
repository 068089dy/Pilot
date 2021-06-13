using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerState curState;
    public Actor lastTitanActor;
    DamageCounter damageCounter;
    PlayerState lastFrameState;
    PlayerWeaponManager playerWeaponManager;

    public Renderer lArmRenderer;
    public Renderer RArmRenderer;
    public Renderer bodyRenderer;
    public Renderer headRenderer;


    // Start is called before the first frame update
    void Start()
    {
        curState = PlayerState.PLAYER_CONTROL;
        damageCounter = GetComponent<DamageCounter>();
        playerWeaponManager = GetComponent<PlayerWeaponManager>();
        damageCounter.DamageAction += setGuidanceMsg;
    }

    void setGuidanceMsg(DamageMsg damageMsg)
    {
        if (lastTitanActor)
        {
            lastTitanActor.GetComponent<MissileLockSystem>().AddGuidanceMsg(damageMsg.target);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lastFrameState != curState && PlayerState.PLAYER_CONTROL == curState)
        {
            // 进入操作状态，显示胳膊
            lArmRenderer.enabled = true;
            RArmRenderer.enabled = true;
            bodyRenderer.enabled = false;
            headRenderer.enabled = false;
        }
        if (lastFrameState != curState && curState == PlayerState.DRIVING)
        {
            // 进入驾驶状态，隐藏身体
            lArmRenderer.enabled = false;
            RArmRenderer.enabled = false;
            bodyRenderer.enabled = false;
            headRenderer.enabled = false;
            playerWeaponManager.DisabelAllWeapon();
        }
        if (curState == PlayerState.PLAYER_CONTROL)
        {

        }
        lastFrameState = curState;
    }


}

public enum PlayerState
{
    IDLE,
    PLAYER_CONTROL,
    DRIVING
}
