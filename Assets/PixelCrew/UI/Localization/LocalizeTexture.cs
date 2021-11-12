using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.Localization
{
    public class LocalizeTexture : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _trash.Retain(LocalizationManager.I.Subscribe(Localize));
            Localize();
        }

        private void Localize()
        {
            var key = LocalizationManager.I.LocalKey;
            _spriteRenderer.sprite =
                Resources.Load<Sprite>($"Textures/TexturesWithText/{key}/{_spriteRenderer.sprite.name}");
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}