using System;
using Code.Game.Core;
using Code.Game.Main;
using Code.Level;
using Code.Player;
using Code.Score;
using Code.UI;
using Code.UI.Screens;
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
            GameEvents.GameStateChange += GameEventsOnGameStateChange;
        }

        private void OnDestroy()
        {
            GameEvents.GameStateChange -= GameEventsOnGameStateChange;
        }

        private void GameEventsOnGameStateChange(GameState gameState)
        {
            if (gameState == GameState.Gameplay)
            {
                StartGamePlay();
            }
        }

        private void StartGamePlay()
        {
            var levelData = LevelManager.Instance.GetLevel(PlayerDataManager.Instance.currentLevel);
            cardGridGenerator.Initialize(levelData);
            ScoreManager.Instance.Init(levelData);
            ScreenManager.Instance.ShowScreen<GamePlayScreen>();
            var generatedCard = cardGridGenerator.GetGeneratedCards();
            if (generatedCard != null)
            {
                cardMatchValidator.Initialize(generatedCard, levelData);
                cardMatchValidator.OnGameOver += OnGameOver;
                cardMatchValidator.OnPairMatched += OnCardMatch;
                cardMatchValidator.OnPairMismatched += OnCardMiss;
            }
        }

        private void OnCardMiss()
        {
            ScoreManager.Instance.CardMiss();
        }

        private void OnCardMatch()
        {
            ScoreManager.Instance.CardMatch();
        }

        private void OnGameOver()
        {
            PlayerDataManager.Instance.currentLevel++;
            GameFlowManager.Instance.ChangeState(GameState.Result);
        }

        private void EndGamePlay()
        {
            
        }
    }
}
