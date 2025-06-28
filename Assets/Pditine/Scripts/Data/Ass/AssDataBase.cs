using PurpleFlowerCore;
using PurpleFlowerCore.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pditine.Data.Ass
{
    [Configurable("Ass")]
    [CreateAssetMenu(fileName = "AssData",menuName = "AssAssIn/Ass")]
    public class AssDataBase : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private float hp;
        // [SerializeField] private float initialVelocity;
        [SerializeField] private float weight;
        [SerializeField] private float energy;
        [SerializeField] private GameObject prototype;
        [SerializeField] private Sprite portraitBlue;
        [SerializeField] private Sprite portraitYellow;
        [SerializeField] private string assName;
        [SerializeField] private string assIntroduction;
        
        public int ID => id;
        public float HP => hp;
        // public float InitialVelocity => initialVelocity;
        public float Weight=>weight;
        public float Energy => energy;
        public GameObject Prototype=>prototype;
        public Sprite PortraitBlue => portraitBlue;
        public Sprite PortraitYellow => portraitYellow;
        public string AssName => assName;
        public string AssIntroduction => assIntroduction;
    }
}