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

        public string Line => _line;
        public Sprite Portrait => _portrait;
        public bool IsHero => _isHero;
    }
}
