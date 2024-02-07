using Code.Audio;
using Code.Game.Core;
using Code.Game.Main;
using Code.Player;
using Code.Score;
using Code.UI.Controller;
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
        [SerializeField] private Button nextActionButton;
        [SerializeField] private TMP_Text nextActionBtnText;

        private GameplayManager _gameplayManager => GameplayManager.Instance;
        private ScoreManager _scoreManager => ScoreManager.Instance;
        
        public override void OnShow()
        {
            base.OnShow();
            nextActionButton.onClick.AddListener(OnNextActionBtnClick);
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySfx("Game:Sfx:GameOver");

            scoreValueText.text = _scoreManager.Score.ToString();
            combosValueText.text = _scoreManager.TotalCombos.ToString();

            var hasPlayerWon = _gameplayManager.GameResultData.isCompleted;
            resultTitleText.text = hasPlayerWon? "YOU WIN" : "YOU LOST";
            nextActionBtnText.text = hasPlayerWon ? "PLAY NEXT" : "RETRY";
        }

        private void OnNextActionBtnClick()
        {
            GameFlowManager.Instance.ChangeState(GameState.Gameplay);
        }

        public override void OnHide()
        {
            base.OnHide();
            nextActionButton.onClick.RemoveAllListeners();
        }
    }
}