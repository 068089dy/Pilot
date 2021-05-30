using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TitanWeaponController))]
public class TitanWeaponLaser : MonoBehaviour
{
    public InputHandler m_InputHandler;
    AudioSource m_AudioSourceLoop;
    TitanWeaponController titanWeaponController;
    public PlayerTitanWeaponManager playerTitanWeaponManager;
    public TitanBulletLaser titanBulletLaser;

    public AudioClip loopChargeWeaponSFX;
    // Start is called before the first frame update
    void Start()
    {
        titanWeaponController = GetComponent<TitanWeaponController>();
        m_AudioSourceLoop = gameObject.AddComponent<AudioSource>();
        m_AudioSourceLoop.clip = loopChargeWeaponSFX;
        m_AudioSourceLoop.playOnAwake = false;
        m_AudioSourceLoop.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (titanWeaponController.active)
        {
            HandleShoot();
        } else
        {
            m_AudioSourceLoop.Stop();
            if (titanBulletLaser != null)
            {
                titanBulletLaser.launching = false;
            }
        }
    }


    void HandleShoot()
    {
        if (m_InputHandler.GetFireInputHeld())
        {
            playerTitanWeaponManager.isShooting = true;
            if (titanBulletLaser != null)
            {
                titanBulletLaser.launching = true;
            }
            if (!m_AudioSourceLoop.isPlaying)
            {
                m_AudioSourceLoop.Play();
            }
        }
        else
        {
            playerTitanWeaponManager.isShooting = false;
            m_AudioSourceLoop.Stop();
            if (titanBulletLaser != null)
            {
                titanBulletLaser.launching = false;
            }
        }
    }
}
