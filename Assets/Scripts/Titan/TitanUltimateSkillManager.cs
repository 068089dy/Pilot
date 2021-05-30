using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TitanStateManager))]
public class TitanUltimateSkillManager : MonoBehaviour
{
    TitanStateManager titanStateManager;
    public LaserCore laserCore;
    public bool isRunning;
    // Start is called before the first frame update
    void Start()
    {
        titanStateManager = GetComponent<TitanStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (titanStateManager.curState == TitanState.PLAYER_CONTROL)
        {
            laserCore.enabled = true;
        } else
        {
            laserCore.enabled = false;
        }
    }
}
