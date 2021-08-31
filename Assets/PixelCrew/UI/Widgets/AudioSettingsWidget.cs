using PixelCrew.Model.Data.Properties;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class AudioSettingsWidget : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Text _value;

        private FloatPersistentProperty _model;

        private void Start()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChange);
        }

        public void OnSliderValueChange(float value) 
        {
            _model.Value = value;
        }

        public void SetModel(FloatPersistentProperty model) 
        {
            _model = model;
            _model.OnChanged += OnValueChanged;
            OnValueChanged(_model.Value, _model.Value);
        }

        private void OnValueChanged(float newValue, float oldValue)
        {
            var valueText = Mathf.Round(newValue * 100f);
            _value.text = valueText.ToString();
            _slider.normalizedValue = newValue;
        }

        private void OnDestroy()
        {
            _model.OnChanged -= OnValueChanged;
            _slider.onValueChanged.RemoveListener(OnSliderValueChange);
        }


    }
}