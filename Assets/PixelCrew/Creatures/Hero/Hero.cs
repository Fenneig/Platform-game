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
using PixelCrew.Effects.CameraRelated;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Model.Definitions.Repository.Items;
using UnityEngine.Experimental.Rendering.Universal;

namespace PixelCrew.Creatures.Hero
{
    [RequireComponent(typeof(HealthComponent))]
    public class Hero : Creature
    {
        [Space] [Header("Stats")] [SerializeField]
        private float _dashSpeed;

        [SerializeField] private float _dashDuration;
        [SerializeField] private int _baseMeleeDamage;
        [SerializeField] private float _invulnerableTime;


        [Space] [Header("Checkers")] [SerializeField]
        private CheckCircleOverlap _interactionRadius;

        [Space] [Header("Controllers")] [SerializeField]
        private AnimatorController _armed;

        [SerializeField] private AnimatorController _unarmed;

        [Space] [Header("Particle System")] [SerializeField]
        private ParticleSystem _dropCoinsOnHitParticle;

        [SerializeField] private SpawnComponent _throwSpawner;
        [SerializeField] private SpriteRenderer _shieldParticle;
        [SerializeField] private ParticleSystem _criticalHitParticle;
        [SerializeField] private GameObject _lighter;

        [Space] [Header("Throw Stats")] [Min(2)] [SerializeField]
        private int _numChargedThrows;

        [SerializeField] private Timer _throwCooldown;
        [SerializeField] private Timer _timeToPerformChargeThrow;
        [SerializeField] private float _timeBetweenThrows;

        [Space] [Header("Shield Stats")] [SerializeField]
        private float _shieldDuration;

        [SerializeField] private float _shieldEndIndicator;
        [SerializeField] private int _blinksAmount;

        [Space] [Header("Teleport Stats")] [SerializeField]
        private float _teleportDisableGravityDuration;

        private bool _allowDashInJump;
        private bool _isDashing;
        private bool _allowDoubleJump;

        private GameSession _session;
        private HealthComponent _healthComponent;
        private Light2D _lightSource;
        private CameraShakeEffect _cameraShake;
        private SpriteRenderer _sprite;

        private readonly Timer _hasteTimer = new Timer();
        private float _speedBonus;
        private float _dashTrigger;
        private float _defaultLighterIntensity;

        public bool IsJumpButtonPressed { get; set; }

        private const string SwordId = "Sword";
        private const string DashId = "dash";
        private const string ShieldId = "shield";
        private const string TeleportId = "teleport";
        private const string OilId = "CandleOil";
        private static readonly int ThrowKey = Animator.StringToHash("is-throw");


        private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;
        private int SwordCount => _session.Data.Inventory.Count("Sword");
        private int CoinCount => _session.Data.Inventory.Count("Coin");
        private Transform _teleportTransform;

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
            _cameraShake = FindObjectOfType<CameraShakeEffect>();
            _healthComponent = GetComponent<HealthComponent>();
            _sprite = GetComponent<SpriteRenderer>();

            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            _session.StatsModel.OnUpgraded += OnHeroUpgraded;

            _healthComponent.Health = _session.Data.Hp;

            _lightSource = _lighter.GetComponentInChildren<Light2D>();
            _defaultLighterIntensity = _lightSource.intensity;


            UpdateHeroWeapon();
        }

        private void OnHeroUpgraded(StatId statId)
        {
            switch (statId)
            {
                case StatId.Hp:
                    var health = (int) _session.StatsModel.GetValue(statId);
                    _healthComponent.MaxHealth = health;
                    _healthComponent.Health.Value = health;
                    break;
            }
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
            _session.StatsModel.OnUpgraded -= OnHeroUpgraded;
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == SwordId) UpdateHeroWeapon();
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
            var defaultSpeed = _session.StatsModel.GetValue(StatId.Speed);
            return defaultSpeed + _speedBonus;
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

        public void AddInInventory(string itemId, int count)
        {
            _session.Data.Inventory.Add(itemId, count);

            var quickInventorySelection = _session.InventoryModel.QuickInventorySelection;
            if (quickInventorySelection.Any(selected => selected.Value == itemId))
                _session.Data.QuickInventory.Add(itemId, count);
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
            _cameraShake.Shake();
            if (CoinCount > 0)
                SpawnCoins();
            StartCoroutine(InvulnerableEffect(_invulnerableTime, _sprite));
        }

        private void RemoveItem(string itemId, int count)
        {
            _session.Data.Inventory.Remove(itemId, count);
            var quickInventorySelection = _session.InventoryModel.QuickInventorySelection;
            if (quickInventorySelection.All(selected => selected.Value != itemId)) return;

            _session.Data.QuickInventory.Remove(itemId, count);
            if (_session.Data.QuickInventory.Count(itemId) > 0) return;

            var itemToRemove =
                _session.InventoryModel.QuickInventorySelection.FirstOrDefault(item => item.Value == itemId);
            _session.InventoryModel.QuickInventorySelection.Remove(itemToRemove);
        }

