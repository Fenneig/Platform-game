using System.Collections;
using System.Linq;
using UnityEngine;
using PixelCrew.Components.Health;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEditor.Animations;
using PixelCrew.Model.Definitions;
using PixelCrew.Components.GOBased;
using PixelCrew.Model.Definitions.Repository.Items;

namespace PixelCrew.Creatures.Hero
{
    [RequireComponent(typeof(HealthComponent))]
    public class Hero : Creature
    {
        [Space] [Header("Stats")] [SerializeField]
        private float _dashSpeed;

        [SerializeField] private float _dashDuration;

        [Space] [Header("Checkers")] [SerializeField]
        private CheckCircleOverlap _interactionRadius;

        [Space] [Header("Controllers")] [SerializeField]
        private AnimatorController _armed;

        [SerializeField] private AnimatorController _unarmed;

        [Space] [Header("Particle System")] [SerializeField]
        private ParticleSystem _dropCoinsOnHitParticle;

        [SerializeField] private SpawnComponent _throwSpawner;
        [SerializeField] private SpriteRenderer _shieldParticle;

        [Space] [Header("Throw Stats")] [Min(2)] [SerializeField]
        private int _numChargedThrows;

        [SerializeField] private Timer _throwCooldown;
        [SerializeField] private Timer _timeToPerformChargeThrow;
        [SerializeField] private float _chargeThrowDelay;

        [Space] [Header("Shield Stats")] [SerializeField]
        private float _shieldDuration;
        [SerializeField] private float _shieldEndIndicator;
        [SerializeField] private int _blinksAmount;

        private bool _allowDashInJump;
        private bool _isDashing;
        private bool _allowDoubleJump;

        private HealthComponent _healthComponent;

        private static readonly int ThrowKey = Animator.StringToHash("is-throw");

        private GameSession _session;

        private Timer _hasteTimer = new Timer();
        private float _speedBonus;
        private float _dashTrigger;

        public bool IsJumpButtonPressed { get; set; }

        private const string SwordId = "Sword";
        private const string DashId = "dash";
        private const string ShieldId = "shield";
        private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;
        private int SwordCount => _session.Data.Inventory.Count("Sword");
        private int CoinCount => _session.Data.Inventory.Count("Coin");

        private bool CanThrow
        {
            get
            {
                if (!_session.QuickInventory.Inventory.Any()) return false;
                if (SelectedItemId == SwordId)
                    return SwordCount > 1;

                var def = DefsFacade.I.Items.Get(SelectedItemId);
                return def.HasTag(ItemTag.Throwable);
            }
        }


        protected override void Awake()
        {
            base.Awake();
            _allowDashInJump = true;
        }

        public void SelectItemByDirection(float direction)
        {
            if (direction != 0) _session.QuickInventory.SetNextItem(direction);
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();

            _healthComponent = GetComponent<HealthComponent>();

            _healthComponent.Health = _session.Data.Hp;
            UpdateHeroWeapon();

            _session.Data.Inventory.OnChanged += OnInventoryChanged;
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == SwordId) UpdateHeroWeapon();
        }

        public void OnHealthChanged(int currentHealth)
        {
            var newHealth = currentHealth > DefsFacade.I.Player.MaxHealth
                ? DefsFacade.I.Player.MaxHealth
                : currentHealth;
            _session.Data.Hp.Value = newHealth;
        }

        protected override void Update()
        {
            base.Update();

            if (_session.PerksModel.Cooldown.IsReady && _session.PerksModel.IsShieldSupported)
            {
                _shieldParticle.enabled = false;
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (IsGrounded)
            {
                _allowDoubleJump = true;
                _allowDashInJump = true;
            }

            if (_dashTrigger == 0) return;

            if (AllowDash())
            {
                StartCoroutine(Dash());
            }

            _dashTrigger = 0f;
        }

        //При рывке отключается просчет движения героя 
        protected override Vector2 CalculateVelocity()
        {
            return !_isDashing ? base.CalculateVelocity() : Rigidbody.velocity;
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded && !IsJumpButtonPressed)
            {
                yVelocity = DoJump();
            }
            else if (_allowDoubleJump && !IsJumpButtonPressed && _session.PerksModel.IsDoubleJumpSupported)
            {
                yVelocity = DoJump();
                _allowDoubleJump = false;
                _session.PerksModel.Cooldown.Reset();
            }

            return yVelocity;
        }


        public void DrinkHastePotion(float speedBonus, float hasteTime)
        {
            _hasteTimer.Value = _hasteTimer.RemainingTime + hasteTime;
            _speedBonus = Mathf.Max(speedBonus, _speedBonus);
            _hasteTimer.Reset();
        }

