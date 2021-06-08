using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TitanWeaponController : MonoBehaviour
{
    public InputHandler inputHandler;
    public TitanStateManager titanStateManager;
    public bool active;
    public UnityAction shootAction;
    public Actor owner;
    public ShootType shootType = ShootType.AUTO;

    public void Update()
    {
        if (titanStateManager.curState == TitanState.PLAYER_CONTROL)
        {
            if (active)
                HandleShoot();
        }
    }

    void HandleShoot()
    {
        if (shootType == ShootType.AUTO)
        {
            if (inputHandler.GetFireInputHeld())
            {
                shootAction?.Invoke();
            }
        }
        else if (shootType == ShootType.HAND)
        {
            if (inputHandler.GetFireInputDown())
            {
                shootAction?.Invoke();
            }
        }
    }
}

public enum ShootType
{
    AUTO,
    HAND
}