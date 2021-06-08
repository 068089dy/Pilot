using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants
{
    public const string k_AxisNameVertical = "Vertical";
    public const string k_AxisNameHorizontal = "Horizontal";
    public const string k_MouseAxisNameHorizontal = "Mouse X";
    public const string k_MouseAxisNameVertical = "Mouse Y";
    public const string k_ButtonNameJump = "Jump";
    public const string k_ButtonNameFire = "Fire1";
    public const string k_ButtonNameAim = "Fire2";
    public const string k_ButtonNameHook = "Hook";

    public const string k_ButtonNameSwitchWeapon = "Mouse ScrollWheel";
    //public const string k_ButtonNameNextWeapon = "NextWeapon";

}

public class OperationConstants
{
    public const string controlTitan = "��E��ʻ";
}

public class PhysicsLayer
{
    // ����һ������movebody��
    public const string team1InfantryMoveBodyLayer = "PlayerMoveBody";
    //public LayerMask team1InfantryMoveBodyMask = LayerMask.GetMask("PlayerMoveBody");
    // ����һ������hitbox��
    public const string playerMoveBodyLayer = "PlayerMoveBody";
    public const string team1InfantryHitBodyLayer = "PlayerMoveBody";
    //public LayerMask team1InfantryHitBodyMask = LayerMask.GetMask(playerMoveBodyLayer, team1InfantryHitBodyLayer);
    // ����һTitan��movebody��
    public const string team1TitanMoveBodyLayer = "TitanMoveBody";
    //public LayerMask team1TitanMoveBodyMask = LayerMask.GetMask(team1TitanMoveBodyLayer);
    // ����һTitan��hitbox��
    public const string team1TitanHitBodyLayer = "TitanHitBody";
    //public LayerMask team1TitanHitBodyMask = LayerMask.GetMask("TitanHitBody");

    // �����������movebody��
    public const string team2InfantryMoveBodyLayer = "EnemyMoveBody";
    //public LayerMask team2InfantryMoveBodyMask = LayerMask.GetMask("EnemyMoveBody");
    // �����������hitBox��
    public const string team2InfantryHitBodyLayer = "EnemyMoveBody";
    //public LayerMask team2InfantryHitBodyMask = LayerMask.GetMask("EnemyMoveBody");
    // �����Titan��movebody��
    public const string team2TitanMoveBodyLayer = "EnemyTitanMoveBody";
    //public LayerMask team2TitanMoveBodyMask = LayerMask.GetMask("EnemyTitanMoveBody");
    // �����Titan��hitbox��
    public const string team2TitanHitBodyLayer = "EnemyTitanHitBody";
    //public LayerMask team2TitanHitBodyMask = LayerMask.GetMask("EnemyTitanHitBody");

    // ����һ��Ա��ҩ�ɴ���Ĳ�
    public static LayerMask team1CanHitLayerMask = LayerMask.GetMask("Default", team2TitanHitBodyLayer, team2InfantryHitBodyLayer);
    // �������Ա��ҩ�ɴ���Ĳ�
    public static LayerMask team2CanHitLayerMask = LayerMask.GetMask("Default", team1TitanHitBodyLayer, team1InfantryHitBodyLayer, playerMoveBodyLayer);
}
