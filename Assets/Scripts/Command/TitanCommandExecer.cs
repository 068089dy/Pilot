using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanCommandExecer : MonoBehaviour
{
    TitanUltimateSkillManager titanUltimateSkillManager;
    public void Start()
    {
        titanUltimateSkillManager = GetComponent<TitanUltimateSkillManager>();
    }

    public void LaunchMissileBatch()
    {
        titanUltimateSkillManager.LaunchMissileBatch();
    }
}
