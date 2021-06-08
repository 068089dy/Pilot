using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TitanStateManager))]
[DisallowMultipleComponent]
public class TitanUltimateSkillManager : MonoBehaviour
{
    TitanStateManager titanStateManager;
    InputHandler inputHandler;
    DamageCounter damageCounter;
    public LaserCore laserCore;
    public MissileBatch missileBatch;
    public bool isRunning;
    public UnityAction LaunchAction;

    public float curEnerge = 0;
    public float maxEnerge = 100;
    public float chargeSpeed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        titanStateManager = GetComponent<TitanStateManager>();
        inputHandler = GetComponent<InputHandler>();
        damageCounter = GetComponent<DamageCounter>();
        damageCounter.DamageAction += AddEnerge;
    }

    void AddEnerge(DamageMsg damageMsg)
    {
        curEnerge += damageMsg.damage * chargeSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (titanStateManager.curState == TitanState.PLAYER_CONTROL && !isRunning)
        {
            //laserCore.enabled = true;
            if (inputHandler.GetUltimateSkillInputDown() && curEnerge >= maxEnerge)
            {
                //LaunchAction?.Invoke();
                laserCore.Launch();
                curEnerge = 0;
            }
            if (inputHandler.GetMissileBatchInputDown() && curEnerge >= maxEnerge)
            {
                //LaunchAction?.Invoke();
                missileBatch.Launch();
                curEnerge = 0;
            }
        }
    }
}
