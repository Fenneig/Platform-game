using PixelCrew.Components.Dialogue;
using PixelCrew.Model.Data.Dialogue;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.Localization
{
    [RequireComponent(typeof(ShowDialogueComponent))]
    public class LocalizeDialogue : MonoBehaviour
    {
        private Sentence[] _sentences;
        private ShowDialogueComponent _dialogueComponent;

        private readonly CompositeDisposable _trash = new CompositeDisposable();


        private void Awake()
        {
            _dialogueComponent = GetComponent<ShowDialogueComponent>();

            _trash.Retain(LocalizationManager.I.Subscribe(Localize));
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

            _dialogueComponent.Data.Font = LocalizationManager.I.SetFont();
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}