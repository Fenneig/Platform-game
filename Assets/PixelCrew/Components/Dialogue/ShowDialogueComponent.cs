using PixelCrew.Model.Definitions;
using PixelCrew.Model.Data.Dialogue;
using PixelCrew.UI.HUD.Dialog;
using System;
using PixelCrew.Effects;
using PixelCrew.UI.Localization;
using UnityEngine;

namespace PixelCrew.Components.Dialogue
{
    public class ShowDialogueComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogueData _bound;
        [SerializeField] private DialogueDef _external;

        private DialogBoxController _dialogBox;
        private VignetteEffect _vignette;

        private void Start()
        {
            _vignette = FindObjectOfType<VignetteEffect>();
        }

        public void Show()
        {
            if (_dialogBox == null) _dialogBox = FindObjectOfType<DialogBoxController>();
            GetComponent<LocalizeDialogue>()?.Localize();
            _dialogBox.ShowDialogue(Data);
            _dialogBox.OnComplete += HideVignette;
            _vignette.ShowVignette();
        }

        private void HideVignette()
        {
            _vignette.HideVignette();
            _dialogBox.OnComplete -= HideVignette;
        }

        public void Show(DialogueDef def)
        {
            _external = def;
            Show();
        }

        public DialogueData Data
        {
            get
            {
                switch (_mode)
                {
                    case Mode.Bound:
                        return _bound;
                    case Mode.External:
                        return _external.Data;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        public enum Mode
        {
            Bound,
            External
        }
    }
}