using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    // Ѫ��
    public Image HPImg;
    public Health health;
    // ׼��
    //public Image ReticleImg;

    // ����/��ɱ����
    //public Image HitFeedbackImg;
    public HitHint hitHint;
    public HitHint killedHint;


    // �յ��˺���ʾ
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
            // Ѫ����ʾ
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

    // ���з���
    void ShowHitFeedback(DamageMsg damageMsg)
    {
        if (hitHint)
        {
            hitHint.Show();
        }
    }

    // ��ɱ����
    void ShowKilledFeedback(DamageMsg damageMsg)
    {
        if (killedHint)
        {
            killedHint.Show(damageMsg);
            m_AudioSource.PlayOneShot(killedSFX);
        }
    }

    // ������ʾ
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
                // ����actor�͹����ߵ�ˮƽ�н�
                
            }
        }
    }
}
