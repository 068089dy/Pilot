using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPositon : MonoBehaviour
{
    public Vector3 localPos;
    public Vector3 pos;
    public Vector3 localEuler;
    public Vector3 euler;
    // Start is called before the first frame update
    void Start()
    {
        localPos = transform.localPosition;
        pos = transform.position;
        localEuler = transform.localEulerAngles;
        euler = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = localPos;
        transform.position = pos;
        transform.eulerAngles = euler;
        transform.localEulerAngles = localEuler;
    }
}
