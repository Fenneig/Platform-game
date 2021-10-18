using System;
using UnityEngine;

namespace PixelCrew.Model.Data.Dialogue
{
    [Serializable]
    public class Sentence
    {
        [SerializeField] private string _line;
        [SerializeField] private Sprite _portrait;
        [SerializeField] private bool _isLeft;
        [SerializeField] private string _key;

        public string Line
        {
            get => _line;
            set => _line = value;
        }

        public Sprite Portrait => _portrait;
        public bool IsLeft => _isLeft;
        public string Key => _key;
    }
}