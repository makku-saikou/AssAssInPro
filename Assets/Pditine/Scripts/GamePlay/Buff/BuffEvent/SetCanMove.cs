using PurpleFlowerCore;
using UnityEngine;
using PurpleFlowerCore.Utility;

namespace Pditine.GamePlay.Buff
{
    [Configurable("Buff/BuffEvent")]
    [CreateAssetMenu(fileName = "SetCanMove",menuName = "AssAssIn/BuffEvent/SetCanMove")]
    public class SetCanMove : BuffEvent
    {
        [SerializeField] private bool canMove;
        public override void Trigger(BuffInfo buffInfo)
        {
            buffInfo.target.canMove = canMove;
        }
    }
}