using PixelCrew.Model;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.UI.Windows
{
    public class InGameAnimatedWindow : AnimatedWindow
    {
        private float _defaultTimeScale;
        private PlayerInput _input;
        protected GameSession Session;

        protected override void Start()
        {
            Session = FindObjectOfType<GameSession>();
            _defaultTimeScale = Time.timeScale;
            _input = FindObjectOfType<PlayerInput>();
            PauseGame();
            base.Start();
        }

        protected override void OnCloseAnimationComplete()
        {
            ResumeGame();
            base.OnCloseAnimationComplete();
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
    }
}