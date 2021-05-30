using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public Image HPImg;
    public Image ReticleImg;
    public Image HitFeedbackImg;
    public Health health;
    public HitHint hitHint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HPImg.fillAmount = health.hp / health.maxHp;
    }

    public void ShowHitFeedback()
    {
        if (hitHint)
        {
            hitHint.Show();
        }
    }
}
