using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBatteryController : MonoBehaviour
{
    public Transform VertiacalRotateHandle;
    public Transform HorizantolRotateHandle;
    public float maxDistance = 500;
    public Health health;
    public float damage = 50;

    public float maxAngle = 40;
    public float lerpSpeed = 0.3f;
    public Actor target;
    //public Transform targetHitTransform;
    bool isTargetVisible;

    //public LayerMask layerMask = -1;

    public List<Actor> targetList;

    // 射击
    public Transform Mullze;
    //public ProjectileBase BulletPrefab;
    public LineRenderer laserLine;
    public ParticleSystem hitFX;
    public float coolingTime = 1;
    float lastShootTime;
    BatteryState curState;

    public GameObject diedFX;
    Actor actor;
    TeamManager teamManager;


    float lastChangeTargetTime;
    // Start is called before the first frame update
    void Start()
    {
        laserLine.enabled = false;
        diedFX.SetActive(false);
        actor = GetComponent<Actor>();
        teamManager = FindObjectOfType<TeamManager>();
        if (actor.team == Team.TEAM1)
        {
            targetList = teamManager.team2Actors;
        }
        else if (actor.team == Team.TEAM2)
        {
            targetList = teamManager.team1Actors;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health.hp > 0)
        {
            curState = BatteryState.NORMAL;
        }
        else
        {
            curState = BatteryState.DIED;
            teamManager.UnRegisterActor(actor);
        }
        if (curState == BatteryState.NORMAL)
        {
            if (diedFX.activeInHierarchy)
            {
                diedFX.SetActive(false);
            }
            if (actor.lastBeHurtTime >= 0
            && Time.time < actor.lastBeHurtTime + 5f
            && actor.lastDamageMsg.shooter
            && actor.lastDamageMsg.shooter.gameObject.activeInHierarchy)
            {
                if (actor.lastDamageMsg.shooter != target && Time.time > lastChangeTargetTime + 2f)
                {
                    target = actor.lastDamageMsg.shooter;
                    lastChangeTargetTime = Time.time;
                }
            }
            else
            {
                findTargetTransform();
            }
            
            if (target)
            {
                HandleRotate();
                HandleShoot();
                //laserLine.enabled = true;
            } else
            {
                laserLine.enabled = false;
                var ems = hitFX.emission;
                ems.enabled = false;
            }
        }
        else if (curState == BatteryState.DIED)
        {
            if (!diedFX.activeInHierarchy)
            {
                diedFX.SetActive(true);
                laserLine.enabled = false;
                hitFX.gameObject.SetActive(false);
            }
        }


    }

    void HandleShoot()
    {
        if (Time.time > lastShootTime + coolingTime)
        {
            lastShootTime = Time.time;
            //ProjectileBase projectileBase = Instantiate(BulletPrefab, Mullze.position, Quaternion.LookRotation(VertiacalRotateHandle.forward));
            ////projectileBase.parentWeaponController = this;
            //projectileBase.owner = gameObject;
            //projectileBase.weapon = gameObject;
            //projectileBase.bulletLayerMask = layerMask;
        }
        laserLine.enabled = true;
        var ems = hitFX.emission;
        ems.enabled = false;
        // 小于3度时，子弹直接射向目标，否则沿着枪口方向射出
        if (Physics.Raycast(laserLine.transform.position, laserLine.transform.forward, out RaycastHit hit, maxDistance, actor.canHitLayerMask, QueryTriggerInteraction.Ignore))
        {
            ems.enabled = true;
            hitFX.transform.position = hit.point;
            laserLine.SetPosition(1, Vector3.forward * Vector3.Distance(hit.point, laserLine.transform.position));
            if (hit.transform.GetComponent<Damagable>())
            {
                AttackMsg attackMsg = new AttackMsg(damage*Time.deltaTime, gameObject.GetComponent<Actor>(), ProtectileType.LASER);
                hit.transform.GetComponent<Damagable>().BeHurt(attackMsg);
                
            }
        }

    }

    void HandleRotate()
    {
        //target = findTargetTransform();
        if (target)
        {
            Vector3 targetHorizantolDir = target.damagePoint.transform.position - HorizantolRotateHandle.transform.position;
            targetHorizantolDir.y = 0;
            // 绕y轴旋转
            HorizantolRotateHandle.forward = Vector3.Lerp(HorizantolRotateHandle.forward, targetHorizantolDir, Time.deltaTime * lerpSpeed);

            Vector3 targetVertical = target.damagePoint.transform.position - VertiacalRotateHandle.position;
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

    Actor findTargetTransform()
    {
        target = null;
        //targetHitTransform = null;

        float minDistance = maxDistance;
        if (targetList.Count > 0)
        {
            foreach (Actor ob in targetList)
            {
                if (ob)
                {
                    if (Vector3.Distance(ob.transform.position, transform.position) < minDistance)
                    {
                        isTargetVisible = false;
                        Transform obHitPoint = ob.transform;
                        if (ob.damagePoint)
                        {
                            obHitPoint = ob.damagePoint;
                        }
                        checkTargetVisible(ob, obHitPoint);
                        if (isTargetVisible)
                        {
                            Debug.Log("发现目标" + ob.name);
                            minDistance = Vector3.Distance(ob.transform.position, transform.position);
                            target = ob;
                            //targetHitTransform = target;
                            //if (obHitPoint)
                            //{
                            //    targetHitTransform = obHitPoint;
                            //}
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

    bool checkTargetVisible(Actor curT, Transform hitPoint)
    {
        isTargetVisible = false;
        //Transform curT = targetHitTransform;
        if (Physics.Raycast(VertiacalRotateHandle.position, hitPoint.position - VertiacalRotateHandle.position, out RaycastHit hit, maxDistance, actor.canHitLayerMask, QueryTriggerInteraction.Ignore))
        {
            //Debug.DrawLine(VertiacalRotateHandle.position, curT);
            //Debug.Log(hit.transform.gameObject.name + (hit.transform == curT.transform));
            // 如果击中的目标是当前目标，或者当前目标的
            if (hit.transform.gameObject.GetComponent<Damagable>())
            {
                if (hit.transform.gameObject.GetComponent<Damagable>().parentActor == curT)
                {
                    isTargetVisible = true;
                }
            }
            //if (hit.transform == curT.transform)
            //{
            //    isTargetVisible = true;
            //}
        }
        return isTargetVisible;
    }
}
