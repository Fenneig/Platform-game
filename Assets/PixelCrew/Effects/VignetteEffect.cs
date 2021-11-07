using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PixelCrew.Effects
{
    public class VignetteEffect : MonoBehaviour
    {
        [SerializeField] private float _dialogueIntensityLevel;
        private Volume _volume;

        private Vignette _vignette;

        private void Awake()
        {
            _volume = GetComponent<Volume>();

            if (_volume.profile.TryGet(out Vignette vignette))
                _vignette = vignette;
        }

        public void ShowVignette()
        {
            _vignette.intensity.value = _dialogueIntensityLevel;
        }

        public void HideVignette()
        {
            _vignette.intensity.value = 0;
        }
    }
}