using System;
using Code.Game.Main;
using UnityEngine;

namespace Code.Game.Core
{
    public static class GameEvents
    {
        public static event Action<int> ScoreChanged;

        public static void NotifyScoreChanged(int newScore)
        {
            ScoreChanged?.Invoke(newScore);
        }

        public static event Action<int> AttemptCountUpdated;

        public static void NotifyAttemptCountUpdated(int remainingAttempts)
        {
            AttemptCountUpdated?.Invoke(remainingAttempts);
        }

        public static event Action<int> ComboAchieved;

        public static void NotifyComboAchieved(int comboValue)
        {
            ComboAchieved?.Invoke(comboValue);
        }
        
        public static event Action<GameState> GameStateChange;

        public static void NotifyGameStateChange(GameState gameState)
        {
            GameStateChange?.Invoke(gameState);
        }
    }
}
