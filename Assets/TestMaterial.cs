using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMaterial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            GetComponent<Renderer>().material.SetColor("Base Color", Color.black);
        }
    }
}
