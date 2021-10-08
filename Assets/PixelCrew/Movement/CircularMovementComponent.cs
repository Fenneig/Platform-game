using PixelCrew.Components.GOBased;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Movement
{
    [RequireComponent(typeof(DestroyObjectComponent))]
    public class CircularMovementComponent : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private float _speed;

        private Rigidbody2D[] _childRigidBody;
        private float[] _startAngle;
        private int _childCount;
        private bool _isPlaying;

        private void Awake()
        {
            _isPlaying = true;
            _childCount = transform.childCount;
            _childRigidBody = new Rigidbody2D[_childCount];
            _startAngle = new float[_childCount];
            for (int i = 0; i < _childCount; i++)
            {
                _startAngle[i] = 2 * Mathf.PI / _childCount * i;
                _childRigidBody[i] = transform.GetChild(i).GetComponent<Rigidbody2D>();
            }
        }

        private void Update()
        {
            for (int i = 0; i < _childCount; i++)
            {
                if (_childRigidBody[i] == null) continue;

                var postion = _childRigidBody[i].position;
                postion.x = transform.position.x + Mathf.Cos(_startAngle[i] + Time.time * _speed) * _radius;
                postion.y = transform.position.y + Mathf.Sin(_startAngle[i] + Time.time * _speed) * _radius;

                _childRigidBody[i].MovePosition(postion);
            }

            if (transform.childCount == 0) GetComponent<DestroyObjectComponent>().DestroyObject();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = Color.white;
            Handles.DrawWireDisc(transform.position, Vector3.forward, _radius);

            if (!_isPlaying)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    var angle = 2 * Mathf.PI / transform.childCount * i;

                    transform.GetChild(i).position = new Vector3(Mathf.Cos(angle) * _radius + transform.position.x,
                                                                 Mathf.Sin(angle) * _radius + transform.position.y,
                                                                 0);
                }
            }
        }
#endif

    }
}