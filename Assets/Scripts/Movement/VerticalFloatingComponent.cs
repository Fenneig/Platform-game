using UnityEngine;

namespace PixelCrew.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class VerticalFloatingComponent : MonoBehaviour
    {
        [SerializeField] float _frequency = 1f;
        [SerializeField] float _amplitude = 1f;
        [SerializeField] bool _randomize;
        [SerializeField] bool _isFloating = true;
        private float _originalY;
        private Rigidbody2D _rigidbody;
        private float _seed = 1f;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _originalY = _rigidbody.position.y;
            if (_randomize) _seed = Random.value * Mathf.PI * 2;
        }

        private void Update()
        {
            if (!_isFloating) return;

            var position = _rigidbody.position;
            position.y = _originalY + Mathf.Sin(_seed + Time.time * _frequency) * _amplitude;
            _rigidbody.MovePosition(position);
        }

    }
}