using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingLightController : MonoBehaviour
{
    public LineRenderer line;
    public Transform ring;
    public Transform ring1;
    public Transform ring2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ring.Rotate(Vector3.up, Time.deltaTime * 100, Space.Self);
        ring1.Rotate(Vector3.up, -Time.deltaTime * 200, Space.Self);
        ring2.Rotate(Vector3.up, Time.deltaTime * 300, Space.Self);
        line.startWidth = Mathf.Sin(5f * Time.time) * 0.5f + 0.5f;
        line.endWidth = line.startWidth;
        Vector3 ringPos = ring.transform.localPosition;
        ringPos.z = Mathf.Abs(Mathf.Sin(1 * Time.time) * 10);
        ring.transform.localPosition = ringPos;
        Vector3 ring1Pos = ring1.transform.localPosition;
        ring1Pos.z = Mathf.Abs(Mathf.Sin(1 * Time.time + Mathf.PI * 0.5f) * 10 + 10);
        ring1.transform.localPosition = ring1Pos;
        Vector3 ring2Pos = ring2.transform.localPosition;
        ring2Pos.z = Mathf.Abs(Mathf.Cos(1 * Time.time + Mathf.PI * 0.5f) * 10 + 10);
        ring2.transform.localPosition = ring2Pos;
    }
}
