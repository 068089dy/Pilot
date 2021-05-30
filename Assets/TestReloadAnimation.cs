using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestReloadAnimation : MonoBehaviour
{
    Animator animator;
    public Transform leftHandTransform;
    public Transform rifleTransform;
    public Transform clipTransform;

    Vector3 posOffset;
    Vector3 rotationOffset;
    Vector3 initPos;
    Vector3 initEuler;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        initPos = clipTransform.localPosition;
        initEuler = clipTransform.localEulerAngles;
        posOffset = clipTransform.localPosition - leftHandTransform.localPosition;
        rotationOffset = clipTransform.localEulerAngles - leftHandTransform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("FPRifleReload"))
        {
            if (info.normalizedTime >= 0.23f)
            {
                
                if (clipTransform.parent == rifleTransform)
                {
                    clipTransform.SetParent(leftHandTransform, true);
                    clipTransform.localPosition = new Vector3(0.00084f, -0.00075f, 0.00122f);
                    clipTransform.localEulerAngles = new Vector3(-71.163f, -63.127f, 51.804f);
                }
                //clipTransform.localPosition = leftHandTransform.localPosition + posOffset;
                //clipTransform.localEulerAngles = leftHandTransform.localEulerAngles + rotationOffset;
            }
            if (info.normalizedTime >= 0.95f)
            {
                //clipTransform.localPosition = initPos;
                //clipTransform.localEulerAngles = initEuler;
                
                if (clipTransform.parent == leftHandTransform)
                {
                    clipTransform.SetParent(rifleTransform, true);
                    clipTransform.localPosition = initPos;
                    clipTransform.localEulerAngles = initEuler;
                }
            }
        }
    }
}
