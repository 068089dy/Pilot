using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public CharacterType characterType;
    public Team team;
    // ¿É¹¥»÷µÄ²ã
    public LayerMask hitLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum CharacterType
{
    ROBOT,
    PLAYER,
    TITAN,
    BATTERY
}

public enum Team
{
    TEAM1,
    TEAM2
}
