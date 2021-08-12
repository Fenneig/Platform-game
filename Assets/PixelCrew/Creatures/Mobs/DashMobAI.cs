using System;
using System.Collections;
using UnityEngine;


namespace PixelCrew.Creatures.Mobs
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DashMobAI : MobAI
    {
        [SerializeField] private float _dashSpeed;
        [SerializeField] private float _jumpHeight = 0.6f;
        [SerializeField] private float _threshold = 0.5f;
        [SerializeField] Collider2D _attackCollider;

        private Vector3 _targetPosition;
        private float _baseMobSpeed;
        private bool _isFixedOnTarget;

        private static readonly int IsAttacking = Animator.StringToHash("is-attacking");

        protected override void Awake()
        {
            base.Awake();
            _baseMobSpeed = Mob.Speed;
            _isFixedOnTarget = false;
            _attackCollider.enabled = false;
        }

        public override void OnHeroInVision(GameObject go)
        {
            if (_isFixedOnTarget || IsDead) return;

            _targetPosition = go.transform.position;

            Target = go;

            StartState(AgroToHero());
        }

        protected override IEnumerator AgroToHero()
        {
            Particles.Spawn("Exclamation");
            yield return new WaitForSeconds(AlarmDelay);

            _isFixedOnTarget = true;

            Mob.Speed = _dashSpeed;
            MobAnimator.SetBool(IsAttacking, true);
            _attackCollider.enabled = true;

            Mob.Attack();
        }

        public void OnDoDash()
        {
            StartState(Dash());
        }

        private IEnumerator Dash()
        {
            SetDirectionToTarget();

            while (MobAnimator.GetBool(IsAttacking))
            {
                HeightReachedCheck();

                TargetReachedCheck();

                yield return null;
            }

            Mob.Speed = _baseMobSpeed;

            StopMoving();
            yield return new WaitForSeconds(AttackCooldown);

            _isFixedOnTarget = false;

            StartState(Patrol?.DoPatrol());
        }

        private void HeightReachedCheck()
        {
            if (transform.position.y >= _targetPosition.y + _jumpHeight)
            {
                var direction = Mob.Direction;
                direction.y = 0;
                Mob.Direction = direction;
            }
        }

        protected override void SetDirectionToTarget()
        {
            var direction = _targetPosition - transform.position;
            direction.y = 1;

            Mob.Direction = direction.normalized;
        }

        private void TargetReachedCheck()
        {
            if (Mathf.Abs(transform.position.x - _targetPosition.x) <= _threshold)
            {
                MobAnimator.SetBool(IsAttacking, false);
                _attackCollider.enabled = false;
            }

        }
    }
}