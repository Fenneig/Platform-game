using PixelCrew.UI.Settings;
using UnityEngine;

namespace PixelCrew.UI.Windows.PauseMenu
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
                var window = Resources.Load<GameObject>("UI/Windows/Pause/PauseMenuWindow");
                var canvasTransform = GetComponent<Canvas>().transform;
                Instantiate(window, canvasTransform);
                _isPaused = true;
                
                Time.timeScale = 0f;
            }
        }
    }
}