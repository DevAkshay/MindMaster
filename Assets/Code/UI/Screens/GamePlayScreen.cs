using System.Collections;
using Code.Audio;
using Code.Game.Core;
using Code.Level;
using Code.Player;
using Code.UI.Controller;
using MPUIKIT;
using TMPro;
using UnityEngine;

namespace Code.UI.Screens
{
    public class GamePlayScreen : ScreenBase
    {
        [SerializeField] private TMP_Text attemptDescriptionText;
        [SerializeField] private TMP_Text currentLevelText;
        [SerializeField] private TMP_Text attemptProgressCountText;
        [SerializeField] private TMP_Text scoreValueText;
        [SerializeField] private MPImage attemptProgressImage;
        [SerializeField] private ComboController comboController;

        private int _totalAttempts;
        private int _currentDisplayedScore;
        
        private Coroutine _scoreAnimationCoroutine;

        public override void OnShow()
        {
            base.OnShow();
            Initialize();
            SubscribeGameEvents();
        }

        private void Initialize()
        {
            scoreValueText.text = "0";
            attemptProgressImage.fillAmount = 1;

            var activeLevelIndex = PlayerDataManager.Instance.ActiveLevelIndex;
            var levelData = LevelManager.Instance.GetLevel(activeLevelIndex);
            _totalAttempts = levelData.NumberOfAttempts;

            currentLevelText.text = (activeLevelIndex + 1).ToString();
            attemptProgressCountText.text = $"{_totalAttempts}/{_totalAttempts}";
            attemptDescriptionText.text = $"You have {_totalAttempts} attempts to match all cards";
        }

        public override void OnHide()
        {
            base.OnHide();
            UnsubscribeGameEvents();
        }

        private void SubscribeGameEvents()
        {
            GameEvents.ScoreChanged += GameEventsOnScoreChanged;
            GameEvents.ComboAchieved += GameEventsOnComboAchieved;
            GameEvents.AttemptCountUpdated += GameEventsOnAttemptCountUpdated;
        }

        private void GameEventsOnAttemptCountUpdated(int remainingAttempts)
        {
            var attemptsUsed = _totalAttempts - remainingAttempts;

            attemptProgressCountText.text = $"{remainingAttempts}/{_totalAttempts}";
            attemptDescriptionText.text = $"You have {remainingAttempts} attempts to match all cards";

            var progress = 1 - (float)attemptsUsed / _totalAttempts;
            attemptProgressImage.fillAmount = progress;
        }


        private void GameEventsOnComboAchieved(int comboCount)
        {
            comboController.ShowCombo(comboCount);
        }

        private void GameEventsOnScoreChanged(int newScore)
        {
            if (_scoreAnimationCoroutine != null)
            {
                StopCoroutine(_scoreAnimationCoroutine);
            }
            _scoreAnimationCoroutine = StartCoroutine(AnimateScoreChange(newScore));
        }

        private IEnumerator AnimateScoreChange(int newScore)
        {
            // Time it takes to count to the new score
            var duration = 0.3f;
            float counter = 0;

            var startScore = _currentDisplayedScore;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                var t = Mathf.Clamp01(counter / duration);

                _currentDisplayedScore = (int)Mathf.Lerp(startScore, newScore, t);
                scoreValueText.text = _currentDisplayedScore.ToString();

                yield return null;
            }

            // Ensure the final score is set correctly after the animation
            _currentDisplayedScore = newScore;
            scoreValueText.text = _currentDisplayedScore.ToString();
        }

        private void UnsubscribeGameEvents()
        {
            GameEvents.ScoreChanged -= GameEventsOnScoreChanged;
            GameEvents.ComboAchieved -= GameEventsOnComboAchieved;
            GameEvents.AttemptCountUpdated -= GameEventsOnAttemptCountUpdated;
        }
    }
}