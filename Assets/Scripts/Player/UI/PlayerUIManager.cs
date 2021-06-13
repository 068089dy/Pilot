using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    // 血量
    public Image HPImg;
    public Health health;
    // 准星
    //public Image ReticleImg;

    // 命中/击杀反馈
    //public Image HitFeedbackImg;
    public HitHint hitHint;
    public HitHint killedHint;


    // 收到伤害提示
    public Damagable damagable;
    public HitHint beHurtHint;
    public Actor actor;
    //public DamageCounter damageCounter;

    public AudioSource m_AudioSource;
    public AudioClip killedSFX;
    public PlayerStateManager playerStateManager;

    public GameObject UIRoot;
    PlayerState lastFrameState;

    // Start is called before the first frame update
    void Start()
    {
        actor.damageCounter.DamageAction += ShowHitFeedback;
        actor.damageCounter.KilledAction += ShowKilledFeedback;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStateManager.curState == PlayerState.PLAYER_CONTROL) {
            if (lastFrameState != playerStateManager.curState)
            {
                UIRoot.SetActive(true);
            }
            // 血量显示
            HPImg.fillAmount = health.hp / health.maxHp;
            ShowShooterPos();
        } else
        {
            if (lastFrameState != playerStateManager.curState)
            {
                UIRoot.SetActive(false);
            }
        }
        lastFrameState = playerStateManager.curState;
    }

    // 命中反馈
    void ShowHitFeedback(DamageMsg damageMsg)
    {
        if (hitHint)
        {
            hitHint.Show();
        }
    }

    // 击杀反馈
    void ShowKilledFeedback(DamageMsg damageMsg)
    {
        if (killedHint)
        {
            killedHint.Show(damageMsg);
            m_AudioSource.PlayOneShot(killedSFX);
        }
    }

    // 攻击提示
    void ShowShooterPos()
    {
        if (actor)
        {
            if (Time.time < actor.lastBeHurtTime + 0.5f)
            {
                if (actor.lastDamageMsg != null && actor.lastDamageMsg.shooter)
                {
                    float angle = Vector3.Angle(actor.transform.forward, actor.lastDamageMsg.shooter.transform.position - actor.transform.position);
                    if (Vector3.Dot(actor.transform.right, actor.lastDamageMsg.shooter.transform.position - actor.transform.position) > 0)
                    {
                        beHurtHint.transform.localEulerAngles = -Vector3.forward * angle;
                    } else
                    {
                        beHurtHint.transform.localEulerAngles = Vector3.forward * angle;
                    }
                    
                    beHurtHint.Show();
                }
                // 计算actor和攻击者的水平夹角
                
            }
        }
    }
}
