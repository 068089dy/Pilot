using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TitanBulletLaser : MonoBehaviour
{

    public GameObject hitVFXPrefab;
    public bool launching;
    GameObject hitVFX;
    LineRenderer lineRender;
    // Start is called before the first frame update
    void Start()
    {
        lineRender = GetComponent<LineRenderer>();
        
        if (hitVFXPrefab)
        {
            hitVFX = Instantiate(hitVFXPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (launching)
        {
            lineRender.enabled = true;
            //lineRender.SetPosition(0, transform.position);
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100))
            {
                hitVFX.GetComponent<Renderer>().enabled = true;
                hitVFX.transform.position = hit.point;
                lineRender.SetPosition(1, (hit.point - transform.position).magnitude * new Vector3(0, 0, 1));
            }
            else
            {
                hitVFX.GetComponent<Renderer>().enabled = false;
                lineRender.SetPosition(1, Vector3.forward * 10000);
            }
        } else
        {
            lineRender.enabled = false;
            hitVFX.GetComponent<Renderer>().enabled = false;
        }
    }

    
}
