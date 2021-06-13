using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMechine
{
    public State curState;
    public StateMechine()
    {

    }
    // Start is called before the first frame update
    public State getState()
    {
        return curState;
    }

    public void setState(State state)
    {
        if (curState != state)
        {
            curState.Exit();
            curState = state;
            state.Start();
        }
        state.doAction();
    }
    
}
