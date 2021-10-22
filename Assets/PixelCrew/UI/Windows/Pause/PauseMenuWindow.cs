using PixelCrew.Components.LevelManagement;
using PixelCrew.UI.Windows.MainMenu;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.UI.Windows.Pause
{
    public class PauseMenuWindow : MainMenuWindow
    {
        private PlayerInput _input;
        private float _defaultTimeScale;

        private void Awake()
        {
            _input = FindObjectOfType<PlayerInput>();
            _input.enabled = false;
            _defaultTimeScale = Time.timeScale;
            PauseGame();
        }

        public void OnResumeGame()
        {
            Close();
        }

        public void OnRestartLevel()
        {
            ResumeGame();
            var reloadScene = FindObjectOfType<ReloadLevelComponent>();
            CloseAction = () => reloadScene.ReloadScene();
            Close();
        }

        private void PauseGame()
        {
            Time.timeScale = 0f;
            _input.enabled = false;
        }

        private void ResumeGame()
        {
            Time.timeScale = _defaultTimeScale;
            _input.enabled = true;
        }

        protected override void OnCloseAnimationComplete()
        {
            ResumeGame();
            base.OnCloseAnimationComplete();
        }
    }
    
}