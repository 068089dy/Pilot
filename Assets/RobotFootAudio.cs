using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotFootAudio : MonoBehaviour
{
    bool wasGrounded;
    bool isGrounded;
    AudioSource audioSource;
    public AudioClip footAudioClip;
    public LayerMask floorLayerMask = -1;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        wasGrounded = isGrounded;
        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawLine(transform.position, transform.position + Vector3.down*0.8f);
        if (Physics.Raycast(ray, 0.8f, floorLayerMask))
        {
            isGrounded = true;
        } else
        {
            isGrounded = false;
        }
        if (!wasGrounded && isGrounded)
        {
            audioSource.PlayOneShot(footAudioClip);
        }
    }

    private void OnDrawGizmos()
    {
    }
}
