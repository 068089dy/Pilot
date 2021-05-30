using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("On Collision Enter.");
        Debug.Log(collision.transform.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("On Triger Enter.");
        Debug.Log(other.transform.gameObject.name);
    }
}
