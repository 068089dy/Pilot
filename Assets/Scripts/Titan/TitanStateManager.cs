using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class TitanStateManager : MonoBehaviour
{
    public int curState;
    public Camera titanCamera;
    // Start is called before the first frame update
    void Start()
    {
        curState = TitanState.LANDING;
    }

    public void setPlayerControlMode()
    {
        curState = TitanState.PLAYER_CONTROL;
        Camera.SetupCurrent(titanCamera);
        titanCamera.gameObject.SetActive(true);
        //titanCamera.enabled = true;
    }
}

public class TitanState
{
    public static int PLAYER_CONTROL = 0;
    public static int LANDING = 1;
    public static int AUTO_CONTROL = 2;
    public static int EXITING = 3;
}
