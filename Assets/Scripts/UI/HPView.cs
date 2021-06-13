using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPView : MonoBehaviour
{
    public Actor actor;
    public Image hpImg;
    Camera curCamara;
    GameFlowManager gameFlowManager;
    Actor playerActor;
    // Start is called before the first frame update
    void Start()
    {
        gameFlowManager = FindObjectOfType<GameFlowManager>();
        playerActor = gameFlowManager.playerActor;
    }

    // Update is called once per frame
    void Update()
    {
        // 如果actor是player正在驾驶titan，隐藏血条
        if (playerActor.GetComponent<PlayerStateManager>().lastTitanActor == actor &&
            playerActor.GetComponent<PlayerStateManager>().curState == PlayerState.DRIVING)
        {
            gameObject.GetComponent<Image>().enabled = false;
            hpImg.enabled = false;
            return;
        } else
        {
            gameObject.GetComponent<Image>().enabled = true;
            hpImg.enabled = true;
        }
        // 按照玩家的状态获取当前的相机
        if (playerActor.GetComponent<PlayerStateManager>().curState == PlayerState.PLAYER_CONTROL)
        {
            curCamara = playerActor.actorMainCamera;
        } else if (playerActor.GetComponent<PlayerStateManager>().curState == PlayerState.DRIVING)
        {
            curCamara = playerActor.GetComponent<PlayerStateManager>().lastTitanActor.actorMainCamera;
        } else
        {
            curCamara = null;
        }
        if (curCamara)
        {
            Vector3 lookDir = curCamara.transform.position;
            lookDir.y = transform.position.y;
            transform.LookAt(lookDir);
            hpImg.fillAmount = actor.health.hp / actor.health.maxHp;
        }
    }
}
