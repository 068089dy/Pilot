using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public ShipState curState;
    public Health health;

    // 起飞
    float startTakeOffTime;
    float takeOffDuration = 5;
    float takeOffHeight = 50;
    float startHeight;

    // 点火
    float startIgniteTime;
    float ignitionDuration = 10;
    public ParticleSystem[] boosters;
    public GameObject explosionFXPrefab;

    // 飞行
    float speed = 500;

    // 摧毁
    //public BigExplosion destroyFX;
    float startExplosionTime;
    public float explosionDuration = 4;

    // 上一帧血量
    float lastFrameHP;
    // Start is called before the first frame update
    void Start()
    {
        curState = ShipState.IDLE;
        startHeight = transform.position.y;
        foreach (ParticleSystem p in boosters)
        {
            p.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lastFrameHP > 0 && health.hp <= 0)
        {
            curState = ShipState.DESTROY;
        }

        if (curState == ShipState.IDLE)
        {
            if (health.hp < 1000)
            {
                curState = ShipState.TAKE_OFF;
                startTakeOffTime = Time.time;
            }
        } else if (curState == ShipState.TAKE_OFF)
        {
            if (transform.position.y < startHeight + takeOffHeight)
            {
                transform.Translate(Vector3.up * Time.deltaTime * takeOffHeight / takeOffDuration, Space.World);
            } else
            {
                startIgniteTime = Time.time;
                curState = ShipState.IGNITING;
            }

        } else if (curState == ShipState.IGNITING)
        {
            if (Time.time < startIgniteTime + ignitionDuration)
            {
                // 尾焰
                foreach (ParticleSystem p in boosters)
                {
                    p.Play();
                }
            } else
            {
                curState = ShipState.FLY;
            }
        } else if (curState == ShipState.FLY)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);
        } else if (curState == ShipState.DESTROY)
        {
            //startExplosionTime = Time.time;
            //destroyFX.Explosion();
            GameObject go = GameObject.Instantiate(explosionFXPrefab, transform.position, Quaternion.identity);
            Destroy(go, 10);
            Destroy(gameObject, 2);
            curState = ShipState.DIED;
        } else if (curState == ShipState.DIED)
        {
            //if (Time.time > startExplosionTime + explosionDuration)
            //{
            //    gameObject.SetActive(false);
            //}
        }
        lastFrameHP = health.hp;
    }
}

public enum ShipState
{
    IDLE,
    TAKE_OFF,
    IGNITING,
    FLY,
    DESTROY,
    DIED
}
