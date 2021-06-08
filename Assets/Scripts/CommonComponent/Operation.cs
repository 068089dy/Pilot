using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Operation : MonoBehaviour
{
    //public InputHandler inputHandler;
    /*
     Player按下操作键时，如果OperateRay触碰到了Operation层的物体，就会寻找Operation组件，
     然后，将该组件的status置为1。
     比如，操作机器人时
    */
    //public int status = 0;
    public UnityAction operate;
    
    // 提示信息
    public string hint;
    // 操作类型:1,按下即调用；2,按下一定时长才调用
    [Range(1, 2)]
    public int type = 1;
    // 类型为2时，需要按下多长时间才调用Opreate
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
