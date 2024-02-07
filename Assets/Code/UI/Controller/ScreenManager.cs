using System;
using System.Collections.Generic;
using Code.UI.Controller;
using Code.Utils;
using UnityEngine;

namespace Code.UI
{
    public class ScreenManager : GenericSingleton<ScreenManager>
    {
        private Dictionary<Type, ScreenBase> _screens = new Dictionary<Type, ScreenBase>();

        protected override void Awake()
        {
            RegisterAllScreens();
        }

        private void RegisterAllScreens()
        {
            foreach (var screen in GetComponentsInChildren<ScreenBase>(true))
            {
                _screens[screen.GetType()] = screen;
                screen.gameObject.SetActive(false); // Initially deactivate screens
            }
        }

        public void ShowScreen<T>() where T : ScreenBase
        {
            var screenType = typeof(T);
            if (_screens.TryGetValue(screenType, out ScreenBase screen))
            {
                foreach (var s in _screens.Values)
                {
                    if (s.GetType() != screenType)
                    {
                        s.OnHide(); // Hide all other screens
                    }
                }

                screen.OnShow(); // Show the requested screen
            }
            else
            {
                Debug.LogWarning($"ScreenManager: Screen of type {screenType.Name} not found.");
            }
        }

        public void HideScreen<T>() where T : ScreenBase
        {
            var screenType = typeof(T);
            if (_screens.TryGetValue(screenType, out ScreenBase screen))
            {
                screen.OnHide();
            }
            else
            {
                Debug.LogWarning($"ScreenManager: Screen of type {screenType.Name} not found.");
            }
        }
    }
}