using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DamageCounter))]
[DisallowMultipleComponent]
public class Actor : MonoBehaviour
{
    public CharacterType characterType;
    [System.NonSerialized]
    public DamageCounter damageCounter;
    [System.NonSerialized]
    public Health health;
    public Team team;

    // �ɹ����Ĳ�
    //[System.NonSerialized]
    public LayerMask canHitLayerMask;

    // ��ɫhitbox���ڲ�
    //[System.NonSerialized]
    public string hitLayer = "Default";
    
    public float lastBeHurtTime;

    // ���һ�α�˭���
    //[System.NonSerialized]
    public DamageMsg lastDamageMsg;

    public Transform damagePoint;

    public UnityAction setLayerAction;

    public Camera actorMainCamera;

    TeamManager teamManager;
    // Start is called before the first frame update
    void Awake()
    {
        teamManager = FindObjectOfType<TeamManager>();
        if (teamManager)
        {
            teamManager.RegisterActor(this);
        }

        damageCounter = GetComponent<DamageCounter>();
        health = GetComponent<Health>();
        // ���ò�
        // �ҷ�
        if (team == Team.TEAM1)
        {
            canHitLayerMask = PhysicsLayer.team1CanHitLayerMask;
            if (characterType == CharacterType.TITAN)
            {
                hitLayer = PhysicsLayer.team1TitanHitBodyLayer;
            } else if (characterType == CharacterType.ROBOT)
            {
                hitLayer = PhysicsLayer.team1InfantryHitBodyLayer;
            } else if (characterType == CharacterType.PLAYER)
            {
                hitLayer = PhysicsLayer.team1InfantryHitBodyLayer;
            } else if (characterType == CharacterType.BATTERY)
            {
                hitLayer = PhysicsLayer.team1TitanHitBodyLayer;
            }
            
        } else if (team == Team.TEAM2)
        {
            // �з�
            if (characterType == CharacterType.TITAN)
            {
                hitLayer = PhysicsLayer.team2TitanHitBodyLayer;
            }
            else if (characterType == CharacterType.ROBOT)
            {
                hitLayer = PhysicsLayer.team2InfantryHitBodyLayer;
            }
            else if (characterType == CharacterType.PLAYER)
            {
                hitLayer = PhysicsLayer.team2InfantryHitBodyLayer;
            }
            else if (characterType == CharacterType.BATTERY)
            {
                hitLayer = PhysicsLayer.team2TitanHitBodyLayer;
            }
            canHitLayerMask = PhysicsLayer.team2CanHitLayerMask;
        }
        setLayerAction?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum CharacterType
{
    ROBOT,
    PLAYER,
    TITAN,
    BATTERY,
    SHIP
}

public enum Team
{
    NOTEAM,
    TEAM1,
    TEAM2
}
