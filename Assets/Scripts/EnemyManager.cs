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
    // Start is called before the first frame update
    void Start()
    {
        curGenerateState = new GenerateState(0, 0);
        //FindObjectOfType<PlayerCharacterController>()
    }

    // Update is called once per frame
    void Update()
    {
        if (branchs.Count > 0) {
            foreach (EnemyBatchMsg branch in branchs)
            {
                // �ж�team1��Ա��centerPoint�ľ����Ƿ�С��triggerRange
                if (CheckTeam1InRange(branch.centerPoint.position, branch.triggerRange))
                {
                    if (branch.enemyMsgs.Length > 0)
                    {
                        foreach (EnemyMsg enemyMsg in branch.enemyMsgs)
                        {
                            if (enemyMsg.characterType == CharacterType.ROBOT)
                            {
                                foreach (Transform pos in enemyMsg.birthPoints)
                                {
                                    GameObject robot = Instantiate(robotPrefab, pos.position, Quaternion.identity);
                                    robot.GetComponent<EnemyBehaviorController>().patrolPoints.Add(pos);
                                }
                            }
                        }
                    }
                    branchs.Remove(branch);
                    break;
                }
            }
        }
    }

    bool CheckTeam1InRange(Vector3 center, float range)
    {
        GameObject[] team1mates = GameObject.FindGameObjectsWithTag("group1");
        foreach (GameObject tm in team1mates)
        {
            if (Vector3.Distance(tm.transform.position, center) <= range)
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
        public EnemyMsg[] enemyMsgs;
        public Transform centerPoint;
        // ������Χ���������ĵ���ʱ���ɵ��ˣ�
        public float triggerRange = 100;
    }

    [Serializable]
    public class EnemyMsg
    {
        public CharacterType characterType = CharacterType.ROBOT;
        public int enemyCount = 10;
        public Transform[] birthPoints;
    }
}


