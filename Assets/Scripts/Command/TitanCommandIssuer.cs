using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanCommandIssuer : MonoBehaviour
{
    Actor actor;
    public GameObject CommandPanel;
    // Start is called before the first frame update
    void Start()
    {
        actor = GetComponent<Actor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            CommandPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
        if (Input.GetKeyUp(KeyCode.C)){
            CommandPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void LaunchMissileBarch()
    {
        if (actor.characterType == CharacterType.PLAYER)
        {
            if (actor.GetComponent<PlayerStateManager>().lastTitanActor)
            {
                Actor lastTitanActor = actor.GetComponent<PlayerStateManager>().lastTitanActor;
                if (lastTitanActor.GetComponent<TitanCommandExecer>())
                {
                    lastTitanActor.GetComponent<TitanCommandExecer>().LaunchMissileBatch();
                }
            }
        }
    }
}
