using System.Collections;
using Code.Game.Main;
using Code.UI.Controller;
using Code.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Screens
{
    public class MainMenuScreen : ScreenBase
    {
        [SerializeField] private Button settingButton;
        [SerializeField] private Button gameStartButton;

        [SerializeField] private GameObject gameLogoObj;
        [SerializeField] private CanvasGroup gameMessageTextCanvasGroup;
        [SerializeField] private CanvasGroup gameStartButtonObjCanvasGroup;

        public override void OnShow()
        {
            base.OnShow();
            settingButton.onClick.AddListener(OnSettingBtnClick);
            gameStartButton.onClick.AddListener(OnGameStartBtnClick);
            
            gameStartButtonObjCanvasGroup.alpha = 0;
            gameMessageTextCanvasGroup.alpha = 0;

            StartCoroutine(Utilities.MoveYCoroutine(gameLogoObj.transform, 443,1f, 0.5f, () =>
            {
                FadeInGameMessage(0.5f);
                StartCoroutine(Utilities.Delay(0.5f, () =>
                {
                    FadeInPlayButton(0.3f);
                }));
            }));
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
        
        private void FadeInGameMessage(float duration)
        {
            StartCoroutine(Utilities.FadeInCoroutine(duration, gameMessageTextCanvasGroup));
        }
        
        private void FadeInPlayButton(float duration)
        {
            StartCoroutine(Utilities.FadeInCoroutine(duration, gameStartButtonObjCanvasGroup));
        }
        
    }
}