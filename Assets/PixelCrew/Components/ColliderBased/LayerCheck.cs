using PixelCrew.Utils;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Components.ColliderBased
{
    public class LayerCheck : MonoBehaviour
    {
        //Скрипт проверяет касается ли данный объект с указанными слоями
        [SerializeField] protected LayerMask _layer;
        [SerializeField] protected bool _isTouchingLayer;

        private Collider2D _collider;
        public bool IsTouchingLayer => _isTouchingLayer;

        private void Start()
        {
            _collider = GetComponent<Collider2D>();
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_collider != null) _isTouchingLayer = _collider.IsTouchingLayers(_layer);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_collider != null) _isTouchingLayer = _collider.IsTouchingLayers(_layer);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var circleCollider = GetComponent<CircleCollider2D>();
            if (circleCollider == null) return;
            Handles.color = IsTouchingLayer ? HandlesUtils.TransparentGreen : HandlesUtils.TransparentRed;
            var offset = GetComponent<CircleCollider2D>().offset;
            var position = transform.position + new Vector3(offset.x, offset.y, 0);
            Handles.DrawSolidDisc(position, Vector3.forward, GetComponent<CircleCollider2D>().radius);
        }
#endif
    }
}