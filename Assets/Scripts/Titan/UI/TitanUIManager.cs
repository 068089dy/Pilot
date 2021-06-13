using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitanUIManager : MonoBehaviour
{
    public TitanStateManager titanStateManager;
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
    public DamageCounter damageCounter;

    public AudioSource m_AudioSource;
    public AudioClip killedSFX;

    public TitanUltimateSkillManager titanUltimateSkillManager;
    public Image EnergeImg;
    //public 
    //public 
    // Start is called before the first frame update
    void Start()
    {
        damageCounter.DamageAction += ShowHitFeedback;
        damageCounter.KilledAction += ShowKilledFeedback;
        hideUI();
    }

    // Update is called once per frame
    void Update()
    {

        // Ѫ����ʾ
        HPImg.fillAmount = health.hp / health.maxHp;
        // ������ʾ
        EnergeImg.fillAmount = titanUltimateSkillManager.curEnerge / titanUltimateSkillManager.maxEnerge;
        if (titanUltimateSkillManager.curEnerge >= titanUltimateSkillManager.maxEnerge)
        {
            EnergeImg.color = new Color(EnergeImg.color.r, EnergeImg.color.g, EnergeImg.color.b, Mathf.Sin(10 * Time.time));
        } else
        {
            EnergeImg.color = new Color(EnergeImg.color.r, EnergeImg.color.g, EnergeImg.color.b, 1);
        }
        ShowShooterPos();
        if (titanStateManager.curState == TitanState.PLAYER_CONTROL)
        {
            showUI();
        } else
        {
            hideUI();
        }
    }

    void hideUI()
    {
        HPImg.enabled = false;
        EnergeImg.enabled = false;
        hitHint.GetComponent<Image>().enabled = false;
        killedHint.GetComponent<Image>().enabled = false;
        beHurtHint.GetComponent<Image>().enabled = false;
    }

    void showUI()
    {
        HPImg.enabled = true;
        EnergeImg.enabled = true;
        hitHint.GetComponent<Image>().enabled = true;
        killedHint.GetComponent<Image>().enabled = true;
        beHurtHint.GetComponent<Image>().enabled = true;
    }

    // ���з���
    void ShowHitFeedback(DamageMsg damageMsg)
    {
        if (titanStateManager.curState == TitanState.PLAYER_CONTROL)
        {
            if (hitHint)
            {
                hitHint.Show();
            }
        }
    }

    // ��ɱ����
    void ShowKilledFeedback(DamageMsg damageMsg)
    {
        if (titanStateManager.curState == TitanState.PLAYER_CONTROL)
        {
            if (killedHint)
            {
                killedHint.Show(damageMsg);
                m_AudioSource.PlayOneShot(killedSFX);
            }
        }
    }

    // ������ʾ
    void ShowShooterPos()
    {
        if (titanStateManager.curState == TitanState.PLAYER_CONTROL)
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
                        }
                        else
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
}
