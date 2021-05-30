using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TitanWeaponController))]
public class TitanWeaponRifle : MonoBehaviour
{
    //public InputHandler inputHandler;
    public Transform weaponMuzzle;
    public Camera aimCamera;
    public LayerMask shotLayermask = -1;
    public float maxShotDistance = 500;

    public GameObject owner;
    public ProjectileBase projectilePrefab;
    public AudioSource audioSource;
    public AudioClip shotSFX;

    public float shootInterval = 0.4f;

    Vector3 curAimPoint;
    float lastShootTime;

    TitanWeaponController titanWeaponController;
    // Start is called before the first frame update
    void Start()
    {
        titanWeaponController = GetComponent<TitanWeaponController>();
        titanWeaponController.shootAction = Shoot;
    }

    // Update is called once per frame
    void Update()
    {
        if (titanWeaponController.active)
        {
            curAimPoint = aimCamera.transform.position + aimCamera.transform.forward * maxShotDistance;
            if (Physics.Raycast(aimCamera.transform.position, aimCamera.transform.forward, out RaycastHit hit, maxShotDistance, shotLayermask, QueryTriggerInteraction.Ignore))
            {
                curAimPoint = hit.point;
            }
            //HandleShoot();
        }
    }

    void Shoot()
    {
        if (Time.time > lastShootTime + shootInterval)
        {
            lastShootTime = Time.time;
            Debug.Log("fire");
            if (projectilePrefab)
            {
                Vector3 shotDir = curAimPoint - weaponMuzzle.position;
                //if (Vector3.Angle(shotDir, aimCamera.transform.forward) > 80)
                //{
                //    shotDir = aimCamera.transform.forward;
                //}
                ProjectileBase projectileBase = Instantiate(projectilePrefab, weaponMuzzle.position, Quaternion.LookRotation(shotDir));
                //projectileBase.parentWeaponController = this;
                projectileBase.owner = owner;
                projectileBase.weapon = gameObject;
                projectileBase.bulletLayerMask = shotLayermask;
                audioSource.PlayOneShot(shotSFX);
            }
        }
    }

    
}
