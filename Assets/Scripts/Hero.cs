using System.Collections;
using UnityEngine;
using PixelCrew.Components;

namespace PixelCrew
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Hero : MonoBehaviour
    {
        //Скрипт персонажа, содержащий основные механики взаимодействия с персонажем
        //переменные настраиваемые из Unity
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _damageJumpSpeed;
        [SerializeField] private float _dashSpeed;
        [SerializeField] private float _dashDuratation;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private SpawnComponent _footParticles;
        [SerializeField] private ParticleSystem _hitParticle;
        //переменные получаемые из методов
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private Vector2 _direction;
        private float _dashDirection;
        private int _coins;
        private bool _allowDoubleJump;
        private bool _isGrounded;
        private bool _isDashing;
        private bool _isJumping;
        private bool _isJumpButtonPressed;
        private bool _isHeavyLanding;
        private float _currentGravity;
        private bool _allowDashInJump;
        private Collider2D[] _interactionResult = new Collider2D[1];
        //переменные-ключи для аниматора
        private static readonly int IsGroundedKey = Animator.StringToHash("is-ground");
        private static readonly int IsRuningKey = Animator.StringToHash("is-running");
        private static readonly int VerticaVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int HitKey = Animator.StringToHash("hit");
        private static readonly int JumpKey = Animator.StringToHash("jump");

        //при создании объекта получаем некоторые значения привязанные к объекту для дальнейшего использования
        //Rigidbody, animator и SpriteRenderer берутся из компонент объекта, гравитация - обычное значение гравитации 
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _currentGravity = _rigidbody.gravityScale;
            _animator = GetComponent<Animator>();
            _isHeavyLanding = false;
            _allowDashInJump = true;
        }

        public float CurrentGravity 
        {
            get { return _currentGravity; }
            set 
            {
                _currentGravity = value;
                _rigidbody.gravityScale = value;
            }
        }
        public bool IsJumpButtonPressed 
        {
            get { return _isJumpButtonPressed; }
            set { _isJumpButtonPressed = value; }
        }
        //Свойство направления 
        public Vector2 Direction 
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public float DashDirection 
        {
            set { _dashDirection = value; }
        }
        public void SaySomething() => Debug.Log("Something");

        public void CollectCoin(int value) => _coins += value;

        public void SayCoins() => Debug.Log($"I have {_coins} coins!");

        public void SayHp() => Debug.Log($"I have {GetComponent<HealthComponent>().GetHealth()} hp now!");

        //проверяем наличие земли под нагами героя. Реализация в классе LayerCheck
        private bool IsGrounded() => _groundCheck.IsTouchingGround();

        private void Update()
        {
            _isGrounded = IsGrounded();
        }

        //В FixedUpdate обрабатывается движения персонажа, с использованием физики используется FixedUpdate метод.
        private void FixedUpdate()
        {
            if (_isGrounded)
            {
                _allowDoubleJump = true;
                _allowDashInJump = true;
                _isJumping = false;
            }

            if (!_isDashing)
            {
                var xVelocity = _direction.x * _speed;
                var yVelocity = CalculateYVelocity();
                _rigidbody.velocity = new Vector2(xVelocity, yVelocity);
            }

            if (_dashDirection != 0)
            {
                if (AllowDash())
                {
                    StartCoroutine(Dash());
                }
                _dashDirection = 0f;
            }

            AnimatorSettings();

            UpdateSpriteRenderer();

        }

        private float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (isJumpPressing)
            {
                _isJumping = true;
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity /= 2f;
            }
            CalculateHeavyLanding(yVelocity);
            return yVelocity;
        }
        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;
            if (!isFalling) return yVelocity;
            _animator.SetTrigger(JumpKey);
            if (_isGrounded)
            {
                yVelocity = _jumpSpeed;
                IsJumpButtonPressed = true;
            }
            else if (_allowDoubleJump && !IsJumpButtonPressed)
            {
                yVelocity = _jumpSpeed;
                _allowDoubleJump = false;
            }
            return yVelocity;
        }
        private void CalculateHeavyLanding(float yVelocity)
        {
            if (yVelocity < -10) _isHeavyLanding = true;
            if (_isHeavyLanding && yVelocity == 0)
            {
                CreateDust("LandingDust");
                _isHeavyLanding = false;
            }
        }


        //Механика рывка: при нажатии рывка отключаю гравитацию действующую на героя, добавляю импульс в направлении рывка
        //жду время рывка и включаю гравитацию обратно, так же во время рывка отключаю возможность двигаться (условие в FixedUpdate)
        IEnumerator Dash()
        {
            UpdateSpriteRenderer();
            CreateDust("DashDust");
            _isDashing = true;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
            _rigidbody.AddForce(new Vector2(_dashSpeed * _dashDirection, 0f), ForceMode2D.Impulse);
            _rigidbody.gravityScale = 0f;
            yield return new WaitForSeconds(_dashDuratation);
            _isDashing = false;
            _rigidbody.gravityScale = CurrentGravity;
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

        private void AnimatorSettings()
        {
            _animator.SetBool(IsGroundedKey, _isGrounded);
            _animator.SetBool(IsRuningKey, _direction.x != 0);
            _animator.SetFloat(VerticaVelocityKey, _rigidbody.velocity.y);
        }

        private void UpdateSpriteRenderer()
        {
            if (_direction.x < 0 || _dashDirection < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (_direction.x > 0 || _dashDirection > 0)
            {
                transform.localScale = Vector3.one;
            }
        }

        public void TakeDamage()
        {
            SayHp();
            _isJumping = false;
            _animator.SetTrigger(HitKey);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);
            if (_coins > 0)
                SpawnCoins();
        }

        private void SpawnCoins()
        {
            var coinsToSpawn = Mathf.Min(_coins, 5);
            _coins -= coinsToSpawn;

            var burst = _hitParticle.emission.GetBurst(0);
            burst.count = coinsToSpawn;
            _hitParticle.emission.SetBurst(0, burst);

            _hitParticle.gameObject.SetActive(true);
            _hitParticle.Play();
        }

        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _interactionRadius,
                _interactionResult,
                _interactionLayer);
            
            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }

        public void CreateDust(string tag)
        {
            _footParticles.Spawn(tag);
        }
    }
}