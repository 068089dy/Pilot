using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetTag : MonoBehaviour
{
    Actor playerActor;
    GameFlowManager gameFlowManager;
    Actor curPlayerTitanActor;
    public GameObject targetListView;

    TagIconItem[] imagePool;
    Camera curCamera;

    TargetListItemPanel[] panels;
    // Start is called before the first frame update
    void Start()
    {
        gameFlowManager = FindObjectOfType<GameFlowManager>();
        playerActor = gameFlowManager.playerActor;
        // 初始化10个image
        Image[] images = GetComponentsInChildren<Image>();

        imagePool = new TagIconItem[images.Length];
        for (int i = 0; i < images.Length; i++)
        {
            imagePool[i] = new TagIconItem(images[i]);
        }
        // 显示列表
        panels = targetListView.GetComponentsInChildren<TargetListItemPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerActor.GetComponent<PlayerStateManager>().curState == PlayerState.PLAYER_CONTROL)
        {
            curCamera = playerActor.actorMainCamera;
        }
        else if (playerActor.GetComponent<PlayerStateManager>().curState == PlayerState.DRIVING)
        {
            curCamera = playerActor.GetComponent<PlayerStateManager>().lastTitanActor.actorMainCamera;
        }
        else
        {
            curCamera = null;
        }
        // 将imagePool全部置为无效
        
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].active = false;
        }
        for (int i = 0; i < imagePool.Length; i++)
        {
            if (imagePool[i].image.enabled)
            {
                imagePool[i].image.enabled = false;
            }
        }
        List<GuidanceMsg> targetCollection = GetTargetCollection();
        if (targetCollection.Count > 0 && targetCollection.Count <= imagePool.Length)
        {
            for (int i = 0; i < targetCollection.Count; i++)
            {
                if (targetCollection[i].target.gameObject.activeInHierarchy)
                {
                    imagePool[i].actor = targetCollection[i].target;
                    imagePool[i].image.enabled = true;
                    panels[i].title = "haha";
                    panels[i].note = targetCollection[i].lifeTime.ToString() + "s";
                    panels[i].active = true;
                    panels[i].hpImg.fillAmount = imagePool[i].actor.health.hp / imagePool[i].actor.health.maxHp;
                }
            }
        }
        // 将有效的img配对到位置上
        for (int i = 0; i < imagePool.Length; i++)
        {
            if (imagePool[i].image.enabled)
            {
                if (Vector3.Dot(imagePool[i].actor.transform.position - curCamera.transform.position, curCamera.transform.forward) > 0)
                {
                    imagePool[i].image.transform.position = curCamera.WorldToScreenPoint(imagePool[i].actor.transform.position);
                } else
                {
                    imagePool[i].image.enabled = false;
                }
                
            }
        }

        

    }

    List<GuidanceMsg> GetTargetCollection()
    {
        List<GuidanceMsg> res = new List<GuidanceMsg>();
        if (playerActor)
        {
            if (playerActor.GetComponent<PlayerStateManager>().lastTitanActor)
            {
                curPlayerTitanActor = playerActor.GetComponent<PlayerStateManager>().lastTitanActor;
                if (curPlayerTitanActor.GetComponent<MissileLockSystem>())
                {
                    if (curPlayerTitanActor.GetComponent<MissileLockSystem>().targetCollection.Count > 0)
                    {
                        res = curPlayerTitanActor.GetComponent<MissileLockSystem>().targetCollection;
                    }
                }
            }
        }
        return res;
    }
}

class TagIconItem
{
    public Image image;
    public Actor actor;
    public bool active;

    public TagIconItem(Image image)
    {
        this.image = image;
    }
    public TagIconItem(Image image, Actor actor)
    {
        this.image = image;
        this.actor = actor;
    }
}
