using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBatch : MonoBehaviour
{
    public InputHandler inputHandler;
    public Camera aimCamera;
    public int missileCount = 10;
    public float launchInterval = 0.4f;
    public float maxRange = 500;
    //public LayerMask layerMask;
    public TitanBulletRocket bulletRocketPrefab;
    public Transform[] muzzles;
    public MissileLockSystem missileLockSystem;
    TitanUltimateSkillManager titanUltimateSkillManager;
    Vector3 targetPos;
    Actor actor;

    //float lastLaunchTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        actor = GetComponent<Actor>();
        titanUltimateSkillManager = GetComponent<TitanUltimateSkillManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Launch()
    {
        //lastLaunchTime = Time.time;
        titanUltimateSkillManager.isRunning = true;
        if (Physics.Raycast(aimCamera.transform.position,
                    aimCamera.transform.forward,
                    out RaycastHit hit,
                    maxRange,
                    actor.canHitLayerMask,
                    QueryTriggerInteraction.Ignore))
        {
            targetPos = hit.point;
        }
        else
        {
            targetPos = aimCamera.transform.position + aimCamera.transform.forward * maxRange;
        }

        for (int i = 0; i < missileCount; i++)
        {
            StartCoroutine(LaunchDelay(i * launchInterval, i));
        }
    }

    IEnumerator LaunchDelay(float delay, int index)
    {
        yield return new WaitForSeconds(delay);
        Transform muzzle = muzzles[Random.Range(0, muzzles.Length)];
        TitanBulletRocket curTitanBulletRocket = Instantiate(bulletRocketPrefab, muzzle);
        curTitanBulletRocket.transform.SetParent(null, true);
        curTitanBulletRocket.layerMask = actor.canHitLayerMask;
        curTitanBulletRocket.targetActor = missileLockSystem.curTarget;
        curTitanBulletRocket.owner = actor;
        curTitanBulletRocket.targetPos = targetPos;
        curTitanBulletRocket.Launch();
        if (index == missileCount - 1)
        {
            if (titanUltimateSkillManager.isRunning == true)
            {
                titanUltimateSkillManager.isRunning = false;
            }
        }
    }

    
}
