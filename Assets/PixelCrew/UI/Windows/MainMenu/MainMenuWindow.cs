using System;
using PixelCrew.UI.LevelsLoader;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.UI.Windows.MainMenu
{
    public class MainMenuWindow : AnimatedWindow
    {
        protected Action CloseAction;

        public void OnStartGame()
        {
            CloseAction = () =>
            {
                var loader = FindObjectOfType<LevelLoader>();
                loader.LoadLevel("Level_0");
            };
            Close();
        }

        public virtual void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/Windows/Settings/SettingsWindow");
        }

        public void OnShowLocales()
        {
            WindowUtils.CreateWindow("UI/Windows/Localization/LocalizationWindow");
        }

        public void OnExit()
        {
            CloseAction = () =>
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };
            Close();
        }

        protected override void OnCloseAnimationComplete()
        {
            CloseAction?.Invoke();
            base.OnCloseAnimationComplete();
        }
    }
}