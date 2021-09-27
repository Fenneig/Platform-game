using PixelCrew.Model.Definitions.Localization;
using UnityEngine;

namespace PixelCrew.UI.Localization
{
    public class LocalizeTexture : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            LocalizationManager.I.OnLocaleChanged += Localize;
            Localize();
        }
        
        private void Localize()
        {
            var key = LocalizationManager.I.LocalKey;
            _spriteRenderer.sprite = Resources.Load<Sprite>($"Textures/TexturesWithText/{key}/{_spriteRenderer.sprite.name}");
        }

        private void OnDestroy()
        {
            LocalizationManager.I.OnLocaleChanged -= Localize;
        }
    }
}