using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTest : MonoBehaviour
{
    public GameObject rotateBone;
    public GameObject rotateBone1;
    public Camera camera;
    public float mouse_sensitivity = 25;
    Vector3 euler;
    Vector3 euler1;
    Vector3 cameraEuler;
    float angle = 0;
    Vector3 cameraPosOffset;
    Vector3 cameraEulerOffset;
    // Start is called before the first frame update
    void Start()
    {
        euler = rotateBone.transform.eulerAngles;
        euler1 = rotateBone1.transform.eulerAngles;
        cameraPosOffset = camera.transform.position - rotateBone.transform.position;
        cameraEulerOffset = camera.transform.eulerAngles - rotateBone.transform.eulerAngles;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float angleChg = -mouse_sensitivity * Time.deltaTime * Input.GetAxis("Mouse Y");
        rotateBone.transform.eulerAngles = euler;
        rotateBone1.transform.eulerAngles = euler1;
        camera.transform.eulerAngles = cameraEuler;
        if (angle <= 90 && angle >= -90 )
        {
            float angleRes = 2 * angleChg + angle;
            if (angleRes >= -90 && angleRes <= 90)
            {
                rotateBone.transform.Rotate(transform.right, angleRes * 0.5f, Space.World);
                rotateBone1.transform.Rotate(transform.right, angleRes * 0.5f, Space.World);
                camera.transform.Rotate(transform.right, angleRes, Space.World);
                angle = angleRes;
            }
            else if (angleRes < -90)
            {
                rotateBone.transform.Rotate(transform.right, -45, Space.World);
                rotateBone1.transform.Rotate(transform.right, -45, Space.World);
                camera.transform.Rotate(transform.right, -90, Space.World);
                angle = -90;
            }
            else if (angleRes > 90)
            {
                rotateBone.transform.Rotate(transform.right, 45, Space.World);
                rotateBone1.transform.Rotate(transform.right, 45, Space.World);
                camera.transform.Rotate(transform.right, 90, Space.World);
                angle = 90;
            }
        }
        camera.transform.position = rotateBone.transform.position + cameraPosOffset;
        //camera.transform.eulerAngles = rotateBone.transform.eulerAngles + cameraEulerOffset;
        //euler = rotateBone.transform.eulerAngles;
        //euler1 = rotateBone1.transform.eulerAngles;
        //cameraEuler = camera.transform.eulerAngles;
        //rotateBone.transform.eulerAngles = euler;
        //rotateBone.transform.Rotate(transform.right, 45 * Time.deltaTime, Space.World);
        //euler = rotateBone.transform.eulerAngles;
        //euler.x += Time.deltaTime * 10;
        //rotateBone.transform.eulerAngles = euler;

    }
}
