using System.Collections.Generic;
using Pditine.Data.GameLevel;
using PurpleFlowerCore;
using PurpleFlowerCore.Utility;
using UnityEngine;

namespace Pditine.Data.GameModule
{
    [Configurable("游戏模式")]
    [CreateAssetMenu(fileName = "GameModelData",menuName = "AssAssIn/GameModelData")]
    public class GameModelBase : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string moduleName;
        [SerializeField] private string introduction;
        [SerializeField] private Sprite preview;
        [SerializeField] private List<int> gameLevels = new(); 
        [SerializeField] private bool done;
        public int ID => id;
        public string ModuleName=>moduleName;
        public string Introduction=>introduction;
        public Sprite Preview=>preview;
        public List<int> GameLevels => gameLevels;
        public bool Done=>done;

        public int GetARandomScene()
        {
            return gameLevels[Random.Range(0, gameLevels.Count)];
        }

    }
}
