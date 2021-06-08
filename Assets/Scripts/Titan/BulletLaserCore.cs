using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BulletLaserCore : MonoBehaviour
{
    // 蓄力时长
    public float chargeDuration = 1.8f;
    // 总时长
    public float duration = 10f;
    public float damage = 50;
    float startChargeTime;
    public float maxDistance = 100;

    // 特效
    public ParticleSystem chargeFX1;
    public ParticleSystem chargeFX2;
    public ParticleSystem exmisionFX1;
    public LineRenderer laserFX;
    public float laserWidth = 0.8f;
    public ParticleSystem hitFX;

    //public LayerMask hitLayer;
    public Actor actor;

    AudioSource audioSource;

    public AudioClip chargeSFX;
    bool isChargeAudioPlayed;
    public AudioClip emissionSFX;
    public AudioClip laserSFX;
    bool isEmissionAudioPlayed;
    public Actor owner;
    public Camera aimCamera;

    public bool isLaunching;

    TitanUltimateSkillManager titanUltimateSkillManager;

    // Start is called before the first frame update
    void Start()
    {
        chargeFX1.Stop();
        chargeFX2.Stop();
        exmisionFX1.Stop();
        laserFX.enabled = false;
        laserFX.startWidth = laserWidth;
        laserFX.SetPosition(1, Vector3.forward * maxDistance);
        audioSource = GetComponent<AudioSource>();
        hitFX.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //hitFX.GetComponent<Renderer>().enabled = false;
        //if (hitFX.isPlaying)
        //    hitFX.Stop();
        if (isLaunching)
        {
            if (Time.time < duration + startChargeTime)
            {
                // 蓄力中
                if (Time.time < startChargeTime + chargeDuration)
                {
                    if (!isChargeAudioPlayed)
                    {
                        audioSource.PlayOneShot(chargeSFX);
                        //audioSource.PlayOneShot(emissionSFX);
                        audioSource.PlayOneShot(laserSFX);
                        isChargeAudioPlayed = true;
                        chargeFX1.Simulate(0.0f);
                        chargeFX2.Simulate(0.0f);
                        chargeFX1.GetComponent<Renderer>().enabled = true;
                        chargeFX2.GetComponent<Renderer>().enabled = true;
                        chargeFX1.Play();
                        chargeFX2.Play();
                    }
                }
                else
                {
                    // 发射
                    if (!isEmissionAudioPlayed)
                    {
                        audioSource.PlayOneShot(emissionSFX);
                        //audioSource.PlayOneShot(laserSFX);
                        isEmissionAudioPlayed = true;
                    }
                    chargeFX1.Stop();
                    chargeFX2.Stop();
                    exmisionFX1.Play();
                    chargeFX1.GetComponent<Renderer>().enabled = false;
                    chargeFX2.GetComponent<Renderer>().enabled = false;
                    laserFX.enabled = true;
                    // 最后一秒减小射线宽度
                    if (Time.time > startChargeTime + duration - 0.5f)
                    {
                        laserFX.startWidth -= Time.deltaTime * 0.5f;
                        laserFX.endWidth -= Time.deltaTime * 0.5f;
                    }
                    // 伤害
                    if (Physics.Raycast(aimCamera.transform.position, aimCamera.transform.forward, out RaycastHit hit, maxDistance, actor.canHitLayerMask, QueryTriggerInteraction.Ignore))
                    {
                        Debug.Log("hit" + hit.point);
                        laserFX.SetPosition(1, transform.InverseTransformPoint(hit.point));
                        //hitFX.GetComponent<Renderer>().enabled = true;
                        if (!hitFX.isPlaying)
                            hitFX.Play();
                        hitFX.transform.position = hit.point;
                        if (hit.transform.GetComponent<Damagable>())
                        {
                            Debug.Log("伤害");
                            AttackMsg attackMsg = new AttackMsg(damage * Time.deltaTime, owner, ProtectileType.LASER);
                            hit.transform.GetComponent<Damagable>().BeHurt(attackMsg);
                        }
                    } else
                    {
                        if (hitFX.isPlaying)
                            hitFX.Stop();
                        laserFX.SetPosition(1, transform.InverseTransformPoint(aimCamera.transform.position + aimCamera.transform.forward * maxDistance));
                    }
                }
            } else
            {
                isLaunching = false;
                laserFX.enabled = false;
                isChargeAudioPlayed = false;
                isEmissionAudioPlayed = false;
                audioSource.Stop();
                exmisionFX1.Stop();
                hitFX.Stop();
                titanUltimateSkillManager.isRunning = false;
            }
        }
    }

    public void Launch(TitanUltimateSkillManager titanUltimateSkillManager)
    {
        this.titanUltimateSkillManager = titanUltimateSkillManager;
        titanUltimateSkillManager.isRunning = true;
        isLaunching = true;
        startChargeTime = Time.time;
        laserFX.startWidth = laserWidth;
        laserFX.endWidth = laserWidth;
        hitFX.Stop();
    }


}
