using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TitanAnimationController : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
