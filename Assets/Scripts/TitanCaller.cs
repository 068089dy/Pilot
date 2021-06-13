using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanCaller : MonoBehaviour
{
    public GameObject titan;
    GameFlowManager gameFlowManager;
    UIManager uIManager;
    bool isCall;
    bool showedToast;
    // Start is called before the first frame update
    void Start()
    {
        titan.SetActive(false);
        gameFlowManager = FindObjectOfType<GameFlowManager>();
        uIManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCall)
        {
            if (gameFlowManager.playerActor.damageCounter.killedAmount > 3)
            {
                if (!showedToast)
                {
                    uIManager.showBigToast("坦克就绪", 10);
                    uIManager.showKeyToast("v", "按下V呼叫", 10);
                    showedToast = true;
                }
                if (Input.GetKeyDown(KeyCode.V))
                {
                    titan.SetActive(true);
                    isCall = true;
                    
                }
            }
        }
        
        
    }
}
