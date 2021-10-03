using Assets.PixelCrew.Model.Data.Dialog;
using System;
using UnityEngine;

namespace PixelCrew.Model.Data.Dialogue
{
    [Serializable]
    public class DialogueData
    {
        [SerializeField] private Sentence[] _sentences;
        public Sentence[] Sentences => _sentences;
    }
}