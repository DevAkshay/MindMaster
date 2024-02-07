using Code.Audio;
using Code.Game.Controller;
using Code.Game.Main;
using Code.Level;
using Code.Player;
using Code.Score;
using Code.UI;
using Code.UI.Screens;
using Code.Utils;
using Core.Level;
using UnityEngine;

namespace Code.Game.Core
{
    public class GameplayManager : GenericSingleton<GameplayManager>
    {
        [SerializeField] private CardGridGenerator cardGridGenerator;
        [SerializeField] private CardMatchValidator cardMatchValidator;

        private PlayerDataManager PlayerDataManager => PlayerDataManager.Instance;
        private ScoreManager ScoreManager => ScoreManager.Instance;

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
            if (gameState == GameState.Gameplay) StartGamePlay();
        }

        private void StartGamePlay()
        {
            var levelData = LevelManager.Instance.GetLevel(PlayerDataManager.ActiveLevelIndex);
            if (levelData == null)
            {
                Debug.LogError($"Level data of Level index {PlayerDataManager.ActiveLevelIndex} is null");
                return;
            }

            if (!IsLevelGridSizeValid(levelData))
            {
                Debug.LogError(
                    $"Level data grid size not an even number, recheck the level index : {PlayerDataManager.ActiveLevelIndex} data");
                return;
            }

            cardGridGenerator.Initialize(levelData);
            ScoreManager.Init(levelData);
            var generatedCard = cardGridGenerator.GetGeneratedCards();
            if (generatedCard != null)
            {
                cardMatchValidator.Initialize(generatedCard, levelData);
                cardMatchValidator.OnGameOver += OnGameOver;
                cardMatchValidator.OnPairMatched += OnCardMatch;
                cardMatchValidator.OnPairMismatched += OnCardMiss;
            }

            ScreenManager.Instance.ShowScreen<GamePlayScreen>();
        }

        private bool IsLevelGridSizeValid(LevelDataSO levelData)
        {
            var row = levelData.RowCount;
            var column = levelData.ColumnCount;

            return row * column % 2 == 0;
        }

        private void OnCardMiss()
        {
            AudioManager.Instance.PlaySfx("Game:Sfx:CardMisMatch");
            ScoreManager.Instance.CardMiss();
        }

        private void OnCardMatch()
        {
            AudioManager.Instance.PlaySfx("Game:Sfx:CardMatch");
            ScoreManager.Instance.CardMatch();
        }

        private void OnGameOver(bool isLevelComplete)
        {
            GameFlowManager.Instance.ChangeState(GameState.Result);
            var result = new PlayerLevelData(PlayerDataManager.ActiveLevelIndex, ScoreManager.Score, isLevelComplete, false);
            PlayerDataManager.SavePlayerResult(result);
            ScoreManager.ResetScoreAndCombos();
            cardMatchValidator.OnGameOver -= OnGameOver;
            cardMatchValidator.OnPairMatched -= OnCardMatch;
            cardMatchValidator.OnPairMismatched -= OnCardMiss;
        }
    }
}