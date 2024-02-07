using Code.Audio;
using Code.Game.Main;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Screens
{
    public class ResultScreen : ScreenBase
    {
        [SerializeField] private TMP_Text scoreValueText;
        [SerializeField] private TMP_Text combosValueText;
        [SerializeField] private Button nextActionButton;
        public override void OnShow()
        {
            base.OnShow();
            nextActionButton.onClick.AddListener(OnNextBtnClick);
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySfx("Game:Sfx:GameOver");
            SetData();
        }

        private void SetData()
        {
            
        }

        private void OnNextBtnClick()
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