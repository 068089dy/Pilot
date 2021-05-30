using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [System.NonSerialized]
    public WeaponController parentWeaponController;
    [System.NonSerialized]
    public GameObject owner;
    [System.NonSerialized]
    public GameObject weapon;
    public float shotSpeed = 300;
    public float maxLifeTime = 5;
    public int damage = 15;
    [System.NonSerialized]
    public LayerMask bulletLayerMask = -1;

    public Vector3 lastFramePos;

    public GameObject HitFXPrefab;
    public float hitFxLifeTime = 0.3f;

    private void OnEnable()
    {
        Destroy(gameObject, maxLifeTime);
    }
    // Start is called before the first frame update
    void Start()
    {
        lastFramePos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * shotSpeed * Time.deltaTime, Space.World);
        Vector3 detectionVector = transform.position - lastFramePos;
        if (Physics.Raycast(lastFramePos, detectionVector, out RaycastHit hit, detectionVector.magnitude, bulletLayerMask, QueryTriggerInteraction.Ignore))
        {
            
            if (hit.transform.gameObject.GetComponent<Damagable>())
            {
                Debug.Log("Уќжа" + hit.point);
                hit.transform.gameObject.GetComponent<Damagable>().BeHurt(damage, owner);
                if (parentWeaponController)
                {
                    parentWeaponController.Hited();
                }
            }
            if (HitFXPrefab)
            {
                GameObject fx = Instantiate(HitFXPrefab, hit.point, Quaternion.identity);
                Destroy(fx, hitFxLifeTime);
            }
            Destroy(gameObject);
        }
        lastFramePos = transform.position;
    }
}
