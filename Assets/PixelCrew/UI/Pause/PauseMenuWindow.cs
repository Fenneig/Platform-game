using PixelCrew.UI.MainMenu;
using PixelCrew.Components.LevelManagement;
using UnityEngine;

namespace PixelCrew.UI.PauseMenu
{
    public class PauseMenuWindow : MainMenuWindow
    {
        public void OnResumeGame()
        {
            Time.timeScale = 1f;
            Close();
        }

        public void OnRestartLevel()
        {
            Time.timeScale = 1f;
            var reloadScene = FindObjectOfType<ReloadLevelComponent>();
            _closeAction = () => reloadScene.ReloadScene();
            Close();
        }
        //Анимации не проигрываются, так как я ставлю Time.timeScale = 0 во время паузы. Для этого я создал отдельное SettingsWindow без анимаций.
        public override void OnShowSettings()
        {
            var window = Resources.Load<GameObject>("UI/Pause/PauseSettingsWindow");
            var canvas = GetComponentInParent<Canvas>();
            Instantiate(window, canvas.transform);
        } 
    }
}