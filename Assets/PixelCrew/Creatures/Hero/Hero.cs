using System.Collections;
using UnityEngine;
using PixelCrew.Components.Health;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Model;
using UnityEditor.Animations;
using PixelCrew.Utils;
using PixelCrew.Components.Collectables;

namespace PixelCrew.Creatures.Hero
{
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(UsableItemComponent))]
    public class Hero : Creature
    {
        //Скрипт персонажа, содержащий основные механики взаимодействия с персонажем
        //переменные настраиваемые из Unity
        [Space]
        [Header("Stats")]
        [SerializeField] private float _dashSpeed;
        [SerializeField] private float _dashDuratation;
        [SerializeField] private int _maxInventorySize;
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
        private int SwordCount => _session.Data.Inventory.Count("Sword");

        private int CoinCount => _session.Data.Inventory.Count("Coin");
        public void SayHp() => Debug.Log($"I have {_session.Data.Hp} hp now!");

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

            _session.Data.Inventory.MaxInventorySize = _maxInventorySize;

            _session.Data.Inventory.OnChanged += OnInventoryChanged;
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
        }

        public void OnInventoryChanged(string id, int value)
        {
            if (id == "Sword") UpdateHeroWeapon();
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;

            SayHp();
        }


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
                yVelocity = DoJump();
            }
            else if (_allowDoubleJump && !IsJumpButtonPressed)
            {
                yVelocity = DoJump();
                _allowDoubleJump = false;
            }
            return yVelocity;
        }

        private float DoJump()
        {
            Particles.Spawn("Jump");
            Sounds.Play("Jump");
            IsJumpButtonPressed = true;

            return JumpSpeed;
        }

        //Механика рывка: при нажатии рывка отключаю гравитацию действующую на героя,
        //добавляю импульс в направлении движения героя, жду время рывка и включаю гравитацию обратно
        private IEnumerator Dash()
        {
            FreezeGravity();
            UpdateSpriteDirection();

            _isDashing = true;
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0f);
            Rigidbody.AddForce(new Vector2(_dashSpeed * transform.localScale.x, 0f), ForceMode2D.Impulse);
            Particles.Spawn("Dash");
            Sounds.Play("Dash");
            yield return new WaitForSeconds(_dashDuratation);
            _isDashing = false;

            UnFreezeGravity();
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

        public void AddInInventory(string id, int count, bool isStackable)
        {
            _session.Data.Inventory.Add(id, count, isStackable);
        }
        public void UseItem()
        {
            GetComponent<UsableItemComponent>().Use();
        }


        public override void TakeDamage()
        {
            base.TakeDamage();

            if (CoinCount > 0)
                SpawnCoins();
        }

        private void SpawnCoins()
        {
            var coinsToSpawn = Mathf.Min(CoinCount, 5);

            _session.Data.Inventory.Remove("Coin", coinsToSpawn);

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
            if (SwordCount <= 0) return;

            base.Attack();
        }

        public void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _unarmed;
        }

        //При нажатии кнопки броска начинается отсчет времени сильного броска
        public void ThrowPushed()
        {
            if (SwordCount <= 0) return;
            _throwChargeTime.Reset();
        }
        //При отпускании кнопки броска, срабатывает либо бросок одним снарядом, либо несколькими если счетчик времени сильного броска прошел 
        public void ThrowReleased()
        {
            if (SwordCount <= 1) return;

            if (_throwCooldown.IsReady)
            {
                _throwCooldown.Reset();

                if (_throwChargeTime.IsReady)
                {
                    _throwedCount = 0;
                    int numberOfThrows = _numberOfThrows >= SwordCount ? SwordCount - 1 : _numberOfThrows;
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
            Sounds.Play("Range");

            _session.Data.Inventory.Remove("Sword", 1);
        }

        public void OnDoThrow()
        {
            Particles.Spawn("Throw");
        }
    }
}