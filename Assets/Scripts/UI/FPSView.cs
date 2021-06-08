using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSView : MonoBehaviour
{
    public float updateInterval = 0.5f;
    private float lastInterval;
    private int frames = 0;
    private float fps;
    Text fpsText;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 100;
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
        fpsText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow >= lastInterval + updateInterval)
        {
            fps = frames / (timeNow - lastInterval);
            frames = 0;
            lastInterval = timeNow;
            fpsText.text = "FPS:" + fps.ToString();
        }
    }
}
