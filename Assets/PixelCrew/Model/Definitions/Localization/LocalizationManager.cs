﻿using System;
using System.Collections.Generic;
using PixelCrew.Model.Data.Properties;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Localization
{
    public class LocalizationManager
    {
        public static readonly LocalizationManager I;

        private readonly StringPersistentProperty _localeKey = new StringPersistentProperty("en", "localization/current");
        private Dictionary<string, string> _localization;

        public string LocalKey => _localeKey.Value;
        public event Action OnLocaleChanged;

        static LocalizationManager()
        {
            I = new LocalizationManager();
        }

        private LocalizationManager()
        {
            LoadLocale(_localeKey.Value);
        }

        private void LoadLocale(string localeToLoad)
        {
            var def = Resources.Load<LocaleDef>($"Locales/{localeToLoad}");
            _localization = def.GetData();
            _localeKey.Value = localeToLoad;
            OnLocaleChanged?.Invoke();
        }

        public string Localize(string key)
        {
            return _localization.TryGetValue(key, out var value) ? value : $"%%%{key}%%%";
        }

        public Font SetFont()
        {
            return Resources.Load<Font>($"Fonts/{_localeKey.Value}");
        }


        public void SetLocale(string localeKey)
        {
            LoadLocale(localeKey);
        }
    }
}