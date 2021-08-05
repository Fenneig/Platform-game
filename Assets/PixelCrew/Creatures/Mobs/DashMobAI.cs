using System.Collections;
using UnityEngine;


namespace PixelCrew.Creatures.Mobs
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DashMobAI : MobAI
    {
        [SerializeField] private AnimationCurve _yAnimation;
        [SerializeField] private float _dashSpeed;
        [SerializeField] private float _jumpHeight = 0.6f;

        private float _dashDuratation;
        private Vector3 _startAttackPosition;
        private Vector3 _targetAttackPosition;
        private float _timeElapsed;
        private Rigidbody2D _rigidbody;

        private static readonly int IsAttacking = Animator.StringToHash("is-attacking");

        protected override void Awake()
        {
            base.Awake();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public override void OnHeroInVision(GameObject go)
        {
            if (IsDead) return;

            _yAnimation = new AnimationCurve();

            _startAttackPosition = transform.position;
            _targetAttackPosition = go.transform.position;

            var distance = Mathf.Abs(_targetAttackPosition.x - _startAttackPosition.x);

            _dashDuratation = distance / _dashSpeed;

            var lastFrameHeight = _targetAttackPosition.y - _startAttackPosition.y;

            _yAnimation.AddKey(0, 0);
            _yAnimation.AddKey(distance / 2, lastFrameHeight + _jumpHeight);
            _yAnimation.AddKey(distance, lastFrameHeight);

            _timeElapsed = 0;
            StartState(AgroToHero());
        }

        protected override IEnumerator AgroToHero()
        {
            Particles.Spawn("Exclamation");
            yield return new WaitForSeconds(AlarmDelay);

            MobAnimator.SetBool(IsAttacking, true);

            Mob.Attack();
        }

        public void OnDoJump() 
        {
            StartState(Jump());
        }

        private IEnumerator Jump()
        {
            while (MobAnimator.GetBool(IsAttacking))
            {
                _timeElapsed += Time.deltaTime;

                var newXPos = Mathf.Lerp(_startAttackPosition.x, _targetAttackPosition.x, _timeElapsed / _dashDuratation);
                var newYPos = _startAttackPosition.y + _yAnimation.Evaluate(_timeElapsed / _dashDuratation);

                _rigidbody.MovePosition(new Vector2(newXPos, newYPos));


                if (_timeElapsed >= _dashDuratation) MobAnimator.SetBool(IsAttacking, false);

                yield return null;
            }

            yield return new WaitForSeconds(AttackCooldown);
            StartState(Patrol?.DoPatrol());
        }

    }
}