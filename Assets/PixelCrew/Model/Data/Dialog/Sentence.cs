using System;
using UnityEngine;

namespace Assets.PixelCrew.Model.Data.Dialog
{
    [Serializable]
    public class Sentence
    {
        [SerializeField] private string _line;
        [SerializeField] private Sprite _portrait;
        [SerializeField] private bool _isHero;
        [SerializeField] private string _key;

        public string Line
        {
            get => _line;
            set => _line = value;
        }

        public Sprite Portrait => _portrait;
        public bool IsHero => _isHero;
        public string Key => _key;
    }
}