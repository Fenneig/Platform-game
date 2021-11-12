using System.Collections;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Health
{
    public class InvulnerableEffectComponent : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private int _blinksAmount;
        
        public IEnumerator InvulnerableEffect(float invulnerableTime, SpriteRenderer blinkingSprite)
        {
            _health.IsInvulnerable.Retain(this);

            var color = blinkingSprite.color;

            for (var i = 0; i < _blinksAmount; i++)
            {
                yield return this.LerpAnimation(i % 2, (i + 1) % 2, invulnerableTime / _blinksAmount,
                    alpha => blinkingSprite.color = new Color(color.r, color.g, color.b, alpha));
            }

            color.a = 255;
            blinkingSprite.color = color;

            _health.IsInvulnerable.Release(this);
        }
    }
}