using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Animator))]
public class AnimationControl : MonoBehaviour
{
    public Animator animator;
    public TwoBoneIKConstraint leftHandIk;
    public Transform leftHandIkTarget;
    public Transform gunLeftHandPos;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Run()
    {
        animator.SetBool(AnimationEnum.RUN, true);
    }

    public void Walk()
    {
        animator.SetBool(AnimationEnum.WALK, true);
    }

    public void Idle()
    {
        animator.SetBool(AnimationEnum.RUN, false);
        animator.SetBool(AnimationEnum.WALK, false);
        animator.SetBool(AnimationEnum.AIM, false);
        animator.SetBool(AnimationEnum.HOOK, false);
        animator.SetBool(AnimationEnum.RELOAD, false);
    }

    public void Aim()
    {
        animator.SetBool(AnimationEnum.AIM, true);
    }

    public void Reload()
    {
        animator.SetBool(AnimationEnum.RELOAD, true);
    }

    public void Hook()
    {
        animator.SetBool(AnimationEnum.HOOK, true);
        animator.SetBool(AnimationEnum.RUN, false);
        animator.SetBool(AnimationEnum.WALK, false);
        animator.SetBool(AnimationEnum.AIM, false);
        animator.SetBool(AnimationEnum.HOOK, false);
    }

    public void closeLeftHandIk()
    {
        leftHandIk.weight = 0;
    }

    public void leftHandToPos(Vector3 pos)
    {
        leftHandIk.weight = 1;
        leftHandIkTarget.transform.position = pos;
    }

    public void leftHandToGun()
    {
        leftHandIk.weight = 1;
        leftHandIkTarget.transform.position = gunLeftHandPos.transform.position;
        leftHandIkTarget.transform.eulerAngles = gunLeftHandPos.transform.eulerAngles;
    }
}

public class AnimationEnum
{
    public static string RUN = "run";
    public static string WALK = "walk";
    public static string AIM = "aim";
    public static string IDLE = "idle";
    public static string RELOAD = "reload";
    public static string HOOK = "hook";
}
