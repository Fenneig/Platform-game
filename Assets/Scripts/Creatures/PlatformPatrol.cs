using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class PlatformPatrol : Patrol
    {
        [SerializeField] LayerCheck _endOfPlatformCheck;

        private Creature _creature;
        private Vector3 _lastFramePosition;

        private void Awake()
        {
            _creature = GetComponent<Creature>();
            _lastFramePosition = transform.position;
        }

        public override IEnumerator DoPatrol()
        {
            while (enabled) 
            {
                var direction = _endOfPlatformCheck.gameObject.transform.position - transform.position;
                direction.y = 0;

                if (IsNeedTurnBack()) direction.x *= -1;

                _creature.Direction = direction.normalized;

                yield return null;
            }
        }

        //Конец платформы определяется двумя способами: если существо уперлось в стену и не может пройти дальше
        //то его нужно развернуть, перед существом есть специальный индекатор проверяющий наличие пола перед ним
        //при отстутвии пола так же надо развернуть персонажа.
        private bool IsNeedTurnBack() 
        {
            if (_lastFramePosition == transform.position) return true;
            else _lastFramePosition = transform.position;
            return !_endOfPlatformCheck.IsTouchingLayer;
        }
    }
}
