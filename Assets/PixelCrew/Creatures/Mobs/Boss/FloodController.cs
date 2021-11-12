using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss
{
    public class FloodController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _floodTime;
        private static readonly int IsFlooded = Animator.StringToHash("IsFlooded");

        private Coroutine _coroutine;

        public void StartFlood()
        {
            if (_coroutine != null) return;
            _coroutine = StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            _animator.SetBool(IsFlooded,true);
            yield return new WaitForSeconds(_floodTime);
            _animator.SetBool(IsFlooded,false);
            _coroutine = null;
        }
    }
}