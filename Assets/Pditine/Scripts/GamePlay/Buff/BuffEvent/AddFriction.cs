using PurpleFlowerCore;
using PurpleFlowerCore.Utility;
using UnityEngine;

namespace Pditine.GamePlay.Buff
{
    [Configurable("Buff/BuffEvent")]
    [CreateAssetMenu(fileName = "AddFriction",menuName = "AssAssIn/BuffEvent/AddFriction")]
    public class AddFriction : BuffEvent
    {
        [SerializeField] private float frictionAddAdjustment;
        public override void Trigger(BuffInfo buffInfo)
        {
            var thePlayer = buffInfo.target;
            thePlayer.frictionAddAdjustment += frictionAddAdjustment;
        }
    }
}