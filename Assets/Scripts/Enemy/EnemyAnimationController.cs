using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AimIK))]
public class EnemyAnimationController : MonoBehaviour
{
    Animator animator;
    AimIK aimIK;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        aimIK = GetComponent<AimIK>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Idle()
    {
        animator.SetBool("shoot", false);
        animator.SetBool("run", false);
        animator.SetBool("walk", false);
        animator.SetBool("aim", false);
        animator.SetBool("fly", false);
    }

    public void Fly()
    {
        animator.SetBool("fly", true);
    }

    public void Walk()
    {
        animator.SetBool("walk", true);
    }

    public void Run()
    {
        animator.SetBool("run", true);
    }

    public void Aim()
    {
        animator.SetBool("aim", true);
    }

    public void Shoot()
    {
        animator.SetBool("shoot", true);
    }

    public void EnableAimIk()
    {
        aimIK.solver.IKPositionWeight = 1;
    }

    public void SetAimIkTarget(Transform target)
    {
        aimIK.solver.target = target;
    }

    public void DisableAimIk()
    {
        aimIK.solver.IKPositionWeight = 0;
    }
}
