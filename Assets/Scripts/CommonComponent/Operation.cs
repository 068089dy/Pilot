using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Operation : MonoBehaviour
{
    //public InputHandler inputHandler;
    /*
     Player���²�����ʱ�����OperateRay��������Operation������壬�ͻ�Ѱ��Operation�����
     Ȼ�󣬽��������status��Ϊ1��
     ���磬����������ʱ
    */
    //public int status = 0;
    public UnityAction operate;
    
    // ��ʾ��Ϣ
    public string hint;
    // ��������:1,���¼����ã�2,����һ��ʱ���ŵ���
    [Range(1, 2)]
    public int type = 1;
    // ����Ϊ2ʱ����Ҫ���¶೤ʱ��ŵ���Opreate
    public float prepareDuration = 1;
    public float curPrepareTime;
    public Actor parentActor;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Operate(Actor actor)
    {
    }
}
