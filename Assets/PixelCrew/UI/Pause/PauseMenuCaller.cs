using PixelCrew.UI.Settings;
using UnityEngine;

namespace PixelCrew.UI.PauseMenu
{
    public class PauseMenuCaller : MonoBehaviour
    {
        private bool _isPaused;
        

        public void SwitchPauseCondition()
        {
            var inGamePauseMenuWindow = FindObjectOfType<PauseMenuWindow>();
            if (inGamePauseMenuWindow == null) _isPaused = false;

            if (_isPaused)
            {
                var settingsMenuWindow = FindObjectOfType<SettingsMenuWindow>();
                if (settingsMenuWindow != null) 
                {
                    settingsMenuWindow.Close(); 
                }
                else
                {
                    inGamePauseMenuWindow.OnResumeGame();
                    _isPaused = false;
                }
            }
            else
            {
                var window = Resources.Load<GameObject>("UI/Pause/PauseMenuWindow");
                var canvas = GetComponent<Canvas>();
                Instantiate(window, canvas.transform);
                _isPaused = true;
                Time.timeScale = 0f;
            }
        }
    }
}