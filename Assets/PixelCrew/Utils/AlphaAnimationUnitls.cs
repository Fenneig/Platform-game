using System.Collections;
using UnityEngine;

namespace PixelCrew.Utils
{
    public static class AlphaAnimationUtils
    {
        public static IEnumerator AlphaAnimation(SpriteRenderer sprite, float destAlpha, float destTime)
        {
            var alphaTime = 0f;
            var spriteAlpha = sprite.color.a;
            while (alphaTime < destTime)
            {
                alphaTime += Time.deltaTime;
                var progress = alphaTime / destTime;
                var tempAlpha = Mathf.Lerp(spriteAlpha, destAlpha, progress);
                var color = sprite.color;
                color.a = tempAlpha;
                sprite.color = color;

                yield return null;
            }
        }
    }
}
