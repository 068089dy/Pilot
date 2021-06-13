using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorController : MonoBehaviour
{
    public List<Transform> patrolPoints = new List<Transform>();
    public int curPatrolTargetIndex;
    public Actor curAttackTarget;
    public List<Actor> targets;
    TeamManager teamManager;
    Actor actor;
    // Start is called before the first frame update
    void Start()
    {
        actor = GetComponent<Actor>();
        if (patrolPoints.Count < 2)
        {
            Debug.Log("Ñ²Âßµã¹ýÉÙ");
        }
        teamManager = FindObjectOfType<TeamManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (actor.team == Team.TEAM1)
        {
            targets = teamManager.team2Actors;
        } else
        {
            targets = teamManager.team1Actors;
        }
    }
}
