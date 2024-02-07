using System.Collections;
using Code.Game.Core;
using Code.Level;
using Code.Player;
using MPUIKIT;
using TMPro;
using UnityEngine;

namespace Code.UI.Screens
{
    public class GamePlayScreen : ScreenBase
    {
        [SerializeField] private TMP_Text attemptDescriptionText;
        [SerializeField] private TMP_Text attemptProgressCountText;
        [SerializeField] private TMP_Text scoreValueText;
        [SerializeField] private MPImage attemptProgressImage;

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

            var levelData = LevelManager.Instance.GetLevel(PlayerDataManager.Instance.currentLevel);
            _totalAttempts = levelData.NumberOfAttempts;
            attemptProgressCountText.text = "0/" + _totalAttempts;
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

            attemptProgressCountText.text = $"{attemptsUsed}/{_totalAttempts}";

            var progress = 1 - (float)attemptsUsed / _totalAttempts;
            attemptProgressImage.fillAmount = progress;
        }


        private void GameEventsOnComboAchieved(int comboCount)
        {
            Debug.Log($"{comboCount} x Combo");
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
            var duration = 1.0f;
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