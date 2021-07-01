using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Patroling
{
    public class PointPatrol : Patrol
    {
        [SerializeField] Transform[] _points;
        [SerializeField] float _threshold = 0.5f;

        private Creature _creature;
        private int _destinationPointIndex;

        private void Awake()
        {
            _creature = GetComponent<Creature>();
        }
        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                if (IsOnPatrol())
                {
                    _destinationPointIndex = (int)Mathf.Repeat(_destinationPointIndex + 1, _points.Length);
                }

                var direction = _points[_destinationPointIndex].position - transform.position;
                direction.y = 0;
                _creature.Direction = direction.normalized;

                yield return null;
            }
        }

        private bool IsOnPatrol()
        {
            return (_points[_destinationPointIndex].position - transform.position).magnitude < _threshold;
        }
    }
}
