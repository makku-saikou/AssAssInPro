using Pditine.Player;
using PurpleFlowerCore;
using UnityEngine;
using PurpleFlowerCore.Utility;

namespace Pditine.GamePlay.Buff
{
    [Configurable("Buff/BuffEvent")]
    [CreateAssetMenu(fileName = "DivATK",menuName = "AssAssIn/BuffEvent/DivATK")]
    public class DivATK : BuffEvent
    {
        [SerializeField] private float atkDivAdjustment;
        public override void Trigger(BuffInfo buffInfo)
        {
            var thePlayer = buffInfo.target;
            thePlayer.atkMulAdjustment /= atkDivAdjustment;
        }
    }
}