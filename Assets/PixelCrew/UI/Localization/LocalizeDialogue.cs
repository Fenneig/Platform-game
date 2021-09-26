using Assets.PixelCrew.Model.Data.Dialog;
using PixelCrew.Components.Dialog;
using PixelCrew.Model.Definitions.Localization;
using UnityEngine;

namespace PixelCrew.UI.Localization
{
    [RequireComponent(typeof(ShowDialogComponent))]
    public class LocalizeDialogue : MonoBehaviour
    {
        private Sentence[] _sentences;

        private ShowDialogComponent _dialogComponent;

        private void Awake()
        {
            _dialogComponent = GetComponent<ShowDialogComponent>();

            LocalizationManager.I.OnLocaleChanged += OnLocaleChanged;
            Localize();
        }
        private void OnLocaleChanged()
        {
            Localize();
        }

        public void Localize()
        {
            _sentences = _dialogComponent.Data.Sentences;
            foreach (var sentence in _sentences)
            {
                var localizedLine = LocalizationManager.I.Localize(sentence.Key);
                sentence.Line = localizedLine;
            }
        }

        private void OnDestroy()
        {
            LocalizationManager.I.OnLocaleChanged -= OnLocaleChanged;
        }
    }
}