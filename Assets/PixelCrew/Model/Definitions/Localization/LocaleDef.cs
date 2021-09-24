using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace PixelCrew.Model.Definitions.Localization
{
    [CreateAssetMenu(menuName = "Defs/LocaleDef", fileName = "LocaleDef")]
    public class LocaleDef : ScriptableObject
    {
        [SerializeField] private string _url;
        [SerializeField] private List<LocaleItem> _localeItems;

        private UnityWebRequest _request;

        public Dictionary<string, string> GetData()
        {
            return _localeItems.ToDictionary(localeItem => localeItem.Key, localeItem => localeItem.Value);
        }

        [ContextMenu("Update locale")]
        private void LoadLocale()
        {
            if (_request != null) return;
            
            _request = UnityWebRequest.Get(_url);
            _request.SendWebRequest().completed += OnDataLoaded;
        }

        private void OnDataLoaded(AsyncOperation operation)
        {
            if (!operation.isDone) return;
            
            var rows = _request.downloadHandler.text.Split('\n');
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