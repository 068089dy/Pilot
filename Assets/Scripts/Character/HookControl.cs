using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CharacterControl))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AnimationControl))]
public class HookControl : MonoBehaviour
{
    public Camera camera;
    public Transform hookStartPoint;
    public Transform leftHandIkTarget;
    public float hookLength = 20f;
    public float hookSpeed = 15f;

    int flootLayerMask = 1 << 9;

    float m_MaxHookTimeLimit;



    LineRenderer lineRenderer;
    Vector3 hookTarget;
    CharacterController m_Controller;
    CharacterControl m_Control;
    AnimationControl animationControl;
    AudioControl audioControl;
    bool isHitFloor = false;

    float m_LastTimeHooked = 0f;
    const float k_HookGroundingPreventionTime = 0.2f;
    int curState;

    enum State
    {
        Idle,
        Hooking
    }

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        m_Controller = GetComponent<CharacterController>();
        m_Control = GetComponent<CharacterControl>();
        animationControl = GetComponent<AnimationControl>();
        audioControl = GetComponent<AudioControl>();
        curState = (int)State.Idle;

        m_MaxHookTimeLimit = hookLength / hookSpeed;
        //if (Physics.CapsuleCast(new Vector3(0, 0, 1), new Vector3(0, 1, 1), 0.4f, Vector3.down, out RaycastHit hit, 0.11f, -1, QueryTriggerInteraction.Ignore))
        //{
        //    Debug.Log(hit.point + hit.normal);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        animationControl.closeLeftHandIk();
        if (curState == (int)State.Idle)
        {
            lineRenderer.enabled = false;
            //m_Control.curState = (int)CharacterControl.State.Move;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, hookLength))
                {
                    hookTarget = hit.point;
                    curState = (int)State.Hooking;
                    m_LastTimeHooked = Time.time;
                    if (audioControl != null)
                    {
                        audioControl.Hooking();
                    }
                    // 计算本次最大钩锁时长；
                    m_MaxHookTimeLimit = (transform.position - hookTarget).magnitude / hookSpeed;
                    //leftHandIkTarget.position = hit.point;

                    //m_Controller.Move((hookTarget - hookStartPoint.position).normalized * 10 * Time.deltaTime);
                }
            }
        } else if (curState == (int)State.Hooking)
        {
            if (// 碰到墙面
                CheckHitWall() ||
                // 碰到地面并且距离目标小于阈值
                (CheckOnGround() && (transform.position-hookTarget).magnitude < 0.2f) ||
                // 角色速度小于3并且不是刚起步（刚起步速度为0，不能做这个判断）
                (m_Controller.velocity.magnitude < 3 && Time.time > m_LastTimeHooked + k_HookGroundingPreventionTime) ||
                // 超出最大钩锁时长
                (Time.time > m_MaxHookTimeLimit + m_LastTimeHooked))
            {
                stopHook();
            } else
            {
                
                // 进行钩锁
                m_Control.curState = (int)CharacterControl.State.Hook;
                Vector3 t_Speed = m_Controller.velocity * 2 + (hookTarget - transform.position).normalized * hookSpeed;
                t_Speed = t_Speed.normalized * hookSpeed;
                m_Controller.Move(t_Speed * Time.deltaTime);
                animationControl.leftHandToPos(hookTarget);
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, hookStartPoint.position);
                lineRenderer.SetPosition(1, hookTarget);
                // 中途结束
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
                {
                    stopHook();
                }
            }
        }
    }

    void stopHook()
    {
        // 结束钩锁
        curState = (int)State.Idle;
        m_Control.curState = (int)CharacterControl.State.Move;
        m_Control.v_Velocity = m_Controller.velocity.y;
        Debug.Log("Stop hook"+m_Controller.velocity.y);
        //if (m_Controller.velocity.y > 0)
        //{
        //    m_Controller.Move(m_Controller.velocity * Time.deltaTime);
        //}
    }

    Vector3 GetCapsuleBottomHemisphere()
    {
        return transform.position + (transform.up * m_Controller.radius);
    }

    Vector3 GetCapsuleTopHemisphere(float atHeight)
    {
        return transform.position + (transform.up * (atHeight - m_Controller.radius));
    }

    bool CheckHitObject()
    {
        if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(m_Controller.height), m_Controller.radius + 0.01f, Vector3.down, out RaycastHit hit, 0.1f, -1, QueryTriggerInteraction.Ignore))
        {
            return true;
        }
        return false;
    }

    // 检测是否碰撞到了墙壁或房顶
    bool CheckHitWall()
    {
        if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(m_Controller.height), m_Controller.radius+0.01f, Vector3.down, out RaycastHit hit, 0.1f, flootLayerMask, QueryTriggerInteraction.Ignore))
        {
            if (Vector3.Dot(hit.normal, Vector3.up) > 0.5)
            {
                isHitFloor = false;
            }
            else
            {
                isHitFloor = true;
            }
            //Debug.Log("碰撞法线" + hit.normal + Vector3.up + hit.point + transform.position);
        }
        else
        {
            isHitFloor = false;
        }
        return isHitFloor;
    }

    bool CheckOnGround()
    {
        if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(m_Controller.height), m_Controller.radius, Vector3.down, out RaycastHit hit, 0.1f, flootLayerMask, QueryTriggerInteraction.Ignore))
        {

            if (Vector3.Dot(hit.normal, Vector3.up) > 0.5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
