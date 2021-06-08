using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerState curState;
    // Start is called before the first frame update
    void Start()
    {
        curState = PlayerState.PLAYER_CONTROL;
    }

    // Update is called once per frame
    void Update()
    {
        //if (curState)
    }


}

public enum PlayerState
{
    IDLE,
    PLAYER_CONTROL,
    DRIVING
}
