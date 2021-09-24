using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.Windows.MainMenu
{
    public class MainMenuWindow : AnimatedWindow
    {
        protected Action _closeAction;

        public void OnStartGame()
        {
            _closeAction = () => { SceneManager.LoadScene("Level_0"); };
            Close();
        }

        public virtual void OnShowSettings()
        {
            CreateWindow("UI/Settings/SettingsWindow");
        }

        public void OnShowLocales()
        {
            CreateWindow("UI/Windows/Localization/LocalizationWindow");
        }

        public void OnExit()
        {
            _closeAction = () =>
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };
            Close();
        }

        public override void OnCloseAnimationComplete()
        {
            _closeAction?.Invoke();
            base.OnCloseAnimationComplete();
        }

        public void CreateWindow(string windowPath)
        {
            var window = Resources.Load<GameObject>(windowPath);
            var canvas = GetComponentInParent<Canvas>();
            Instantiate(window, canvas.transform);
            
        }
    }
}