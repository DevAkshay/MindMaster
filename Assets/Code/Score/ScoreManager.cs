using System;
using Code.Game.Core;
using Code.Utils;
using Core.Level;
using UnityEngine;

namespace Code.Score
{
    public class ScoreManager : GenericSingleton<ScoreManager>
    {
        private int _pointsPerCardMatch;
        private int _score;
        private int _comboCount;
        private int _totalCombos; 

        public int Score
        {
            get => _score;
            private set
            {
                _score = value;
                GameEvents.NotifyScoreChanged(_score);
            }
        }

        public int TotalCombos => _totalCombos;

        public void Init(LevelDataSO levelData)
        {
            ResetScoreAndCombos();
            _pointsPerCardMatch = levelData.PointsPerMatch;
        }
        
        public void CardMatch()
        {
            Score += _pointsPerCardMatch;

            _comboCount++;
            if (_comboCount > 1)
            {
                GameEvents.NotifyComboAchieved(_comboCount);
                _totalCombos++;
            }
        }

        public void CardMiss()
        {
            ResetCombo();
        }

        private void ResetScoreAndCombos()
        {
            Score = 0;
            ResetCombo();
            _totalCombos = 0;
        }

        private void ResetCombo()
        {
            _comboCount = 0;
        }
    }
}