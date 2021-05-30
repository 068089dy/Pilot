using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCore : MonoBehaviour
{
    public InputHandler inputHandler;
    public BulletLaserCore laserCoreBullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inputHandler.GetMissileBatchInputDown())
        {
            if (!laserCoreBullet.isLaunching)
                laserCoreBullet.Launch();
        }
    }
}
