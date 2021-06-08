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
    public const string controlTitan = "按E驾驶";
}

public class PhysicsLayer
{
    // 队伍一步兵的movebody层
    public const string team1InfantryMoveBodyLayer = "PlayerMoveBody";
    //public LayerMask team1InfantryMoveBodyMask = LayerMask.GetMask("PlayerMoveBody");
    // 队伍一步兵的hitbox层
    public const string playerMoveBodyLayer = "PlayerMoveBody";
    public const string team1InfantryHitBodyLayer = "PlayerMoveBody";
    //public LayerMask team1InfantryHitBodyMask = LayerMask.GetMask(playerMoveBodyLayer, team1InfantryHitBodyLayer);
    // 队伍一Titan的movebody层
    public const string team1TitanMoveBodyLayer = "TitanMoveBody";
    //public LayerMask team1TitanMoveBodyMask = LayerMask.GetMask(team1TitanMoveBodyLayer);
    // 队伍一Titan的hitbox层
    public const string team1TitanHitBodyLayer = "TitanHitBody";
    //public LayerMask team1TitanHitBodyMask = LayerMask.GetMask("TitanHitBody");

    // 队伍二步兵的movebody层
    public const string team2InfantryMoveBodyLayer = "EnemyMoveBody";
    //public LayerMask team2InfantryMoveBodyMask = LayerMask.GetMask("EnemyMoveBody");
    // 队伍二步兵的hitBox层
    public const string team2InfantryHitBodyLayer = "EnemyMoveBody";
    //public LayerMask team2InfantryHitBodyMask = LayerMask.GetMask("EnemyMoveBody");
    // 队伍二Titan的movebody层
    public const string team2TitanMoveBodyLayer = "EnemyTitanMoveBody";
    //public LayerMask team2TitanMoveBodyMask = LayerMask.GetMask("EnemyTitanMoveBody");
    // 队伍二Titan的hitbox层
    public const string team2TitanHitBodyLayer = "EnemyTitanHitBody";
    //public LayerMask team2TitanHitBodyMask = LayerMask.GetMask("EnemyTitanHitBody");

    // 队伍一成员弹药可打击的层
    public static LayerMask team1CanHitLayerMask = LayerMask.GetMask("Default", team2TitanHitBodyLayer, team2InfantryHitBodyLayer);
    // 队伍二成员弹药可打击的层
    public static LayerMask team2CanHitLayerMask = LayerMask.GetMask("Default", team1TitanHitBodyLayer, team1InfantryHitBodyLayer, playerMoveBodyLayer);
}
