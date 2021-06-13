using PixelCrew.Utils;
using UnityEditor;
using UnityEngine;

namespace PixelCrew
{
    public class LayerCheck : MonoBehaviour
    {
        // Скрипт проверяет касается ли получаемый с прикрепленного к скрипту объекта коллайдер с слоями считающимися за "землю"
        [SerializeField] private LayerMask _groundLayer;
        private Collider2D _collider;
        private bool _isTouchingLayer;
        public bool IsTouchingGround() => _isTouchingLayer;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(_groundLayer);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(_groundLayer);
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (IsTouchingGround()) Handles.color = HandlesUtils.TransparentGreen;
            else Handles.color = HandlesUtils.TransparentRed;
            var offset = GetComponent<CircleCollider2D>().offset;
            var position = transform.position + new Vector3(offset.x, offset.y, 0);
            Handles.DrawSolidDisc(position, Vector3.forward, GetComponent<CircleCollider2D>().radius);
        }
#endif
    }
}