using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public List<Actor> team1Actors;
    public List<Actor> team2Actors;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterActor(Actor actor)
    {
        if (actor.team == Team.TEAM1)
        {
            team1Actors.Add(actor);
        } else if (actor.team == Team.TEAM2)
        {
            team2Actors.Add(actor);
        }
    }

    public void UnRegisterActor(Actor actor)
    {
        if (actor.team == Team.TEAM1)
        {
            team1Actors.Remove(actor);
        }
        else if (actor.team == Team.TEAM2)
        {
            team2Actors.Remove(actor);
        }
    }
}
