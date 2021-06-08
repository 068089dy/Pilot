using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TitanWeaponController))]
public class TitanWeaponRocket : MonoBehaviour
{
    public InputHandler m_InputHandler;
    public PlayerTitanWeaponManager playerTitanWeaponManager;
    public Camera aimCamera;
    //public LayerMask layerMask = -1;
    TitanWeaponController titanWeaponController;

    public float coolingTime = 2f;
    float lastShootTime;
    float maxRange = 500;

    public TitanBulletRocket bulletRocketPrefab;
    public Transform muzzle;
    //[System.NonSerialized]
    public TitanBulletRocket curTitanBulletRocket;
    Actor owner;

    // 锁定系统
    public MissileLockSystem missileLockSystem;
    // Start is called before the first frame update
    void Start()
    {
        titanWeaponController = GetComponent<TitanWeaponController>();
        curTitanBulletRocket = Instantiate(bulletRocketPrefab, muzzle);
        titanWeaponController.shootAction = Shoot;
        owner = titanWeaponController.owner;
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
            // 不知道为什么有时候没有
            if (!curTitanBulletRocket)
            {
                curTitanBulletRocket = Instantiate(bulletRocketPrefab, muzzle);
            }
            if (missileLockSystem && missileLockSystem.curTarget)
            {
                curTitanBulletRocket.targetActor = missileLockSystem.curTarget;
            }
            else
            {
                if (Physics.Raycast(aimCamera.transform.position,
                    aimCamera.transform.forward,
                    out RaycastHit hit,
                    maxRange,
                    owner.canHitLayerMask,
                    QueryTriggerInteraction.Ignore))
                {
                    curTitanBulletRocket.targetPos = hit.point;
                }
                else
                {
                    curTitanBulletRocket.targetPos = aimCamera.transform.position + aimCamera.transform.forward * maxRange;
                }
            }
            //playerTitanWeaponManager.isShooting = true;
            lastShootTime = Time.time;
            curTitanBulletRocket.transform.SetParent(null, true);
            
            curTitanBulletRocket.layerMask = owner.canHitLayerMask;
            curTitanBulletRocket.owner = owner;
            curTitanBulletRocket.Launch();
            // 重新生成火箭弹
            curTitanBulletRocket = Instantiate(bulletRocketPrefab, muzzle);
            
        }
        else
        {
            //playerTitanWeaponManager.isShooting = true;
        }
    }
   
    //void HandleShoot()
    //{
    //    if (Time.time > coolingTime + lastShootTime)
    //    {
    //        playerTitanWeaponManager.isShooting = false;
    //        if (m_InputHandler.GetFireInputDown())
    //        {
    //            if (Physics.Raycast(aimCamera.transform.position,
    //                aimCamera.transform.forward,
    //                out RaycastHit hit, 
    //                maxRange,
    //                layerMask,
    //                QueryTriggerInteraction.Ignore))
    //            {
    //                curTitanBulletRocket.targetPos = hit.point;
    //            } else
    //            {
    //                curTitanBulletRocket.targetPos = aimCamera.transform.position + aimCamera.transform.forward * maxRange;
    //            }
    //            playerTitanWeaponManager.isShooting = true;
    //            lastShootTime = Time.time;
    //            curTitanBulletRocket.transform.SetParent(null, true);
    //            curTitanBulletRocket.Launch();
    //            curTitanBulletRocket.layerMask = layerMask;
    //            curTitanBulletRocket = Instantiate(bulletRocketPrefab, muzzle);
    //        }
            
    //    } else
    //    {
    //        playerTitanWeaponManager.isShooting = true;
    //    }
        


    //}
}
