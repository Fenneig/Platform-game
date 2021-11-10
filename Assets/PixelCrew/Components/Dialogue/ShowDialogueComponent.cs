using PixelCrew.Model.Definitions;
using PixelCrew.Model.Data.Dialogue;
using PixelCrew.UI.HUD.Dialog;
using System;
using PixelCrew.Effects;
using PixelCrew.UI.Localization;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Dialogue
{
    public class ShowDialogueComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogueData _bound;
        [SerializeField] private DialogueDef _external;
        [SerializeField] private UnityEvent _onComplete;

        private DialogBoxController _dialogBox;
        private VignetteEffect _vignette;

        private void Start()
        {
            _vignette = FindObjectOfType<VignetteEffect>();
            _dialogBox = FindObjectOfType<DialogBoxController>();
        }

        public void Show()
        {
            _dialogBox.OnComplete += OnComplete;
            GetComponent<LocalizeDialogue>()?.Localize();
            _dialogBox.ShowDialogue(Data);
            _vignette.ShowVignette();
        }

        private void OnComplete()
        {
            _onComplete?.Invoke();
            _vignette.HideVignette();
        }

        private void OnDestroy()
        {
            _dialogBox.OnComplete -= OnComplete;
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