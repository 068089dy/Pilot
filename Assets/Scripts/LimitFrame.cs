using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFrame : MonoBehaviour
{
    public int limitFrameRate = 200;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = limitFrameRate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
