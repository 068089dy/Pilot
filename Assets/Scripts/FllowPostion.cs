using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FllowPostion : MonoBehaviour
{
    public Transform followTransform;
    public bool x;
    public bool y;
    public bool z;

    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = followTransform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        if (z)
            position.z = followTransform.position.z - offset.z;
        if (y)
            position.y = followTransform.position.y - offset.y;
        if (x)
            position.x = followTransform.position.x - offset.x;
        transform.position = position;
    }
}
