using PixelCrew.Utils;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.ColliderBased
{

    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _range = 0.2f;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private OnOverlapEvent _onOverlap;

        private readonly Collider2D[] _interactionResult = new Collider2D[20];

        public void Check()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _range,
                _interactionResult,
                _mask);

            for (var i = 0; i < size; i++)
                _onOverlap?.Invoke(_interactionResult[i].gameObject);

        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _range);
        }
#endif
        [Serializable]
        public class OnOverlapEvent : UnityEvent<GameObject> { }
    }
}