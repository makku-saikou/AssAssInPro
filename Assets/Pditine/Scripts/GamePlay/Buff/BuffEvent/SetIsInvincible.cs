using PurpleFlowerCore;
using UnityEngine;

namespace Pditine.GamePlay.Buff
{
    [Configurable("Buff/BuffEvent")]
    [CreateAssetMenu(fileName = "SetIsInvincible",menuName = "AssAssIn/BuffEvent/SetIsInvincible")]
    public class SetIsInvincible : BuffEvent
    {
        [SerializeField] private bool isInvincible;
        public override void Trigger(BuffInfo buffInfo)
        {
            buffInfo.target.isInvincible = isInvincible;
        }
    }
}