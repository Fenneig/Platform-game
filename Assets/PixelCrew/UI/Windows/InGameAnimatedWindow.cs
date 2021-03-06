using PixelCrew.Effects;
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
        private VignetteEffect _vignette;


        protected override void Start()
        {
            Session = GameSession.Instance;
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
            _vignette = FindObjectOfType<VignetteEffect>();
            _vignette.ShowVignette();
            Time.timeScale = 0f;
            _input.enabled = false;
        }

        private void ResumeGame()
        {
            _vignette.HideVignette();
            Time.timeScale = _defaultTimeScale;
            _input.enabled = true;
        }
    }
}