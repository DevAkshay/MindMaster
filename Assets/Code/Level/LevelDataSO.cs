using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Level
{
    [CreateAssetMenu(menuName = "Level Data", fileName = "New Level Data")]
    public class LevelDataSO : ScriptableObject
    {
        public int PointsPerMatch;
        public int NumberOfAttempts;
        public int RowCount;
        public int ColumnCount;
        public List<Sprite> CardSprites;
    }
}

