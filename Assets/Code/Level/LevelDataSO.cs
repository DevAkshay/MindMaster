using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Level
{
    [CreateAssetMenu(menuName = "Level Data", fileName = "New Level Data")]
    public class LevelDataSO : ScriptableObject
    {
        public int RowCount;
        public int ColumnCount;
        public List<Sprite> CardSprites;
    }
}

