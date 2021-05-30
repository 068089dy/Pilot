using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTitanOperation : Operation
{
    // 当前相机
    public Camera curCamera;
    // 相机的目标位置
    public Transform cameraTargetTransform;
    // Player
    public GameObject player;
    public PlayerTitanController m_PlayerTitanController;
    public TitanAnimationController titanAnimationController;
    //public TitanStateManager titanStateManager;
    public TitanController titanController;

    // 驾驶舱盖子
    public GameObject cockPitCover;
    //public GameObject titanBody;

    public Camera lerpCamera;
    //public Camera titanCamera;
    public float lerpSpeed = 5;


    int curState;
    private void Start()
    {
        //curCamera = Camera.current;
        //Camera.current
        //m_PlayerTitanController.enabled = false;
        lerpCamera.gameObject.SetActive(false);
        curState = ControlTitanState.IDLE;
    }
    // Start is called before the first frame update
    void Update()
    {
        // 上机状态
        if (curState == ControlTitanState.ONING)
        {
            // 摄像机插值
            lerpCamera.transform.position = Vector3.Lerp(lerpCamera.transform.position, cameraTargetTransform.position, Time.deltaTime * lerpSpeed);
            //lerpCamera.transform.right = Vector3.Lerp(lerpCamera.transform.right, cameraTargetTransform.right, Time.deltaTime * lerpSpeed);
            lerpCamera.transform.forward = Vector3.Lerp(lerpCamera.transform.forward, cameraTargetTransform.forward, Time.deltaTime * lerpSpeed);

            if (Vector3.Distance(lerpCamera.transform.position, cameraTargetTransform.position) < 0.01f &&
                Vector3.Distance(lerpCamera.transform.forward, cameraTargetTransform.forward) < 0.01f)
            {
                m_PlayerTitanController.enabled = true;
                //lerpCamera.enabled = false;
                
            }
            // 起立
            titanAnimationController.Idle();
            // 关闭驾驶舱
            float coverXAngle = cockPitCover.transform.localEulerAngles.x;
            coverXAngle = Mathf.Lerp(coverXAngle, -6f, Time.deltaTime * 2);
            cockPitCover.transform.localEulerAngles = new Vector3(coverXAngle, 0, 0);
            //Debug.Log(cockPitCover.transform.localEulerAngles);
            // 关闭完成后初始化
            if (coverXAngle <= 0.1f)
            {
                curState = ControlTitanState.INITING;
                cockPitCover.transform.localEulerAngles = new Vector3(-6, 0, 0);
            }
        } else if (curState == ControlTitanState.INITING)
        {
            // 初始化
            // 交由铁驭控制
            titanController.setPlayerControlMode();
            //titanStateManager.setPlayerControlMode();
            
            // 隐藏驾驶舱盖子和body
            //cockPitCover.GetComponent<Renderer>().enabled = false;
            //titanBody.GetComponent<Renderer>().enabled = false;

            // 初始化完成后恢复状态
            curState = ControlTitanState.IDLE;
            lerpCamera.gameObject.SetActive(false);

        } else if (curState == ControlTitanState.IDLE)
        {
            //lerpCamera.gameObject.SetActive(false);
        }
    }

    public override void Operate()
    {
        /*
         将player置为无效，并且新建一个相机，将相机从当前县及位置插值到目标位置，然后激活Titan控制
         */
        //lerpCamera.enabled = true;
        lerpCamera.gameObject.SetActive(true);
        //titanCamera.gameObject.SetActive(false);
        //Camera.SetupCurrent(lerpCamera);
        lerpCamera.transform.position = curCamera.transform.position;
        lerpCamera.transform.forward = curCamera.transform.forward;
        lerpCamera.fieldOfView = curCamera.fieldOfView;
        //lerping = true;
        curState = ControlTitanState.ONING;
        player.GetComponent<PlayerCharacterController>().initPlayer();
        player.SetActive(false);
        // 打开驾驶舱盖子
        cockPitCover.transform.localEulerAngles = new Vector3(90, 0, 0);
    }
}

class ControlTitanState
{
    // 静默
    public static int IDLE = 0;
    // 正在上机
    public static int ONING = 1;
    // 起立
    public static int STANDING = 2;
    public static int INITING = 3;
}
