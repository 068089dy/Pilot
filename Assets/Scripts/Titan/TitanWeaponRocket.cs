using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TitanWeaponController))]
public class TitanWeaponRocket : MonoBehaviour
{
    public InputHandler m_InputHandler;
    public PlayerTitanWeaponManager playerTitanWeaponManager;
    public Camera aimCamera;
    public LayerMask layerMask = -1;
    TitanWeaponController titanWeaponController;

    public float coolingTime = 2f;
    float lastShootTime;
    float maxRange = 500;

    public TitanBulletRocket bulletRocketPrefab;
    public Transform muzzle;
    [System.NonSerialized]
    public TitanBulletRocket curTitanBulletRocket;
    // Start is called before the first frame update
    void Start()
    {
        titanWeaponController = GetComponent<TitanWeaponController>();
        curTitanBulletRocket = Instantiate(bulletRocketPrefab, muzzle);
        titanWeaponController.shootAction = Shoot;
    }

    // Update is called once per frame
    void Update()
    {
        //if (titanWeaponController.active)
        //{
        //    //HandleShoot();
        //}
    }

    void Shoot()
    {
        if (Time.time > coolingTime + lastShootTime)
        {
            //playerTitanWeaponManager.isShooting = false;
            
                if (Physics.Raycast(aimCamera.transform.position,
                    aimCamera.transform.forward,
                    out RaycastHit hit,
                    maxRange,
                    layerMask,
                    QueryTriggerInteraction.Ignore))
                {
                    curTitanBulletRocket.targetPos = hit.point;
                }
                else
                {
                    curTitanBulletRocket.targetPos = aimCamera.transform.position + aimCamera.transform.forward * maxRange;
                }
                //playerTitanWeaponManager.isShooting = true;
                lastShootTime = Time.time;
                curTitanBulletRocket.transform.SetParent(null, true);
                curTitanBulletRocket.Launch();
                curTitanBulletRocket.layerMask = layerMask;
                curTitanBulletRocket = Instantiate(bulletRocketPrefab, muzzle);
            

        }
        else
        {
            //playerTitanWeaponManager.isShooting = true;
        }
    }

    void HandleShoot()
    {
        if (Time.time > coolingTime + lastShootTime)
        {
            playerTitanWeaponManager.isShooting = false;
            if (m_InputHandler.GetFireInputDown())
            {
                if (Physics.Raycast(aimCamera.transform.position,
                    aimCamera.transform.forward,
                    out RaycastHit hit, 
                    maxRange,
                    layerMask,
                    QueryTriggerInteraction.Ignore))
                {
                    curTitanBulletRocket.targetPos = hit.point;
                } else
                {
                    curTitanBulletRocket.targetPos = aimCamera.transform.position + aimCamera.transform.forward * maxRange;
                }
                playerTitanWeaponManager.isShooting = true;
                lastShootTime = Time.time;
                curTitanBulletRocket.transform.SetParent(null, true);
                curTitanBulletRocket.Launch();
                curTitanBulletRocket.layerMask = layerMask;
                curTitanBulletRocket = Instantiate(bulletRocketPrefab, muzzle);
            }
            
        } else
        {
            playerTitanWeaponManager.isShooting = true;
        }
        


    }
}
