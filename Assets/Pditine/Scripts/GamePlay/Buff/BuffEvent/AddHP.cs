using PurpleFlowerCore;
using PurpleFlowerCore.Utility;
using UnityEngine;

namespace Pditine.GamePlay.Buff
{
    [Configurable("Buff/BuffEvent")]
    [CreateAssetMenu(fileName = "AddHP",menuName = "AssAssIn/BuffEvent/AddHP")]
    public class AddHP : BuffEvent
    {
        [SerializeField] private int hpAddAdjustment;
        public override void Trigger(BuffInfo buffInfo)
        {
            var thePlayer = buffInfo.target;
            thePlayer.ChangeHP(hpAddAdjustment);
        }
    }
}