        protected override float CalculateSpeed()
        {
            if (_hasteTimer.IsReady) _speedBonus = 0f;
            return base.CalculateSpeed() + _speedBonus;
        }

        private float DoJump()
        {
            _particles.Spawn("Jump");
            Sounds.Play("Jump");
            IsJumpButtonPressed = true;

            return _jumpSpeed;
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
            _particles.Spawn("Dash");
            Sounds.Play("Dash");
            yield return new WaitForSeconds(_dashDuration);
            _isDashing = false;

            UnFreezeGravity();
        }

        private void FreezeGravity()
        {
            Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
            Rigidbody.freezeRotation = true;
        }

        private void UnFreezeGravity()
        {
            Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private bool AllowDash()
        {
            if (!_session.PerksModel.IsDashSupported) return false;
            _session.PerksModel.Cooldown.Reset();

            if (!_allowDashInJump) return _allowDashInJump;

            _allowDashInJump = false;

            return true;
        }

        public void RecoverExtraMoves()
        {
            _allowDashInJump = true;
            _allowDoubleJump = true;
            _session.PerksModel.Cooldown.EarlyComplete();
        }

        public void AddInInventory(string id, int count)
        {
            _session.Data.Inventory.Add(id, count);
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

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _unarmed;
        }

        private void PerformThrowing()
        {
            if (!CanThrow || !_throwCooldown.IsReady) return;

            _throwCooldown.Reset();

            if (_timeToPerformChargeThrow.IsReady &&
                _session.PerksModel.IsChargedThrowSupported)
            {
                ChargedThrow();
                _session.PerksModel.Cooldown.Reset();
            }
            else
            {
                Throw();
            }
        }

        public void StartThrowing()
        {
            if (!CanThrow) return;
            _timeToPerformChargeThrow.Reset();
        }

        public void UseInventory()
        {
            if (IsSelectedItem(ItemTag.Throwable)) PerformThrowing();
            else if (IsSelectedItem(ItemTag.Potion)) UsePotion();
        }

        private void UsePotion()
        {
            var usableItemDef = DefsFacade.I.Potion.Get(SelectedItemId);
            if (string.IsNullOrEmpty(usableItemDef.Id)) return;

            usableItemDef.Object.GetComponent<IUsable>()?.Use(gameObject);
            Sounds.Play("Potion");
            _session.Data.Inventory.Remove(SelectedItemId, 1);
        }

        private bool IsSelectedItem(ItemTag tag)
        {
            return _session.QuickInventory.SelectedDef.HasTag(tag);
        }

        private void ChargedThrow()
        {
            var throwableCount = _session.Data.Inventory.Count(SelectedItemId);
            var possibleCount = SelectedItemId == SwordId ? throwableCount - 1 : throwableCount;
            var numThrows = Mathf.Min(_numChargedThrows, possibleCount);
            StartCoroutine(ThrowMultiply(numThrows));
        }

        private IEnumerator ThrowMultiply(int numberOfThrows)
        {
            for (var i = 0; i < numberOfThrows; i++)
            {
                Throw();
                yield return new WaitForSeconds(_chargeThrowDelay);
            }
        }

        private void Throw()
        {
            Animator.SetTrigger(ThrowKey);
            Sounds.Play("Range");
            var throwableId = _session.QuickInventory.SelectedItem.Id;
            var throwableDef = DefsFacade.I.Throwable.Get(throwableId);

            if (string.IsNullOrEmpty(throwableDef.Id)) return;

            _throwSpawner.SetPrefab(throwableDef.Projectile);
            _session.Data.Inventory.Remove(throwableId, 1);
        }

        public void OnDoThrow()
        {
            _throwSpawner.Spawn();
        }


        public void UsePerk(float value)
        {
            var perkType = _session.PerksModel.Used;
            switch (perkType)
            {
                case DashId:
                {
                    _dashTrigger = value;
                    break;
                }
                case ShieldId:
                {
                    if (value != 0) ActivateShield();
                    break;
                }
            }
        }

        private void ActivateShield()
        {
            _session.PerksModel.Cooldown.Reset();
            StartCoroutine(ShieldEffect());
        }

        private IEnumerator ShieldEffect()
        {
            _shieldParticle.enabled = true;
            _healthComponent.IsInvulnerable = true;
            yield return new WaitForSeconds(_shieldDuration - _shieldEndIndicator);
            for (var i = 0; i < _blinksAmount; i++)
                yield return AlphaAnimationUtils.AlphaAnimation(_shieldParticle, i % 2, _shieldEndIndicator / _blinksAmount);
            var color = _shieldParticle.color;
            color.a = 255;
            _shieldParticle.color = color;
            _healthComponent.IsInvulnerable = false;
            _shieldParticle.enabled = false;
        }
    }

    public interface IUsable
    {
        void Use(GameObject target);
    }
}