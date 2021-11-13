using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss
{
    public class FloodController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private static readonly int IsFlooded = Animator.StringToHash("IsFlooded");


        public void StartFlood()
        {
            _animator.SetBool(IsFlooded, true);
        }

        public void StopFlood()
        {
            _animator.SetBool(IsFlooded, false);
        }
    }
}