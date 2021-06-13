using PixelCrew.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destTransform;
        [SerializeField] private float _alphaTime = 1f;
        [SerializeField] private float _moveTime = 1f;

        public void Teleport(GameObject target)
        {
            StartCoroutine(AnimateTeleport(target));
        }

        private IEnumerator AnimateTeleport(GameObject target)
        {
            var sprite = target.GetComponent<SpriteRenderer>();
            var input = target.GetComponent<PlayerInput>();
            var collider = target.GetComponent<Collider2D>();

            yield return AlphaAnimation(sprite, 0f);
            SetLockInput(input, true);
            collider.enabled = false;

            yield return StartCoroutine(MoveAnimation.MoveToTarget(target, _destTransform.position, _moveTime));

            collider.enabled = true;
            SetLockInput(input, false);
            yield return AlphaAnimation(sprite, 1f);
        }

        private void SetLockInput(PlayerInput input, bool isLocked)
        {
            if (input != null)
            {
                input.enabled = !isLocked;
            }
        }

        private IEnumerator AlphaAnimation(SpriteRenderer sprite, float destAlpha)
        {
            var alphaTime = 0f;
            var spriteAlpha = sprite.color.a;
            while (alphaTime < _alphaTime)
            {
                alphaTime += Time.deltaTime;
                var progress = alphaTime / _alphaTime;
                var tempAlpha = Mathf.Lerp(spriteAlpha, destAlpha, progress);
                var color = sprite.color;
                color.a = tempAlpha;
                sprite.color = color;

                yield return null;
            }

        }
    }
}