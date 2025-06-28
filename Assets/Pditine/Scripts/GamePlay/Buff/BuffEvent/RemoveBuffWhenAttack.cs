using PurpleFlowerCore;
using UnityEngine;
using PurpleFlowerCore.Utility;

namespace Pditine.GamePlay.Buff
{
    [Configurable("Buff/BuffEvent")]
    [CreateAssetMenu(fileName = "RemoveBuffWhenAttack",menuName = "AssAssIn/BuffEvent/RemoveBuffWhenAttack")]
    public class RemoveBuffWhenAttack : BuffEvent
    {
        public override void Trigger(BuffInfo buffInfo)
        {
            var theThorn = buffInfo.target.TheThorn;
            theThorn.OnAttack += () =>
            {
                BuffManager.Instance.LostBuff(buffInfo); //todo:是否有内存泄漏?}
            };
        }
    }
}