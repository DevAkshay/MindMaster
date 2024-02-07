using Code.Game.Main;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Screens
{
    public class MainMenuScreen : ScreenBase
    {
        [SerializeField] private Button settingButton;
        [SerializeField] private Button gameStartButton;
        public override void OnShow()
        {
            base.OnShow();
            settingButton.onClick.AddListener(OnSettingBtnClick);
            gameStartButton.onClick.AddListener(OnGameStartBtnClick);
        }

        private void OnGameStartBtnClick()
        {
            GameFlowManager.Instance.ChangeState(GameState.Gameplay);
        }

        private void OnSettingBtnClick()
        {
            GameFlowManager.Instance.ChangeState(GameState.Setting);
        }

        public override void OnHide()
        {
            base.OnHide();
            settingButton.onClick.RemoveAllListeners();
            gameStartButton.onClick.RemoveAllListeners();
        }
    }
}