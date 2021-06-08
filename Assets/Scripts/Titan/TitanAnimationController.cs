using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class TitanAnimationController : MonoBehaviour
{
    Animator animator;
    //FullBodyBipedIK fullBodyIk;
    public FABRIK rightArmFABRIK;


    //public Transform leftHandAimPoint;
    //public Transform rightHandAimPoint;
    //public Transform rightShoulderPoint;
    public float rhAimToUpperArmDistance = 3;
    Transform aimTarget;
    public bool enableRightHandIk;
    public bool enableLeftHandIk;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //fullBodyIk = GetComponent<FullBodyBipedIK>();
        //rhAimToUpperArmDistance = Vector3.Distance(fullBodyIk.references.rightUpperArm.transform.position, rightHandAimPoint.position);
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (enableLeftHandIk)
        //{
        //    float lerpV = Mathf.Lerp(fullBodyIk.solver.leftHandEffector.positionWeight, 1, Time.deltaTime * 10f);
        //    fullBodyIk.solver.leftHandEffector.positionWeight = lerpV;
        //    fullBodyIk.solver.leftHandEffector.rotationWeight = lerpV;

        //}
        //else
        //{
        //    float lerpV = Mathf.Lerp(fullBodyIk.solver.leftHandEffector.positionWeight, 0, Time.deltaTime * 20f);
        //    fullBodyIk.solver.leftHandEffector.positionWeight = lerpV;
        //    fullBodyIk.solver.leftHandEffector.rotationWeight = lerpV;
        //}

        if (enableRightHandIk)
        {
            float lerpV = Mathf.Lerp(rightArmFABRIK.solver.IKPositionWeight, 1, Time.deltaTime * 3f);
            rightArmFABRIK.solver.IKPositionWeight = lerpV;
            //fullBodyIk.solver.rightHandEffector.positionWeight = lerpV;
            //fullBodyIk.solver.rightHandEffector.rotationWeight = lerpV;
            //rightHandAimPoint.position = rhAimToUpperArmDistance * (aimTarget.position - rightShoulderPoint.position).normalized + rightShoulderPoint.position;
        }
        else
        {
            float lerpV = Mathf.Lerp(rightArmFABRIK.solver.IKPositionWeight, 0, Time.deltaTime * 5f);
            rightArmFABRIK.solver.IKPositionWeight = lerpV;
            //fullBodyIk.solver.rightHandEffector.positionWeight = lerpV;
            //fullBodyIk.solver.rightHandEffector.rotationWeight = lerpV;
        }
    }

    public void Idle()
    {
        animator.SetBool("walk", false);
        animator.SetBool("run", false);
        animator.SetBool("gun1", false);
        animator.SetBool("gun2", false);
        animator.SetBool("shoot", false);
        animator.SetBool("squat", false);
        animator.SetBool("air", false);
    }

    public void SetRightHandIk(Transform transform)
    {
        //fullBodyIk.solver.rightHandEffector.target = transform;
        aimTarget = transform;
        rightArmFABRIK.solver.target = aimTarget;
        enableRightHandIk = true;
    }

    public void DisableRightHandIK()
    {
        enableRightHandIk = false;
    }

    

    public void SetLeftHandIk(Transform transform)
    {
        //fullBodyIk.solver.leftHandEffector.target = transform;
        enableLeftHandIk = true;
    }

    public void DisableLeftHandIK()
    {
        enableLeftHandIk = false;
    }

    public void Run()
    {
        animator.SetBool("run", true);
        animator.SetBool("gun1", false);
        animator.SetBool("gun2", false);
        animator.SetBool("shoot", false);
    }

    public void Walk()
    {
        animator.SetBool("walk", true);
        animator.SetBool("gun1", false);
        animator.SetBool("gun2", false);
        animator.SetBool("shoot", false);
    }

    public void Weapon1()
    {
        animator.SetBool("gun1", true);
        animator.SetBool("gun2", false);
    }

    public void Weapon2()
    {
        animator.SetBool("gun2", true);
        animator.SetBool("gun1", false);
    }

    public void Shoot()
    {
        animator.SetBool("shoot", true);
    }

    public void Down()
    {
        animator.SetTrigger("down");
    }

    public void Air()
    {
        animator.SetBool("air", true);
    }
    public void Squat()
    {
        animator.SetBool("squat", true);
        animator.SetBool("air", false);
    }

    public void SetLayerEnable(int layerIndex)
    {
        //m_Animator.SetLayerWeight(layerIndex, 1);
        for (int i = 0; i < animator.layerCount; i++)
        {
            if (i == layerIndex)
            {
                animator.SetLayerWeight(layerIndex, 1);
            }
            else
            {
                animator.SetLayerWeight(i, 0);
            }
        }
    }
}
