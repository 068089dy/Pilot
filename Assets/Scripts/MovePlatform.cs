using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    //public GameObject curObject;
    public Vector3 velocity = Vector3.up;

    public Transform startTransform;
    public Transform endTransform;

    public int curPosIndex = 0;
    bool launch;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //velocity = Vector3.up * (Mathf.Sin(Time.time * 0.2f)*8);
        //transform.Translate(velocity * Time.deltaTime);
        if (launch)
        {
            if (curPosIndex == 0)
            {
                transform.position = Vector3.Lerp(transform.position, endTransform.position, Time.deltaTime * 0.5f);
                if (Vector3.Distance(transform.position, endTransform.position) < 0.1f)
                {
                    curPosIndex = 1;
                    launch = false;
                }
            }
            else if (curPosIndex == 1)
            {
                transform.position = Vector3.Lerp(transform.position, startTransform.position, Time.deltaTime * 0.5f);
                if (Vector3.Distance(transform.position, startTransform.position) < 0.1f)
                {
                    curPosIndex = 0;
                    launch = false;
                }
            }
        }
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

    public void Launch()
    {
        launch = true;
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
