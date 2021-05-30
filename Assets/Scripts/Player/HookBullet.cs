using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBullet : MonoBehaviour
{
    public float speed = 60f;
    public LayerMask hookLayerMask = -1;
    [System.NonSerialized]
    public Vector3 direction;
    //public GameObject HookObject;
    [System.NonSerialized]
    public Hook m_Hook;
    [System.NonSerialized]
    public bool isSend = false;

    AudioSource m_AudioSource;
    public AudioClip hookSFX;
    public AudioClip ropeSFX;

    Vector3 lastFramePos;
    // Start is called before the first frame update
    void Start()
    {
        //m_Hook = HookObject.GetComponent<Hook>();
        transform.Rotate(new Vector3(90, 0, 0));
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        lastFramePos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isSend && m_Hook && !m_Hook.hooking) {
            lastFramePos = transform.position;
            transform.Translate(direction * Time.deltaTime * speed, Space.World);
            Vector3 sendVector = transform.position - lastFramePos;
            Ray ray = new Ray(lastFramePos, sendVector);
            
            if (Physics.Raycast(ray, out RaycastHit hit, sendVector.magnitude, hookLayerMask))
            {
                m_AudioSource.PlayOneShot(hookSFX);
                StartCoroutine(playRopePull(0.1f));
                transform.position = hit.point;
                transform.forward = hit.normal;
                transform.SetParent(hit.transform, true);
                //Debug.Log("hook collider.");
                m_Hook.hooking = true;
                isSend = false;
                m_Hook.lastHookTime = Time.time;

                //m_Hook.thisHookLength = transform.position - 

            }
            if (Vector3.Distance(m_Hook.transform.position,transform.position) > m_Hook.hookLength)
            {
                m_Hook.hooking = false;
                isSend = false;
                m_Hook.RecyclingHook();
            }
        }
        
    }

    IEnumerator playRopePull(float t)
    {
        yield return new WaitForSeconds(t);//运行到这，暂停t秒

        //t秒后，继续运行下面代码
        m_AudioSource.PlayOneShot(ropeSFX);
    }
}
