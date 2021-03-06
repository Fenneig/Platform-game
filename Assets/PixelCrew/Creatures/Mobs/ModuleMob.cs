using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ModuleMob : MonoBehaviour
    {
        [SerializeField] private GameObject _head;
        [SerializeField] private GameObject _body;
        [SerializeField] [Min(1)] private int _bodyLength = 1;
        [SerializeField] private float _positionDifference = 0.685f;

        private Vector3 _nextPosition;
        private GameObject[] _moduleInstances;

        private void Awake()
        {
            GetComponent<SpriteRenderer>().enabled = false;

            _nextPosition = gameObject.transform.position;
            _moduleInstances = new GameObject[_bodyLength];

            for (var i = 0; i < _bodyLength - 1; i++)
            {                
                _moduleInstances[i] = Instantiate(_body, _nextPosition, Quaternion.identity, gameObject.transform);
                _moduleInstances[i].name = $"{gameObject.name}_{_body.name}_{i}"; 
                _nextPosition.y += _positionDifference;
            }
            _moduleInstances[_bodyLength - 1] = Instantiate(_head, _nextPosition, Quaternion.identity, gameObject.transform);
            _moduleInstances[_bodyLength - 1].name = $"{gameObject.name}_{_head.name}";
        }

    }
}