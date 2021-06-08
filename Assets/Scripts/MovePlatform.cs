using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public GameObject curObject;
    public Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = Vector3.up * (Mathf.Sin(Time.time)*5);
        transform.Translate(velocity * Time.deltaTime);
        //Vector3 pos = transform.position;
        //pos.x = -140 + Mathf.Sin(Time.time) * 10;
        //transform.position = pos;
        //if (curObject)
        //{
        //    if (curObject.GetComponent<CharacterController>())
        //    {
        //        //curObject.GetComponent<CharacterController>().velocity = curObject.GetComponent<Rigidbody>().velocity;
        //    }
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Actor>())
        {
            //Debug.Log("上车");
            other.gameObject.transform.SetParent(transform);
        }
        //curObject = other.gameObject;
        //if (other.gameObject.GetComponent<Actor>())
        //{
        //    if (other.gameObject.GetComponent<Actor>().characterType == CharacterType.PLAYER)
        //    {
        //        Debug.Log("上台triger" + other.gameObject.name);
        //        other.gameObject.GetComponent<PlayerCharacterController>().onPlatform = true;
        //        other.gameObject.GetComponent<PlayerCharacterController>().addtionalVelocity = velocity;
        //    }
        //    //other.gameObject.transform.SetParent(transform);
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Actor>())
        {
            //Debug.Log("下车");
            other.gameObject.transform.SetParent(null);
        }
        //if (other.gameObject.GetComponent<Actor>())
        //{
        //    if (other.gameObject.GetComponent<Actor>().characterType == CharacterType.PLAYER)
        //    {
        //        Debug.Log("xai台triger" + other.gameObject.name);
        //        other.gameObject.GetComponent<PlayerCharacterController>().onPlatform = false;
        //        other.gameObject.GetComponent<PlayerCharacterController>().addtionalVelocity = Vector3.zero;
        //    }
        //    //other.gameObject.transform.SetParent(transform);
        //}
    }



    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("上台" + collision.gameObject.name);
    //    if (collision.gameObject.GetComponent<Actor>())
    //    {
    //        collision.gameObject.transform.SetParent(transform);
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    Debug.Log("下台" + collision.gameObject.name);
    //    if (collision.gameObject.GetComponent<Actor>())
    //    {
    //        collision.gameObject.transform.SetParent(null);
    //    }
    //}
}
