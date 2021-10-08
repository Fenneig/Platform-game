using PixelCrew.UI.Windows.Settings;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.UI.Windows.Pause
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
                var otherMenuOpened = FindObjectOfType<AnimatedWindow>();
                if (otherMenuOpened != null) 
                {
                    otherMenuOpened.Close(); 
                }
                else
                {
                    inGamePauseMenuWindow.OnResumeGame();
                    _isPaused = false;
                }
            }
            else
            {
                WindowUtils.CreateWindow("UI/Windows/Pause/PauseMenuWindow");
                
                _isPaused = true;
                
                Time.timeScale = 0f;
            }
        }
    }
}