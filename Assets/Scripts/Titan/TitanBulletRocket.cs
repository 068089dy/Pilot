using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TitanBulletRocket : MonoBehaviour
{
    [System.NonSerialized]
    public bool launch;
    bool wasLaunched;
    public float speed = 50f;
    public Renderer trail;
    [System.NonSerialized]
    public GameObject owner;
    [System.NonSerialized]
    public Vector3 targetPos;
    public float lifeTime = 10;
    public float rotationSpeed = 0.1f;
    [System.NonSerialized]
    public LayerMask layerMask = -1;

    [Header("Injury")]
    public int centralInjury = 80;
    public float explosionRange = 6;
    public float damageLoss = 10;

    public GameObject ExplosionFXPrefab;
    public AudioClip launchSFX;
    AudioSource audioSource;

    Vector3 startPos;
    Vector3 lastFramePos;
    float launchTime;
    float delayRotationTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        //trail.enabled = false;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (launch)
        {
            if (!wasLaunched)
            {
                audioSource.PlayOneShot(launchSFX);
            }
            if (Time.time >= launchTime + delayRotationTime)
            {
                transform.up = Vector3.Lerp(transform.up, targetPos - transform.position, Time.deltaTime * rotationSpeed);
            }
            transform.Translate(transform.up * Time.deltaTime * speed, Space.World);
            if (Physics.Raycast(
                lastFramePos, 
                transform.position-lastFramePos,
                out RaycastHit hit,
                (transform.position - lastFramePos).magnitude,
                layerMask,
                QueryTriggerInteraction.Ignore))
            {
                RaycastHit[] hits = Physics.SphereCastAll(
                    hit.point,
                    explosionRange,
                    transform.up,
                    0.01f,
                    layerMask,
                    QueryTriggerInteraction.Ignore);
                if (hits.Length > 0)
                {
                    foreach(RaycastHit info in hits)
                    {
                        Debug.Log(info.transform.gameObject.name);
                        //if (info.transform.gameObject.GetComponent<Damagable>())
                        //{
                        //    // º∆À„…À∫¶
                        //    int damage = centralInjury - Mathf.FloorToInt(damageLoss * Vector3.Distance(info.transform.position, hit.point));
                        //    Debug.Log(info.transform.gameObject.name +"…À∫¶"+ damage +"time"+ Time.time);
                        //    info.transform.gameObject.GetComponent<Damagable>().BeHurt(damage);
                        //}
                    }
                }
                GameObject exFx = Instantiate(ExplosionFXPrefab, transform.position, Quaternion.identity);
                Destroy(exFx, 5);
                Destroy(gameObject);
                launch = false;
            }
        }
        wasLaunched = launch;
    }

    public void Launch()
    {
        launchTime = Time.time;
        launch = true;
        trail.enabled = true;
        startPos = transform.position;
        lastFramePos = startPos;
        //audioSource.PlayOneShot(launchSFX);
        Destroy(gameObject, lifeTime);
        
    }
}