        private void SpawnCoins()
        {
            var coinsToSpawn = Mathf.Min(CoinCount, 5);

            RemoveItem("Coin", coinsToSpawn);

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
            var MHComponent = _attackRange.GetComponent<ModifyHealthComponent>();
            var statDamage = (int) _session.StatsModel.GetValue(StatId.MeleeDamage);
            CalculateDamage(MHComponent, _baseMeleeDamage, statDamage);
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

        private IEnumerator ThrowMultiply(int numberOfThrows)
        {
            for (var i = 0; i < numberOfThrows; i++)
            {
                Throw();
                yield return new WaitForSeconds(_timeBetweenThrows);
            }
        }

        private void Throw()
        {
            var throwableId = _session.QuickInventory.SelectedItem.Id;
            var throwableDef = DefsFacade.I.Throwable.Get(throwableId);

            if (string.IsNullOrEmpty(throwableDef.Id)) return;

            Animator.SetTrigger(ThrowKey);
            Sounds.Play("Range");

            var MHComponent = throwableDef.Projectile.GetComponent<ModifyHealthComponent>();
            var baseDamage = throwableDef.BaseDamage;
            var statDamage = (int) _session.StatsModel.GetValue(StatId.RangeDamage);

            CalculateDamage(MHComponent, baseDamage, statDamage);

            _throwSpawner.SetPrefab(throwableDef.Projectile);
            RemoveItem(throwableId, 1);
        }

        private void ChargedThrow()
        {
            var throwableCount = _session.Data.Inventory.Count(SelectedItemId);
            var possibleCount = SelectedItemId == SwordId ? throwableCount - 1 : throwableCount;
            var numThrows = Mathf.Min(_numChargedThrows, possibleCount);
            StartCoroutine(ThrowMultiply(numThrows));
        }

        public void OnDoThrow()
        {
            _throwSpawner.Spawn();
            _teleportTransform = null;
            _teleportTransform = _throwSpawner.InstanceTransform;
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
            RemoveItem(SelectedItemId, 1);
        }

        private bool IsSelectedItem(ItemTag tag)
        {
            return _session.QuickInventory.SelectedDef.HasTag(tag);
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
                case TeleportId:
                {
                    Teleport();
                    break;
                }
            }
        }

        private void Teleport()
        {
            var cd = _session.PerksModel.Cooldown;
            if (_teleportTransform == null || !cd.IsReady) return;

            transform.position = _teleportTransform.position;
            _teleportTransform.GetComponent<DestroyObjectComponent>().DestroyObject();
            StartCoroutine(DisableGravity());
            cd.Reset();
        }

        private IEnumerator DisableGravity()
        {
            FreezeGravity();
            yield return new WaitForSeconds(_teleportDisableGravityDuration);
            UnFreezeGravity();
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
            yield return InvulnerableEffect(_shieldEndIndicator, _shieldParticle);
            _shieldParticle.enabled = false;
        }

        private IEnumerator InvulnerableEffect(float invulnerableTime, SpriteRenderer blinkingSprite)
        {
            _healthComponent.IsInvulnerable = true;

            var color = blinkingSprite.color;

            for (var i = 0; i < _blinksAmount; i++)
            {
                yield return this.LerpAnimation(i % 2, (i + 1) % 2, invulnerableTime / _blinksAmount,
                    alpha => blinkingSprite.color = new Color(color.r, color.g, color.b, alpha));
            }

            color.a = 255;
            blinkingSprite.color = color;

            _healthComponent.IsInvulnerable = false;
        }

        private void CalculateDamage(ModifyHealthComponent attackerMHComponent, int baseDamage, int statDamage)
        {
            var criticalChance = _session.StatsModel.GetValue(StatId.CriticalChance);
            var isCritical = criticalChance >= Random.Range(0, 100);
            var criticalDamage = _session.StatsModel.GetValue(StatId.CriticalDamage) / 100;
            var damage = -(baseDamage + statDamage);
            if (isCritical) _criticalHitParticle.Play();
            attackerMHComponent.Delta = isCritical ? (int) (damage * criticalDamage) : damage;
        }

        public void SwitchLight()
        {
            _lighter.SetActive(!_lighter.activeSelf);
            StartCoroutine(OilConsume());
        }

        private IEnumerator OilConsume()
        {
            while (_lighter.activeSelf)
            {
                _session.Data.Inventory.Remove(OilId, 1);
                var oilItem = _session.Data.Inventory.GetItem(OilId);

                if (oilItem == null)
                {
                    SwitchLight();
                    continue;
                }

                const float minOilAmount = 10f;
                if (oilItem.Value > minOilAmount)
                {
                    _lightSource.intensity = _defaultLighterIntensity;
                }
                else
                {
                    _lightSource.intensity -= 0.1f;
                }

                yield return new WaitForSeconds(1f);
            }
        }
    }

    public interface IUsable
    {
        void Use(GameObject target);
    }
}