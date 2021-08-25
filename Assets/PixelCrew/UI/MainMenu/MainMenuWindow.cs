using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.MainMenu
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
            var window = Resources.Load<GameObject>("UI/Settings/SettingsWindow");
            var canvas = FindObjectOfType<Canvas>();
            Instantiate(window, canvas.transform);
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
    }
}

