using Assets.PixelCrew.Model.Data.Dialog;
using System;
using UnityEngine;

namespace PixelCrew.Model.Data.Dialog
{
    [Serializable]
    public class DialogData
    {
        [SerializeField] private Sentence[] _sentences;
        public Sentence[] Sentences => _sentences;
    }
}