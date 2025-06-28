using Pditine.Player;
using PurpleFlowerCore;
using UnityEngine;
using PurpleFlowerCore.Utility;

namespace Pditine.GamePlay.Buff
{
    [Configurable("Buff/BuffEvent")]
    [CreateAssetMenu(fileName = "MulInitialVelocity",menuName = "AssAssIn/BuffEvent/MulInitialVelocity")]
    public class MulInitialVelocity : BuffEvent
    {
        [SerializeField] private float initialVelocityMulAdjustment;
        public override void Trigger(BuffInfo buffInfo)
        {
            var thePlayer = buffInfo.target;
            //thePlayer.initialVelocityMulAdjustment *= initialVelocityMulAdjustment;
        }
    }
}