using System;
using Code.Level;
using Code.Player;
using Code.Utils;
using UnityEngine;

namespace Code.Game.Controller
{
    public class GameplayManager : GenericSingleton<GameplayManager>
    {
        [SerializeField] private CardGridGenerator cardGridGenerator;
        [SerializeField] private CardMatchValidator cardMatchValidator;

        private void Start()
        {
            StartGamePlay();
        }

        private void StartGamePlay()
        {
            var levelData = LevelManager.Instance.GetLevel(PlayerDataManager.Instance.currentLevel);
            cardGridGenerator.Initialize(levelData);
            var generatedCard = cardGridGenerator.GetGeneratedCards();
            if (generatedCard != null)
            {
                cardMatchValidator.Initialize(generatedCard, levelData);
                cardMatchValidator.OnGameOver += OnGameOver;
            }
        }

        private void OnGameOver()
        {
            //Testing
            PlayerDataManager.Instance.currentLevel++;
            StartGamePlay();
        }

        private void EndGamePlay()
        {
            
        }
    }
}
