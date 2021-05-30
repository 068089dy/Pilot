using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperationRay : MonoBehaviour
{
    public InputHandler inputHandler;
    public LayerMask operateLayerMask = -1;
    public float rayLength = 5;

    public Image loadingBar;
    public Text hintText;

    Operation lastHitOperation;


    bool canInput;
    // Start is called before the first frame update
    void Start()
    {
        hideOperateHint();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayLength, operateLayerMask, QueryTriggerInteraction.Ignore))
        {
            Operation operation = hit.transform.gameObject.GetComponent<Operation>();
            if (operation)
            {
                showOperateHint();
                hintText.text = operation.hint;
                //Debug.Log(operation.hint);
                if (operation.type == 1)
                {
                    if (inputHandler.GetOperateInputDown())
                    {
                        operation.Operate();
                    }
                } else if (operation.type == 2)
                {
                    if (inputHandler.GetOperateInputHeld() && canInput)
                    {
                        if (operation.curPrepareTime <= operation.prepareDuration)
                        {
                            operation.curPrepareTime += Time.deltaTime;
                            loadingBar.fillAmount = operation.curPrepareTime / operation.prepareDuration;
                        } else
                        {
                            operation.curPrepareTime = 0;
                            loadingBar.fillAmount = 0;
                            canInput = false;
                            operation.Operate();
                            
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
            } else
            {
                canInput = true;
                hideOperateHint();
            }
        } else
        {
            canInput = true;
            if (lastHitOperation)
            {
                hideOperateHint();
            }
        }
    }

    void showOperateHint()
    {
        loadingBar.enabled = true;
        hintText.enabled = true;
        
    }

    void hideOperateHint()
    {
        loadingBar.enabled = false;
        hintText.enabled = false;
        loadingBar.fillAmount = 0;
    }
}
