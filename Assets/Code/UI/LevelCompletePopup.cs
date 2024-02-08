using System;
using Code.Game.Main;
using Code.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class LevelCompletePopup : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private Button clearDataButton;

        private void OnEnable()
        {
            clearDataButton.onClick.AddListener(OnClearDataBtnClick);
        }

        private void OnDisable()
        {
            clearDataButton.onClick.RemoveAllListeners();
        }

        private void OnClearDataBtnClick()
        {
            PlayerDataManager.Instance.ClearPlayerData();
            GameFlowManager.Instance.ChangeState(GameState.MainMenu);
            container.SetActive(false);
        }

        public void Show()
        {
            container.SetActive(true);
        }
    }
}