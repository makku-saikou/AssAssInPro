using Org.BouncyCastle.Pqc.Crypto.Ntru;
using PurpleFlowerCore;
using PurpleFlowerCore.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Pditine.GamePlay.Buff
{
    /// <summary>
    /// Buff增加时的更新方式
    /// </summary>
    public enum BuffAttachType
    {
        Override,   // 覆写
        Add,        // 增加层数，时间叠加
        Keep        // 无法叠加
    }

    /// <summary>
    /// Buff时间到时的更新方式
    /// </summary>
    public enum BuffLostType
    {
        Reduce,     // 减少一层
        Clear       // 全部移除
    }

    [Configurable("Buff/BuffData")]
    [CreateAssetMenu(fileName = "BuffData", menuName = "AssAssIn/BuffData")]
    public class BuffData : ScriptableObject
    {
        [Title("Base Info")]
        public int id;
        public string buffName;
        public string description;
        public Sprite icon;
        public int priority;
        public int maxStack;
        public string[] tags;
        public bool showInUI;

        [Title("Time Info")]
        public bool willLastForever;
        public float durationTime; // 大于等于10000则认为持续时间无限
        public float tickTime;

        [Title("Update Type")]
        public BuffAttachType attachType;
        public BuffLostType lostType;

        [Title("Callback Event")]
        [InlineEditor] public BuffEvent[] onInitEvents;
        [InlineEditor] public BuffEvent[] onAttachEvents;
        [InlineEditor] public BuffEvent[] onLostEvents;
        [InlineEditor] public BuffEvent[] onTickEvents;
        [InlineEditor] public BuffEvent[] onResetEvents;
    }
}