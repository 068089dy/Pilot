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
    public LayerMask layerMask;
    public TitanBulletRocket bulletRocketPrefab;
    public Transform muzzle;
    Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inputHandler.GetMissileBatchInputDown())
        {
            
            if (Physics.Raycast(aimCamera.transform.position,
                    aimCamera.transform.forward,
                    out RaycastHit hit,
                    maxRange,
                    layerMask,
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
                StartCoroutine(LaunchDelay(i * launchInterval + Random.Range(-0.1f, 0.1f)));
            }
        }
    }

    IEnumerator LaunchDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        TitanBulletRocket curTitanBulletRocket = Instantiate(bulletRocketPrefab, muzzle);
        curTitanBulletRocket.transform.SetParent(null, true);
        curTitanBulletRocket.transform.Rotate(Vector3.forward * Random.Range(-90, 90));
        //curTitanBulletRocket.transform.
        curTitanBulletRocket.Launch();
        curTitanBulletRocket.layerMask = layerMask;
        curTitanBulletRocket.targetPos = targetPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(targetPos, 2f);
    }
}
