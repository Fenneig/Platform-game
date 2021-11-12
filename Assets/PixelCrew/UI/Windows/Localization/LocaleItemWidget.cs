using System;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
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

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private LocaleInfo _data;

        private void Awake()
        {
            _trash.Retain(LocalizationManager.I.Subscribe(UpdateSelection));
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
            _trash.Dispose();
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