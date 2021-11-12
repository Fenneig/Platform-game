using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Utils.Disposables;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

namespace PixelCrew.UI.Localization
{
    [RequireComponent(typeof(Text))]
    public class LocalizeText : MonoBehaviour
    {
        [SerializeField] private string _key;
        [SerializeField] private bool _capitalize;
        private Text _text;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Awake()
        {
            _text = GetComponent<Text>();

            _trash.Retain(LocalizationManager.I.Subscribe(Localize));
            Localize();
        }

        private void Localize()
        {
            var localized = LocalizationManager.I.Localize(_key);
            _text.text = _capitalize ? localized.ToUpper() : localized;
            _text.font = LocalizationManager.I.SetFont();
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}