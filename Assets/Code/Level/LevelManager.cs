using System.Collections.Generic;
using Code.Utils;
using Core.Level;
using UnityEngine;

namespace Code.Level
{
    public class LevelManager : GenericSingleton<LevelManager>
    {
        [SerializeField] private List<LevelDataSO> alllevelDatas;
        
        public LevelDataSO GetLevel(int level)
        {
            if (level >= 0 && level < alllevelDatas.Count)
                return alllevelDatas[level];
            return null;
        }
    }
}
