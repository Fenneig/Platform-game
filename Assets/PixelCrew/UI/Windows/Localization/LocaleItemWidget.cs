using System;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.UI.Widgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.Localization
{
    public class LocaleItemWidget : MonoBehaviour, IItemRenderer<LocaleInfo>
    {
        [SerializeField] private Text _text;
        [SerializeField] private GameObject _selector;
        [SerializeField] private SelectLocale _onSelected;

        private LocaleInfo _data;

        private void Awake()
        {
            LocalizationManager.I.OnLocaleChanged += UpdateSelection;
        }

        public void SetData(LocaleInfo localeInfo, int index)
        {
            _data = localeInfo;
            UpdateSelection();
            _text.text = _data.LocaleId.ToUpper();
        }
        
        private void UpdateSelection()
        {
            var isSelected = LocalizationManager.I.LocalKey == _data.LocaleId; 
            _selector.SetActive(isSelected);
        }
        
        public void OnSelected() => _onSelected?.Invoke(_data.LocaleId);

        private void OnDestroy()
        {
            LocalizationManager.I.OnLocaleChanged -= UpdateSelection;
        }
    }

    public class LocaleInfo
    {
        public string LocaleId;
    }

    [Serializable]
    public class SelectLocale : UnityEvent<string>
    {
    }
}