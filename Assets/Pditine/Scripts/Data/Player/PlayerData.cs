// -------------------------------------------------
// Copyright@ The Most Devout Followers of McDonald
// Author : lijianhao
// Date: 2024_12_05
// -------------------------------------------------

using PurpleFlowerCore;
using PurpleFlowerCore.Utility;
using UnityEngine;

namespace Pditine.Data.Player
{
    [Configurable("PlayerData")]
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [Header("玩家蓄力可存储的最大能量")]
        public float battery = 25;
        [Header("玩家能量的恢复速度")]
        public float energyRecoverSpeed = 25;
        [Header("玩家点击发射后,能量开始恢复的时间")]
        public float recoverCD = 1;
        [Header("玩家的转向速度")]
        public float rotateSpeed = 0.4f;
        [Header("键盘输入时,当前方向向目标方向转向的速度")]
        public float keyboardRotateSpeed = 0.05f;
    }
}