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

            comboPanel.SetActive(false);
            combosValueText.text = _scoreManager.TotalCombos.ToString();

            resultTitleText.text = "0";
            StartCoroutine(AnimateScore(_scoreManager.Score, () =>
            {
                StartCoroutine(Utilities.Delay(0.5f, () =>
                {
                    comboPanel.SetActive(_scoreManager.TotalCombos > 0);
                }));
            }));

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

        private IEnumerator AnimateScore(int finalScore, Action onComplete)
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
                scoreValueText.text = displayedScore.ToString();

                yield return null;
            }

            scoreValueText.text = finalScore.ToString();

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