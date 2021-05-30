using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTitanOperation : Operation
{
    // ��ǰ���
    public Camera curCamera;
    // �����Ŀ��λ��
    public Transform cameraTargetTransform;
    // Player
    public GameObject player;
    public PlayerTitanController m_PlayerTitanController;
    public TitanAnimationController titanAnimationController;
    //public TitanStateManager titanStateManager;
    public TitanController titanController;

    // ��ʻ�ո���
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
        // �ϻ�״̬
        if (curState == ControlTitanState.ONING)
        {
            // �������ֵ
            lerpCamera.transform.position = Vector3.Lerp(lerpCamera.transform.position, cameraTargetTransform.position, Time.deltaTime * lerpSpeed);
            //lerpCamera.transform.right = Vector3.Lerp(lerpCamera.transform.right, cameraTargetTransform.right, Time.deltaTime * lerpSpeed);
            lerpCamera.transform.forward = Vector3.Lerp(lerpCamera.transform.forward, cameraTargetTransform.forward, Time.deltaTime * lerpSpeed);

            if (Vector3.Distance(lerpCamera.transform.position, cameraTargetTransform.position) < 0.01f &&
                Vector3.Distance(lerpCamera.transform.forward, cameraTargetTransform.forward) < 0.01f)
            {
                m_PlayerTitanController.enabled = true;
                //lerpCamera.enabled = false;
                
            }
            // ����
            titanAnimationController.Idle();
            // �رռ�ʻ��
            float coverXAngle = cockPitCover.transform.localEulerAngles.x;
            coverXAngle = Mathf.Lerp(coverXAngle, -6f, Time.deltaTime * 2);
            cockPitCover.transform.localEulerAngles = new Vector3(coverXAngle, 0, 0);
            //Debug.Log(cockPitCover.transform.localEulerAngles);
            // �ر���ɺ��ʼ��
            if (coverXAngle <= 0.1f)
            {
                curState = ControlTitanState.INITING;
                cockPitCover.transform.localEulerAngles = new Vector3(-6, 0, 0);
            }
        } else if (curState == ControlTitanState.INITING)
        {
            // ��ʼ��
            // ������Ԧ����
            titanController.setPlayerControlMode();
            //titanStateManager.setPlayerControlMode();
            
            // ���ؼ�ʻ�ո��Ӻ�body
            //cockPitCover.GetComponent<Renderer>().enabled = false;
            //titanBody.GetComponent<Renderer>().enabled = false;

            // ��ʼ����ɺ�ָ�״̬
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
         ��player��Ϊ��Ч�������½�һ�������������ӵ�ǰ�ؼ�λ�ò�ֵ��Ŀ��λ�ã�Ȼ�󼤻�Titan����
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
        // �򿪼�ʻ�ո���
        cockPitCover.transform.localEulerAngles = new Vector3(90, 0, 0);
    }
}

class ControlTitanState
{
    // ��Ĭ
    public static int IDLE = 0;
    // �����ϻ�
    public static int ONING = 1;
    // ����
    public static int STANDING = 2;
    public static int INITING = 3;
}
