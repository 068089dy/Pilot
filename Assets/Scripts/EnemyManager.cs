using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //public GameObject robotRoot;
    //public GameObject titanRoot;
    /*
    共有几波敌人
    每一波敌人的数量和类型
    每一波敌人的出生点中心
    */
    //public int batchNums = 3;
    public GenerateState curGenerateState;
    public List<EnemyBatchMsg> branchs;

    public GameObject robotPrefab;
    public GameObject titanPrefab;
    TeamManager teamManager;
    // Start is called before the first frame update
    void Start()
    {
        curGenerateState = new GenerateState(0, 0);
        teamManager = FindObjectOfType<TeamManager>();
        //FindObjectOfType<PlayerCharacterController>()
    }

    // Update is called once per frame
    void Update()
    {
        if (branchs.Count > 0) {
            for (int i = 0; i < branchs.Count; i++)
            {
                EnemyBatchMsg branch = branchs[i];
                if (!branch.active)
                {
                    // 距离触发
                    if (branch.triggerType == EnemyTriggerType.RANGE)
                    {
                        // 判断team1成员到centerPoint的距离是否小于triggerRange
                        if (CheckTeam1InRange(branch.root.transform.position, branch.triggerRange))
                        {
                            branch.root.SetActive(true);
                            branch.active = true;
                        }
                    } else if(branch.triggerType == EnemyTriggerType.INHERIT) {
                        bool lastBranchEnemyWipeOut = true;
                        if (i > 0)
                        {
                            // 如果上一波敌人已经激活
                            if (branchs[i - 1].active)
                            {
                                Actor[] actors = branchs[i - 1].root.GetComponentsInChildren<Actor>();
                                if (actors.Length > 0)
                                {
                                    //Debug.Log("第" + i + "have human.");
                                    foreach (Actor a in actors)
                                    {
                                        // 如果发现上一批还有活着的敌人
                                        //Debug.Log("actor列表" + a);
                                        //Debug.Log("actor health" + a.health);
                                        if (a && a.health.hp > 0)
                                        {
                                            lastBranchEnemyWipeOut = false;
                                        }
                                        //lastBranchEnemyWipeOut = false;
                                    }
                                }
                            } else
                            {
                                lastBranchEnemyWipeOut = false;
                            }
                        }
                        if (lastBranchEnemyWipeOut)
                        {
                            branch.root.SetActive(true);
                            branch.active = true;
                        }
                    }
                }
                
                
            }
        }
    }

    bool CheckTeam1InRange(Vector3 center, float range)
    {
        List<Actor> team1mates = teamManager.team1Actors;
        foreach (Actor tm in team1mates)
        {
            if (tm && Vector3.Distance(tm.transform.position, center) <= range)
            {
                return true;
            }
        }
        return false;
    }

    public class GenerateState
    {
        public int brachNum = 0;
        // 0:未生成，1：已生成
        public int status = 0;

        public GenerateState(int brachNum, int status)
        {
            this.brachNum = brachNum;
            this.status = status;
        }
    }

    [Serializable]
    public class EnemyBatchMsg
    {
        //public EnemyMsg[] enemyMsgs;
        public GameObject root;
        public EnemyTriggerType triggerType = EnemyTriggerType.RANGE;
        // 触发范围（距离中心点多近时生成敌人）
        public float triggerRange = 100;
        // 是否激活
        public bool active;
    }

    public enum EnemyTriggerType
    {
        RANGE,//距离root一定距离时触发
        INHERIT, //继承消灭上一批次敌人后触发
        RANGE_AND_INHERIT,
    }

    //[Serializable]
    //public class EnemyMsg
    //{
    //    public CharacterType characterType = CharacterType.ROBOT;
    //    public int enemyCount = 10;
    //    public Transform[] birthPoints;
    //}
}


