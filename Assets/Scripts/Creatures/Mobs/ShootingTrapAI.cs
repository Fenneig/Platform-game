using UnityEngine;
using PixelCrew.Utils;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GOBased;

namespace PixelCrew.Creatures.Mobs
{
    class ShootingTrapAI : MonoBehaviour
    {

        [SerializeField] private LayerCheck _vision;
        [SerializeField] private Timer _attackCooldown;

        [Header("Melee")]
        [Space]
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private LayerCheck _meleeCanAttack;

        [Header("Range")]
        [Space]
        [SerializeField] private SpawnComponent _rangeAttack;

        private Animator _animator;

        protected static readonly int MeleeKey = Animator.StringToHash("melee");
        protected static readonly int RangeKey = Animator.StringToHash("range");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_vision.IsTouchingLayer)
            {
                if (_meleeCanAttack != null && _meleeCanAttack.IsTouchingLayer)
                {
                    if (_attackCooldown.IsReady)
                    {
                        MeleeAttack();
                    }

                    return;
                }

                if (_attackCooldown.IsReady)
                {
                    RangeAttack();
                }
            }
        }

        private void MeleeAttack()
        {
            _animator.SetTrigger(MeleeKey);
            _attackCooldown.Reset();
        }

        private void RangeAttack()
        {
            _animator.SetTrigger(RangeKey);
            _attackCooldown.Reset();
        }

        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
        }

        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }

        public void OnRecieveHit() 
        {
            _attackCooldown.EarlyComplete();
        }

    }
}
