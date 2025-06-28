using PurpleFlowerCore;
using UnityEngine;

namespace Pditine.GamePlay.Buff
{
    [Configurable("Buff/BuffEvent"),
     CreateAssetMenu(fileName = "RemoveBuffWhenBeAttack", menuName = "AssAssIn/BuffEvent/RemoveBuffWhenBeAttack")]
    public class RemoveBuffWhenBeAttack : BuffEvent
    {
        public override void Trigger(BuffInfo buffInfo)
        {
            var theAss = buffInfo.target.TheAss;
            theAss.OnBeAttack += (c) => BuffManager.Instance.LostBuff(buffInfo);//todo:是否有内存泄漏?
        }
    }
}