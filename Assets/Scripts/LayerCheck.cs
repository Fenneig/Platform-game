using System.Collections;
using System.Collections.Generic;
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
    }
}