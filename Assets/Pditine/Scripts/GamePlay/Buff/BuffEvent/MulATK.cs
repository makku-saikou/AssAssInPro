using Pditine.Player;
using PurpleFlowerCore;
using UnityEngine;
using PurpleFlowerCore.Utility;

namespace Pditine.GamePlay.Buff
{
    [Configurable("Buff/BuffEvent")]
    [CreateAssetMenu(fileName = "MulATKEvent",menuName = "AssAssIn/BuffEvent/MulATK")]
    public class MulATK : BuffEvent
    {
        [SerializeField] private float atkMulAdjustment;
        public override void Trigger(BuffInfo buffInfo)
        {
            var thePlayer = buffInfo.target;
            thePlayer.atkMulAdjustment *= atkMulAdjustment;
            
        }
    }
}