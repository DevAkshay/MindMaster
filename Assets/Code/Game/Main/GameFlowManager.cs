using System;
using System.Collections;
using Code.Game.Core;
using Code.UI;
using Code.UI.Screens;
using Code.Utils;
using UnityEngine;

namespace Code.Game.Main
{
    public enum GameState
    {
        SplashScreen,
        MainMenu,
        Gameplay,
        Result,
        Setting
    }

    public class GameFlowManager : GenericSingleton<GameFlowManager>
    {
        private GameState _currentState;
        private ScreenManager _screenManager;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _screenManager = ScreenManager.Instance;
            ChangeState(GameState.SplashScreen);
        }

        public void ChangeState(GameState newState)
        {
            _currentState = newState;
            switch (_currentState)
            {
                case GameState.SplashScreen:
                    StartCoroutine(HandleSplashScreen());
                    break;
                case GameState.MainMenu:
                    ShowMainMenu();
                    break;
                case GameState.Gameplay:
                    StartGameplay();
                    break;
                case GameState.Result:
                    ShowResult();
                    break;
                case GameState.Setting:
                    GoSetting();
                    break;
            }
            GameEvents.NotifyGameStateChange(newState);
        }

        private IEnumerator HandleSplashScreen()
        {
            _screenManager.ShowScreen<SplashScreen>();
            yield return new WaitForSeconds(4);
            _screenManager.HideScreen<SplashScreen>();
            ChangeState(GameState.MainMenu);
        }

        private void ShowMainMenu()
        {
            _screenManager.ShowScreen<MainMenuScreen>();
        }

        private void StartGameplay()
        {
        }

        private void ShowResult()
        {
            _screenManager.ShowScreen<ResultScreen>();
        }

        private void GoSetting()
        {
            ChangeState(GameState.MainMenu);
        }

        public void OnPlayClicked()
        {
            if (_currentState == GameState.MainMenu) ChangeState(GameState.Gameplay);
        }
    }
}