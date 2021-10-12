using System.Collections;
using PixelCrew.Animations;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Components.ColliderBased
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

            yield return AlphaAnimationUtils.AlphaAnimation(sprite, 0f, _alphaTime);
            SetLockInput(input, true);
            collider.enabled = false;

            yield return StartCoroutine(MoveAnimation.MoveToTarget(target, _destTransform.position, _moveTime));

            SetLockInput(input, false);
            collider.enabled = true;
            yield return AlphaAnimationUtils.AlphaAnimation(sprite, 1f, _alphaTime);
        }

        private void SetLockInput(PlayerInput input, bool isLocked)
        {
            if (input != null)
            {
                input.enabled = !isLocked;
            }
        }
    }
}