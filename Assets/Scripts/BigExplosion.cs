using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BigExplosion : MonoBehaviour
{
    public ParticleSystem[] allParticles;
    AudioSource audioSource;
    public AudioClip explosionSFX;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        foreach (ParticleSystem p in allParticles)
        {
            p.Stop();
        }
    }

    public void Explosion()
    {
        audioSource.PlayOneShot(explosionSFX);
        foreach (ParticleSystem p in allParticles)
        {
            p.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
