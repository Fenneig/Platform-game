using System.Collections;
using UnityEngine;
using PixelCrew.Components.Health;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Model;
using UnityEditor.Animations;
using PixelCrew.Utils;

namespace PixelCrew.Creatures.Hero
{
    [RequireComponent(typeof(HealthComponent))]
    public class Hero : Creature
    {
        //Скрипт персонажа, содержащий основные механики взаимодействия с персонажем
        //переменные настраиваемые из Unity
        [Space]
        [Header("Stats")]
        [SerializeField] private float _dashSpeed;
        [SerializeField] private float _dashDuratation;
        [Space]
        [Header("Checkers")]
        [SerializeField] private CheckCircleOverlap _interactionRadius;

        [Space]
        [Header("Controllers")]
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;

        [Space]
        [Header("Particle System")]
        [SerializeField] private ParticleSystem _dropCoinsOnHitParticle;

        [Space]
        [Header("Throw Stats")]
        [Min(2)] [SerializeField] private int _numberOfThrows;
        [SerializeField] private Timer _throwCooldown;
        [SerializeField] private Timer _throwChargeTime;
        [SerializeField] private float _timeBetweenChargedThrows;

        private int _throwedCount;
        private float _dashTrigger;
        private bool _allowDashInJump;
        private bool _isDashing;
        private bool _isJumpButtonPressed;
        private bool _allowDoubleJump;

        private HealthComponent _healthComponent;

        protected static readonly int ThrowKey = Animator.StringToHash("is-throw");

        private GameSession _session;

        public bool IsJumpButtonPressed
        {
            get => _isJumpButtonPressed;
            set => _isJumpButtonPressed = value;
        }

        public float DashTrigger
        {
            get => _dashTrigger;
            set => _dashTrigger = value;
        }

        protected override void Awake()
        {
            base.Awake();
            _allowDashInJump = true;
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();

            _healthComponent = GetComponent<HealthComponent>();

            _healthComponent.Health = _session.Data.Hp;

            UpdateHeroWeapon();
        }

        public void OnHealthChanged()
        {
            _session.Data.Hp = _healthComponent.Health;

            SayHp();
        }

        public void CollectCoin(int value)
        {
            _session.Data.Coins += value;
            SayCoins();
        }

        public void CollectSword(int value)
        {
            _session.Data.Swords += value;
            SaySwords();
        }

        public void SayCoins() => Debug.Log($"I have {_session.Data.Coins} coins!");

        public void SaySwords() => Debug.Log($"I have {_session.Data.Swords} swords!");

        public void SayHp() => Debug.Log($"I have {_session.Data.Hp} hp now!");

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (IsGrounded)
            {
                _allowDoubleJump = true;
                _allowDashInJump = true;
            }

            if (DashTrigger != 0)
            {
                if (AllowDash())
                {
                    StartCoroutine(Dash());
                }
                DashTrigger = 0f;
            }
        }

        //При рывке отключается просчет движения героя 
        protected override Vector2 CalculateVelocity()
        {
            if (!_isDashing)
            {
                return base.CalculateVelocity();
            }

            return Rigidbody.velocity;
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded && !IsJumpButtonPressed)
            {
                Particles.Spawn("Jump");
                yVelocity = JumpSpeed;
                IsJumpButtonPressed = true;
            }
            else if (_allowDoubleJump && !IsJumpButtonPressed)
            {
                Particles.Spawn("Jump");
                yVelocity = JumpSpeed;
                _allowDoubleJump = false;
                IsJumpButtonPressed = true;
            }
            return yVelocity;
        }

        //Механика рывка: при нажатии рывка отключаю гравитацию действующую на героя,
        //добавляю импульс в направлении движения героя, жду время рывка и включаю гравитацию обратно
        private IEnumerator Dash()
        {
            Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
            Rigidbody.freezeRotation = true;
            UpdateSpriteDirection();
            _isDashing = true;
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0f);
            Rigidbody.AddForce(new Vector2(_dashSpeed * transform.localScale.x, 0f), ForceMode2D.Impulse);
            Particles.Spawn("Dash");
            yield return new WaitForSeconds(_dashDuratation);
            _isDashing = false;
            Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private bool AllowDash()
        {
            if (_allowDashInJump)
            {
                _allowDashInJump = false;
                return true;
            }
            return _allowDashInJump;
        }

        public void RecoverExtraMoves()
        {
            _allowDashInJump = true;
            _allowDoubleJump = true;
        }

        public override void TakeDamage()
        {
            base.TakeDamage();

            OnHealthChanged();

            if (_session.Data.Coins > 0)
                SpawnCoins();
        }

        private void SpawnCoins()
        {
            var coinsToSpawn = Mathf.Min(_session.Data.Coins, 5);
            _session.Data.Coins -= coinsToSpawn;

            var burst = _dropCoinsOnHitParticle.emission.GetBurst(0);
            burst.count = coinsToSpawn;
            _dropCoinsOnHitParticle.emission.SetBurst(0, burst);

            _dropCoinsOnHitParticle.gameObject.SetActive(true);
            _dropCoinsOnHitParticle.Play();
        }

        public void Interact()
        {
            _interactionRadius.Check();
        }

        public override void Attack()
        {
            if (!_session.Data.IsArmed) return;

            base.Attack();
        }

        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            UpdateHeroWeapon();
        }

        public void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _unarmed;
        }

        public void ThrowPushed()
        {
            if (!_session.Data.IsArmed) return;
            _throwChargeTime.Reset();
        }

        public void ThrowReleased()
        {
            if (!_session.Data.IsArmed) return;

            if (_throwCooldown.IsReady && _session.Data.Swords > 1)
            {
                _throwCooldown.Reset();

                if (_throwChargeTime.IsReady)
                {
                    _throwedCount = 0;
                    int numberOfThrows = _numberOfThrows >= _session.Data.Swords ? _session.Data.Swords - 1 : _numberOfThrows;
                    StartCoroutine(ThrowMultiply(numberOfThrows));
                }
                else
                {
                    Throw();
                }
            }
        }

        private IEnumerator ThrowMultiply(int numberOfThrows)
        {
            while (_throwedCount < numberOfThrows)
            {
                Throw();
                _throwedCount++;
                yield return new WaitForSeconds(_timeBetweenChargedThrows);
            }
        }

        public void Throw()
        {
            Animator.SetTrigger(ThrowKey);

            _session.Data.Swords -= 1;

            SaySwords();
        }

        public void OnDoThrow()
        {
            Particles.Spawn("Throw");
        }

    }
}