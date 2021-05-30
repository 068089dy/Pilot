using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    //PlayerCharacterController m_PlayerCharacterController;
    public Health playerHealth;
    public GameObject diedText;
    // Start is called before the first frame update
    void Start()
    {
        diedText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        if (diedText != null)
        {
            diedText.SetActive(true);
        }
    }

    
}
