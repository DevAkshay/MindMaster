using System.Collections;
using System.Collections.Generic;
using Code.Game.Core;
using Code.Level;
using Code.Player;
using MPUIKIT;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class GamePlayScreen : ScreenBase
    {
        [SerializeField] private TMP_Text attemptDescriptionText;
        [SerializeField] private TMP_Text attemptProgressCountText;
        [SerializeField] private TMP_Text scoreValueText;
        [SerializeField] private MPImage attemptProgressImage;

        private int _totalAttempts;
        private int currentDisplayedScore = 0;

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
            int attemptsUsed = _totalAttempts - remainingAttempts;

            attemptProgressCountText.text = $"{attemptsUsed}/{_totalAttempts}";

            float progress = 1 - ((float)attemptsUsed / _totalAttempts); 
            attemptProgressImage.fillAmount = progress; 
        }


        private void GameEventsOnComboAchieved(int obj)
        {
            //TODO show a combo recieved animation
        }

        private void GameEventsOnScoreChanged(int newScore)
        {
            StopCoroutine("AnimateScoreChange");
            StartCoroutine(AnimateScoreChange(newScore));
        }
        
        private IEnumerator AnimateScoreChange(int newScore)
        {
            // Time it takes to count to the new score
            float duration = 1.0f;
            float counter = 0;

            int startScore = currentDisplayedScore;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                float t = Mathf.Clamp01(counter / duration);
                
                currentDisplayedScore = (int)Mathf.Lerp(startScore, newScore, t);
                scoreValueText.text = currentDisplayedScore.ToString();

                yield return null; 
            }

            // Ensure the final score is set correctly after the animation
            currentDisplayedScore = newScore;
            scoreValueText.text = currentDisplayedScore.ToString();
        }

        private void UnsubscribeGameEvents()
        {
            GameEvents.ScoreChanged -= GameEventsOnScoreChanged;
            GameEvents.ComboAchieved -= GameEventsOnComboAchieved;
            GameEvents.AttemptCountUpdated -= GameEventsOnAttemptCountUpdated;
        }
    }
}