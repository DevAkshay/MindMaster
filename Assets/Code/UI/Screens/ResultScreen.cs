using System;
using System.Collections;
using Code.Audio;
using Code.Game.Core;
using Code.Game.Main;
using Code.Score;
using Code.UI.Controller;
using Code.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Screens
{
    public class ResultScreen : ScreenBase
    {
        [SerializeField] private TMP_Text resultTitleText;
        [SerializeField] private TMP_Text scoreValueText;
        [SerializeField] private TMP_Text combosValueText;
        [SerializeField] private TMP_Text nextActionBtnText;

        [SerializeField] private GameObject lineSeparator;
        [SerializeField] private GameObject comboPanel;

        [SerializeField] private Button nextActionButton;
        [SerializeField] private Button homeButton;

        private const string GameOverAudioId = "Game:Sfx:GameOver";

        private GameplayManager _gameplayManager => GameplayManager.Instance;
        private ScoreManager _scoreManager => ScoreManager.Instance;

        public override void OnShow()
        {
            base.OnShow();
            nextActionButton.onClick.AddListener(OnNextActionBtnClick);
            homeButton.onClick.AddListener(OnHomeBtnClick);

            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySfx(GameOverAudioId);

            lineSeparator.SetActive(false);
            comboPanel.SetActive(false);
            
            scoreValueText.text = "0";
            combosValueText.text = "0";

            StartCoroutine(AnimateScore(_scoreManager.Score, scoreValueText, null));

            if (_scoreManager.TotalCombos > 0)
            {
                lineSeparator.SetActive(true);
                comboPanel.SetActive(true);
                StartCoroutine(AnimateScore(_scoreManager.TotalCombos, combosValueText,null));
            }

            var hasPlayerWon = _gameplayManager.GameResultData.isCompleted;
            resultTitleText.text = hasPlayerWon ? "YOU WIN" : "YOU LOST";
            nextActionBtnText.text = hasPlayerWon ? "PLAY NEXT" : "RETRY";
        }

        private void OnHomeBtnClick()
        {
            GameFlowManager.Instance.ChangeState(GameState.MainMenu);
        }

        private void OnNextActionBtnClick()
        {
            GameFlowManager.Instance.ChangeState(GameState.Gameplay);
        }

        private IEnumerator AnimateScore(int finalScore, TMP_Text textAsset, Action onComplete)
        {
            var duration = 0.5f; 
            float elapsedTime = 0;

            yield return new WaitForSeconds(1f);
            
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                var t = elapsedTime / duration;
                t = Mathf.SmoothStep(0, 1, t);

                var displayedScore = Mathf.RoundToInt(Mathf.Lerp(0, finalScore, t));
                textAsset.text = displayedScore.ToString();

                yield return null;
            }

            textAsset.text = finalScore.ToString();

            onComplete?.Invoke();
        }


        public override void OnHide()
        {
            base.OnHide();
            nextActionButton.onClick.RemoveAllListeners();
            homeButton.onClick.RemoveAllListeners();
        }
    }
}