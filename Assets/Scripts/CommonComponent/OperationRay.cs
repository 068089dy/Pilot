using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OperationRay : MonoBehaviour
{
    public InputHandler inputHandler;
    public LayerMask operateLayerMask = -1;
    public float rayLength = 5;

    public Image loadingBar;
    //public Text hintText;
    public TMP_Text hintTextMesh;
    public Actor actor;
    Operation lastHitOperation;

    public PlayerStateManager playerStateManager;

    bool canInput;
    // Start is called before the first frame update
    void Start()
    {
        hideOperateHint();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStateManager.curState == PlayerState.PLAYER_CONTROL)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayLength, operateLayerMask, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log(Time.time + "opreation");
                Operation operation = hit.transform.gameObject.GetComponent<Operation>();
                if (operation &&
                    (operation.parentActor.team == Team.NOTEAM ||
                    operation.parentActor.team == actor.team)
                    )
                {
                    showOperateHint();
                    //hintText.text = operation.hint;
                    hintTextMesh.text = operation.hint;
                    // 点按操作
                    if (operation.type == 1)
                    {
                        if (inputHandler.GetOperateInputDown())
                        {
                            operation.Operate(actor);
                            playerStateManager.curState = PlayerState.DRIVING;
                            hideOperateHint();
                        }
                    }
                    else if (operation.type == 2)
                    {
                        // 长按操作
                        if (inputHandler.GetOperateInputHeld() && canInput)
                        {
                            if (operation.curPrepareTime <= operation.prepareDuration)
                            {
                                operation.curPrepareTime += Time.deltaTime;
                                loadingBar.fillAmount = operation.curPrepareTime / operation.prepareDuration;
                            }
                            else
                            {
                                operation.curPrepareTime = 0;
                                loadingBar.fillAmount = 0;
                                canInput = false;
                                operation.Operate(actor);

                            }
                        }
                        if (inputHandler.GetOperateInputRelease())
                        {
                            canInput = true;
                            operation.curPrepareTime = 0;
                            loadingBar.fillAmount = 0;
                        }
                    }
                    lastHitOperation = operation;
                }
                else
                {
                    canInput = true;
                    hideOperateHint();
                }
            }
            else
            {
                canInput = true;
                if (lastHitOperation)
                {
                    hideOperateHint();
                }
            }
        }
    }

    void showOperateHint()
    {
        loadingBar.enabled = true;
        //hintText.enabled = true;
        hintTextMesh.enabled = true;
    }

    void hideOperateHint()
    {
        loadingBar.enabled = false;
        //hintText.enabled = false;
        hintTextMesh.enabled = false;
        loadingBar.fillAmount = 0;
    }
}
