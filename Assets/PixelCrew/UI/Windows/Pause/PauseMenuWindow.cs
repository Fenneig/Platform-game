using PixelCrew.UI.Windows.MainMenu;
using PixelCrew.Components.LevelManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.UI.Windows.PauseMenu
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
        }

        public void OnResumeGame()
        {
            Time.timeScale = _defaultTimeScale;
            _input.enabled = true;
            Close();
        }

        public void OnRestartLevel()
        {
            Time.timeScale = _defaultTimeScale;
            _input.enabled = true;
            var reloadScene = FindObjectOfType<ReloadLevelComponent>();
            _closeAction = () => reloadScene.ReloadScene();
            Close();
        }

        public override void OnShowSettings()
        {
            Time.timeScale = 0;
            _input.enabled = false;
            base.OnShowSettings();
        }
    }
}