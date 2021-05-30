using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataUI : MonoBehaviour
{
    public Health health;
    public Image HPImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health)
        {
            if (HPImage)
            {
                HPImage.fillAmount = (float)health.hp / (float)health.maxHp;
            }
        }
    }
}
