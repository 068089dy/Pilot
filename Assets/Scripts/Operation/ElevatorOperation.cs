using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorOperation : Operation
{
    public MovePlatform movePlatform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Operate(Actor actor)
    {
        movePlatform.Launch();
    }
}
