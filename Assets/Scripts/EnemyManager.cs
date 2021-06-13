using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //public GameObject robotRoot;
    //public GameObject titanRoot;
    /*
    ���м�������
    ÿһ�����˵�����������
    ÿһ�����˵ĳ���������
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
                    // ���봥��
                    if (branch.triggerType == EnemyTriggerType.RANGE)
                    {
                        // �ж�team1��Ա��centerPoint�ľ����Ƿ�С��triggerRange
                        if (CheckTeam1InRange(branch.root.transform.position, branch.triggerRange))
                        {
                            branch.root.SetActive(true);
                            branch.active = true;
                        }
                    } else if(branch.triggerType == EnemyTriggerType.INHERIT) {
                        bool lastBranchEnemyWipeOut = true;
                        if (i > 0)
                        {
                            // �����һ�������Ѿ�����
                            if (branchs[i - 1].active)
                            {
                                Actor[] actors = branchs[i - 1].root.GetComponentsInChildren<Actor>();
                                if (actors.Length > 0)
                                {
                                    //Debug.Log("��" + i + "have human.");
                                    foreach (Actor a in actors)
                                    {
                                        // ���������һ�����л��ŵĵ���
                                        //Debug.Log("actor�б�" + a);
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
        // 0:δ���ɣ�1��������
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
        // ������Χ���������ĵ���ʱ���ɵ��ˣ�
        public float triggerRange = 100;
        // �Ƿ񼤻�
        public bool active;
    }

    public enum EnemyTriggerType
    {
        RANGE,//����rootһ������ʱ����
        INHERIT, //�̳�������һ���ε��˺󴥷�
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


