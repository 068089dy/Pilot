using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RootMotion.FinalIK;

//[RequireComponent(typeof(WeaponController))]
public class WeaponRifle : MonoBehaviour
{
    public PlayerWeaponManager m_PlayerWeaponManager;
    WeaponController m_WeaponController;
    public InputHandler m_InputHandler;
    public AudioSource m_AudioSource;
    public AudioClip shootSFX;
    public Image ammoLoadBar;

    public Animator animator;
    public PlayerAnimationManager m_PlayerAnimationManager;
    //public HandPoser leftHandPoser;
    //public FullBodyBipedIK fullBodyBipedIK;
    public float shootInterval = 0.1f;
    // 弹夹容量
    //public int m_WeaponController.magazineSize = 40;
    // 弹药数量
    //public int m_WeaponController.ammoNum = 800;
    // 当前弹夹弹药数量
    //public int m_WeaponController.curMagazineNum = 20;
    float lastShootTime;


    // 弹夹相关
    //public Transform leftHandTransform;
    //public Transform rifleTransform;
    public Transform clipTransform;
    //bool clipInHand;
    // Start is called before the first frame update
    void Start()
    {
        m_WeaponController = GetComponent<WeaponController>();
        m_WeaponController.StopWeapon += StopRifle;
        m_WeaponController.StartWeapon = StartRifle;
        m_WeaponController.ShootAction = Shoot;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_PlayerWeaponManager.isLoading)
            ListenReloadAnimation();
        // 换弹
        if (m_InputHandler.GetReloadInputDown() && m_WeaponController.curMagazineNum < m_WeaponController.magazineSize && m_WeaponController.ammoNum > 0)
        {

            m_PlayerWeaponManager.isLoading = true;
            m_PlayerWeaponManager.isAiming = false;
            m_PlayerWeaponManager.isShooting = false;
            m_PlayerAnimationManager.enableLeftHandIk = false;
            //leftHandPoser.weight = 0;
            //fullBodyBipedIK.solver.leftHandEffector.positionWeight = 0;
            //fullBodyBipedIK.solver.leftHandEffector.rotationWeight = 0;
        }
        ammoLoadBar.fillAmount = (float)m_WeaponController.curMagazineNum / (float)m_WeaponController.magazineSize;
    }

    void ListenReloadAnimation()
    {
        //clipInHand = false;
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(1);
        if (info.IsName("FPRifleReload"))
        {
            if (info.normalizedTime >= 0.23f)
            {
            }
            m_PlayerAnimationManager.enableLeftHandIk = false;
            //leftHandPoser.weight = 0;
            //fullBodyBipedIK.solver.leftHandEffector.positionWeight = 0;
            //fullBodyBipedIK.solver.leftHandEffector.rotationWeight = 0;
            if (info.normalizedTime >= 0.95f)
            {
                m_PlayerWeaponManager.isLoading = false;
                Reload();
            }
        }
        else
        {
            m_PlayerAnimationManager.enableLeftHandIk = true;
            //leftHandPoser.weight = Mathf.Lerp(leftHandPoser.weight, 1, Time.deltaTime*5);
            //fullBodyBipedIK.solver.leftHandEffector.positionWeight = Mathf.Lerp(fullBodyBipedIK.solver.leftHandEffector.positionWeight, 1, Time.deltaTime * 5);
            //fullBodyBipedIK.solver.leftHandEffector.rotationWeight = Mathf.Lerp(fullBodyBipedIK.solver.leftHandEffector.rotationWeight, 1, Time.deltaTime * 5);
        }

        if (!m_PlayerWeaponManager.isLoading)
        {
            StopReload();
        }
    }

    private void LateUpdate()
    {
        //if (clipInHand)
        //{
        //    clipTransform.localPosition = new Vector3(-0.00097f, -0.00062f, 0.00088f);
        //    clipTransform.localEulerAngles = new Vector3(-77.837f, -106.9f, 8.56f);
        //}
    }

    void StopRifle()
    {
        StopReload();
        clipTransform.GetComponent<Renderer>().enabled = false;
    }

    void StartRifle()
    {
        clipTransform.GetComponent<Renderer>().enabled = true;
    }

    public void StopReload()
    {
        //clipInHand = false;
        m_PlayerWeaponManager.isLoading = false;
        m_PlayerAnimationManager.enableLeftHandIk = true;
        m_PlayerAnimationManager.Idle();
        //leftHandPoser.weight = 1;
        //fullBodyBipedIK.solver.leftHandEffector.positionWeight = 1;
        //fullBodyBipedIK.solver.leftHandEffector.rotationWeight = 1;
        //clipTransform.SetParent(rifleTransform, false);
        //clipTransform.localPosition = new Vector3(0, 1.697242e-07f, 0);
        //clipTransform.localEulerAngles = Vector3.zero;
        //clipTransform.localScale = Vector3.one;
    }

    void Reload()
    {
        
        int suppNum = m_WeaponController.magazineSize - m_WeaponController.curMagazineNum;
        if (m_WeaponController.ammoNum > suppNum)
        {
            m_WeaponController.ammoNum -= suppNum;
            m_WeaponController.curMagazineNum += suppNum;
        } else
        {
            m_WeaponController.ammoNum = 0;
            m_WeaponController.curMagazineNum += m_WeaponController.ammoNum;
        }
    }

    public void Shoot()
    {
        if (m_WeaponController.curMagazineNum <= 0)
        {
            ListenReloadAnimation();
            return;
        }
        if (Time.time >= lastShootTime + shootInterval && m_WeaponController.curMagazineNum > 0)
        {
            lastShootTime = Time.time;
            m_AudioSource.PlayOneShot(shootSFX);
            m_WeaponController.curMagazineNum--;
            m_WeaponController.Fire();
        }
    }
}
