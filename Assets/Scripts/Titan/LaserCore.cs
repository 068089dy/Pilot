using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TitanUltimateSkillManager))]
public class LaserCore : MonoBehaviour
{
    TitanUltimateSkillManager titanUltimateSkillManager;
    public InputHandler inputHandler;
    public BulletLaserCore laserCoreBullet;
    // Start is called before the first frame update
    void Start()
    {
        titanUltimateSkillManager = GetComponent<TitanUltimateSkillManager>();
        titanUltimateSkillManager.LaunchAction = Launch;
    }

    public void Launch()
    {
        if (!laserCoreBullet.isLaunching)
        {
            laserCoreBullet.Launch(titanUltimateSkillManager);
        }
    }
}
