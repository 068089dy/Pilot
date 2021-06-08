using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponController : MonoBehaviour
{
    // 枪口
    public Transform weaponMuzzle;
    public Camera aimCamera;
    //public LayerMask shotLayermask = -1;
    public float maxShotDistance = 500;

    public Actor owner;
    public ProjectileBase projectilePrefab;
    //public HitHint hitHint;
    public PlayerUIManager playerUIManager;

    Vector3 curAimPoint;
    public UnityAction StopWeapon;
    public UnityAction StartWeapon;
    public UnityAction ShootAction;

    public int magazineSize = 50;
    public int curMagazineNum = 50;
    public int ammoNum = 800;

    public bool active;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        curAimPoint = aimCamera.transform.position - aimCamera.transform.forward * maxShotDistance;
        if (Physics.Raycast(aimCamera.transform.position, aimCamera.transform.forward, out RaycastHit hit, maxShotDistance, owner.canHitLayerMask, QueryTriggerInteraction.Ignore))
        {
            curAimPoint = hit.point;
        }
    }

    public void Shoot()
    {
        ShootAction?.Invoke();
    }

    // 供具体的枪械组件调用
    public void Fire()
    {
        if (projectilePrefab)
        {
            Vector3 shotDir = curAimPoint - weaponMuzzle.position;
            if (Vector3.Angle(shotDir, aimCamera.transform.forward) > 80)
            {
                shotDir = aimCamera.transform.forward;
            }
            ProjectileBase projectileBase = Instantiate(projectilePrefab, weaponMuzzle.position, Quaternion.LookRotation(shotDir));
            //projectileBase.parentWeaponController = this;
            projectileBase.owner = owner;
            projectileBase.weapon = gameObject;
            projectileBase.bulletLayerMask = owner.canHitLayerMask;
        }
    }

    //public void Hited()
    //{
    //    if (playerUIManager)
    //    {
    //        playerUIManager.ShowHitFeedback();
    //    }
    //}

    //public void Hited(DamageMsg damageMsg)
    //{
    //    if (playerUIManager && damageMsg.damage > 0)
    //    {
    //        playerUIManager.ShowHitFeedback();
    //    }
    //}

    public void DisableWeapon()
    {
        active = false;
        gameObject.SetActive(false);
        StopWeapon?.Invoke();
    }

    public void EnableWeapon()
    {
        active = true;
        gameObject.SetActive(true);
        StartWeapon?.Invoke();
    }
}
