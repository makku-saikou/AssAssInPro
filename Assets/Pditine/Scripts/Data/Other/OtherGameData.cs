// -------------------------------------------------
// Copyright@ The Most Devout Followers of McDonald
// Author : lijianhao
// Date: 2024_12_09
// -------------------------------------------------

using PurpleFlowerCore;
using PurpleFlowerCore.Utility;
using UnityEngine;

namespace Pditine.Data.Other
{
    [CreateAssetMenu(fileName = "OtherGameData", menuName = "AssAssIn/OtherGameData")]
    [Configurable]
    public class OtherGameData : ScriptableObject
    {
        public float keyBoardRotateSpeed = 0.05f;
    }
}