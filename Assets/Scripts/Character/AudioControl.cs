using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioControl : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip walkAudio;
    //public AudioClip runAudio;
    public AudioClip downAudio;
    public AudioClip hookRopeAudio;

    float m_footstepDistanceCounter;
    float m_lastHookingAudioTime;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Idle()
    {
        audioSource.Stop();
    }

    public void Down()
    {
        audioSource.PlayOneShot(downAudio);
    }

    public void Hooking()
    {
        if (m_lastHookingAudioTime + hookRopeAudio.length < Time.time)
        {
            
            audioSource.PlayOneShot(hookRopeAudio);
            m_lastHookingAudioTime = Time.time;
        }
    }

    public void Walk()
    {
        if (m_footstepDistanceCounter + 0.4f < Time.time)
        {
            audioSource.PlayOneShot(walkAudio);
            m_footstepDistanceCounter = Time.time;
        }
        
    }

    public void Run()
    {
        if (m_footstepDistanceCounter + 0.25f < Time.time)
        {
            audioSource.PlayOneShot(walkAudio);
            m_footstepDistanceCounter = Time.time;
        }

    }
}
