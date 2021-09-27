using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace PixelCrew.Model.Definitions.Localization
{
    [CreateAssetMenu(menuName = "Defs/LocaleDef", fileName = "LocaleDef")]
    public class LocaleDef : ScriptableObject
    {
        [SerializeField] private string _url;
        [Tooltip("File must be in folder Assets/Resources/locales/")]
        [SerializeField] private string _localeName;
        [SerializeField] private List<LocaleItem> _localeItems;

        private UnityWebRequest _request;

        public Dictionary<string, string> GetData() =>
            _localeItems.ToDictionary(localeItem => localeItem.Key, localeItem => localeItem.Value);

        [ContextMenu("Update locale")]
        private void LoadLocale()
        {
            if (_request != null) return;

            _request = UnityWebRequest.Get(_url);
            _request.SendWebRequest().completed += OnDataLoaded;
        }

        [ContextMenu("Update locale from file")]
        private void LoadLocaleFromFile()
        {
            var reader = new StreamReader($"Assets/Resources/locales/{_localeName}.tsv");

            SplitText(reader.ReadToEnd());
        }

        private void OnDataLoaded(AsyncOperation operation)
        {
            if (!operation.isDone) return;

            SplitText(_request.downloadHandler.text);
        }

        private void SplitText(string text)
        {
            var rows = text.Split('\n');
            _localeItems.Clear();
            foreach (var row in rows) AddLocaleItem(row);
        }

        private void AddLocaleItem(string row)
        {
            try
            {
                var parts = row.Split('\t');
                _localeItems.Add(new LocaleItem {Key = parts[0], Value = parts[1]});
            }
            catch (Exception e)
            {
                Debug.Log($"Can't parse row: {row}. \n {e}");
            }
        }

        [Serializable]
        private class LocaleItem
        {
            public string Key;
            public string Value;
        }
    }
}