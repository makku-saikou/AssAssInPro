using PurpleFlowerCore;
using PurpleFlowerCore.Utility;
using UnityEngine;

namespace Pditine.Data.Thorn
{
    [Configurable("刺")]
    [CreateAssetMenu(fileName = "ThornData",menuName = "AssAssIn/Thorn")]
    public class ThornDataBase : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private float atk;
        // [SerializeField] private float cd;
        [SerializeField] private float speedCoefficient;
        [SerializeField] private float weight;
        [SerializeField] private GameObject prototype;
        [SerializeField] private Sprite portrait;
        [SerializeField] private string thornName;
        [SerializeField] private string thornIntroduction;

        public int ID => id;
        public float ATK => atk;
        // public float CD => cd;
        public float SpeedCoefficient => speedCoefficient;
        public float Weight=>weight;
        public GameObject Prototype=>prototype;
        public Sprite Portrait => portrait;
        public string ThornName => thornName;
        public string ThornIntroduction => thornIntroduction;
    }
}