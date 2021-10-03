using Assets.PixelCrew.Model.Data.Dialog;
using PixelCrew.Components.Dialogue;
using PixelCrew.Model.Definitions.Localization;
using UnityEngine;

namespace PixelCrew.UI.Localization
{
    [RequireComponent(typeof(ShowDialogueComponent))]
    public class LocalizeDialogue : MonoBehaviour
    {
        private Sentence[] _sentences;
        private ShowDialogueComponent _dialogueComponent;

        private void Awake()
        {
            _dialogueComponent = GetComponent<ShowDialogueComponent>();

            LocalizationManager.I.OnLocaleChanged += Localize;
            Localize();
        }

        public void Localize()
        {
            _sentences = _dialogueComponent.Data.Sentences;
            foreach (var sentence in _sentences)
            {
                var localizedLine = LocalizationManager.I.Localize(sentence.Key);
                sentence.Line = localizedLine;
            }
        }

        private void OnDestroy()
        {
            LocalizationManager.I.OnLocaleChanged -= Localize;
        }
    }
}