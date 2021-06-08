using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryController : MonoBehaviour
{
    public Transform VertiacalRotateHandle;
    public Transform HorizantolRotateHandle;
    public float maxDistance = 500;
    public Health health;

    public float maxAngle = 40;
    public float lerpSpeed = 0.3f;
    public Transform target;
    public Transform targetHitTransform;
    bool isTargetVisible;

    public LayerMask layerMask = -1;

    public GameObject[] targetList;

    // 射击
    public Transform Mullze;
    public ProjectileBase BulletPrefab;
    public float coolingTime = 1;
    AudioSource audioSource;
    public AudioClip shootSFX;
    float lastShootTime;
    BatteryState curState;

    public GameObject diedFX;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        diedFX.SetActive(false);
        if (gameObject.tag == "group1")
        {
            targetList = GameObject.FindGameObjectsWithTag("group2");
        }
        else if (gameObject.tag == "group2")
        {
            targetList = GameObject.FindGameObjectsWithTag("group1");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health.hp > 0)
        {
            curState = BatteryState.NORMAL;
        } else
        {
            curState = BatteryState.DIED;
        }
        if (curState == BatteryState.NORMAL)
        {
            if (diedFX.activeInHierarchy)
            {
                diedFX.SetActive(false);
            }
            findTargetTransform();
            //checkTargetVisible();
            if (target)
            {
                HandleRotate();
                HandleShoot();
            }
        } else if (curState == BatteryState.DIED)
        {
            if (!diedFX.activeInHierarchy)
            {
                diedFX.SetActive(true);
            }
        }
        

    }

    void HandleShoot()
    {
        if (Time.time > lastShootTime + coolingTime)
        {
            audioSource.PlayOneShot(shootSFX);
            lastShootTime = Time.time;
            ProjectileBase projectileBase = Instantiate(BulletPrefab, Mullze.position, Quaternion.LookRotation(VertiacalRotateHandle.forward));
            //projectileBase.parentWeaponController = this;
            projectileBase.owner = gameObject.GetComponent<Actor>();
            projectileBase.weapon = gameObject;
            projectileBase.bulletLayerMask = layerMask;
        }
    }

    void HandleRotate()
    {
        //target = findTargetTransform();
        if (target)
        {
            Vector3 targetHorizantolDir = targetHitTransform.position - transform.position;
            targetHorizantolDir.y = 0;
            // 绕y轴旋转
            HorizantolRotateHandle.forward = Vector3.Lerp(HorizantolRotateHandle.forward, targetHorizantolDir, Time.deltaTime * lerpSpeed);

            Vector3 targetVertical = targetHitTransform.position - VertiacalRotateHandle.position;
            //targetVertical.x = 0;
            // x轴旋转
            VertiacalRotateHandle.forward = Vector3.Lerp(VertiacalRotateHandle.forward, targetVertical, Time.deltaTime * lerpSpeed);
            float vAngle = VertiacalRotateHandle.localEulerAngles.x;
            if (vAngle > maxAngle && vAngle <= 180)
            {
                vAngle = maxAngle;
            }
            if (vAngle > 180 && vAngle < 360 - maxAngle)
            {
                vAngle = 360 - maxAngle;
            }
            VertiacalRotateHandle.localEulerAngles = Vector3.right * vAngle;
        }
    }

    Transform findTargetTransform()
    {
        target = null;
        targetHitTransform = null;
        
        float minDistance = maxDistance;
        if (targetList.Length > 0)
        {
            foreach (GameObject ob in targetList)
            {
                if (ob.activeInHierarchy)
                {
                    if (Vector3.Distance(ob.transform.position, transform.position) < minDistance)
                    {
                        isTargetVisible = false;
                        Transform obHitPoint = null;
                        if (ob.transform.gameObject.GetComponent<Damagable>())
                        {
                            if (ob.transform.gameObject.GetComponent<Damagable>().parentActor.damagePoint)
                            {
                                obHitPoint = ob.transform.gameObject.GetComponent<Damagable>().parentActor.damagePoint;
                                
                            }
                        }
                        checkTargetVisible(ob.transform, obHitPoint);
                        if (isTargetVisible)
                        {
                            Debug.Log("发现目标" + ob.name);
                            minDistance = Vector3.Distance(ob.transform.position, transform.position);
                            target = ob.transform;
                            targetHitTransform = target;
                            if (obHitPoint)
                            {
                                targetHitTransform = obHitPoint;
                            }
                            // 如果有攻击点，目标为攻击点
                            //if (target.gameObject.GetComponent<Damagable>())
                            //{
                            //    if (target.gameObject.GetComponent<Damagable>().DamagePoint)
                            //    {
                            //        targetHitTransform = target.gameObject.GetComponent<Damagable>().DamagePoint.transform;
                            //    }
                            //}
                        }
                    }
                    //return tf.transform;
                }
            }
            //return targetList[0].transform;
        }
        return target;
    }

    bool checkTargetVisible(Transform curT, Transform hitPoint)
    {
        isTargetVisible = false;
        //Transform curT = targetHitTransform;
        if (Physics.Raycast(VertiacalRotateHandle.position, hitPoint.position - VertiacalRotateHandle.position, out RaycastHit hit, maxDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(VertiacalRotateHandle.position, curT.position);
            Debug.Log(hit.transform.gameObject.name + (hit.transform == curT.transform));
            // 如果击中的目标是当前目标，或者当前目标的
            //if (hit.transform.gameObject.GetComponent<Damagable>())
            //{
            //    if (hit.transform.gameObject.GetComponent<Damagable>().DamagePoint == curT.transform)
            //    {
            //        isTargetVisible = true;
            //    }
            //}
            if (hit.transform == curT.transform)
            {
                isTargetVisible = true;
            }
        }
        return isTargetVisible;
    }
}

class hitTarget
{
    public Transform transform;
    public Transform hitTransform;
}

public enum BatteryState
{
    IDLE,
    NORMAL,
    DIED
}
