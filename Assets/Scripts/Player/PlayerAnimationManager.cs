using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationManager : MonoBehaviour
{
    Animator m_Animator;
    public HandPoser leftHandPoser;
    public FullBodyBipedIK fullBodyIk;
    public bool enableLeftHandIk;
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        enableLeftHandIk = true;
    }

    private void Update()
    {
        if (enableLeftHandIk)
        {
            float lerpV = Mathf.Lerp(leftHandPoser.weight, 1, Time.deltaTime * 10f);
            fullBodyIk.solver.leftHandEffector.positionWeight = lerpV;
            fullBodyIk.solver.leftHandEffector.rotationWeight = lerpV;
            leftHandPoser.weight = lerpV;
            
        } else
        {
            float lerpV = Mathf.Lerp(leftHandPoser.weight, 0, Time.deltaTime * 20f);
            fullBodyIk.solver.leftHandEffector.positionWeight = lerpV;
            fullBodyIk.solver.leftHandEffector.rotationWeight = lerpV;
            leftHandPoser.weight = lerpV;
            
        }
    }

    public void SetLeftHandPoser(Transform transform)
    {
        leftHandPoser.poseRoot = transform;
    }

    public void SetLeftHandIk(Transform transform)
    {
        fullBodyIk.solver.leftHandEffector.target = transform;
    }


    public void SetLayerEnable(int layerIndex)
    {
        //m_Animator.SetLayerWeight(layerIndex, 1);
        for (int i = 0; i < m_Animator.layerCount; i++)
        {
            if (i == layerIndex)
            {
                m_Animator.SetLayerWeight(layerIndex, 1);
            }else
            {
                m_Animator.SetLayerWeight(i, 0);
            }
        }
    }

    public void Reload()
    {
        m_Animator.SetBool("reload", true);
    }

    public void Run()
    {
        m_Animator.SetBool("run", true);
    }

    public void Idle()
    {
        m_Animator.SetBool("run", false);
        m_Animator.SetBool("aim", false);
        m_Animator.SetBool("shoot", false);
        m_Animator.SetBool("reload", false);
    }

    public void Aim()
    {
        m_Animator.SetBool("aim", true);
    }

    public void Shoot()
    {
        m_Animator.SetBool("shoot", true);
    }

    public void StopShoot()
    {
        m_Animator.SetBool("shoot", false);
    }

    public void Melee()
    {
        Idle();
        m_Animator.SetBool("melee", true);
    }

    public void StopMelee()
    {
        m_Animator.SetBool("melee", false);
    }

    public AnimatorStateInfo GetCurrentAnimatorStateInfo(int Layer)
    {
        return m_Animator.GetCurrentAnimatorStateInfo(Layer);
    }

    public float GetLayerWeight(int Layer)
    {
        return m_Animator.GetLayerWeight(Layer);
    }

    //public int GetAnimatorLayerIndex()
    //{
    //    //return m_Animator.
    //}
}
