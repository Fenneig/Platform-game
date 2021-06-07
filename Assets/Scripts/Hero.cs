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
        [SerializeField] private ParticleSystem _dropCoinsOnHitParticle;
        //переменные получаемые из методов
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private Vector2 _movementDirection;
        private float _dashDirection;
        private int _coins;
        private bool _allowDoubleJump;
        private bool _isGrounded;
        private bool _isDashing;
        private bool _isJumping;
        private bool _isJumpButtonPressed;
        private bool _isHeavyLanding;
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
            _animator = GetComponent<Animator>();
            _isHeavyLanding = false;
            _allowDashInJump = true;
        }
        public bool IsJumpButtonPressed 
        {
            get => _isJumpButtonPressed;
            set => _isJumpButtonPressed = value;
        }

        //Свойство направления 
        public Vector2 Direction 
        {
            get => _movementDirection;
            set => _movementDirection = value; 
        }

        public float DashDirection 
        {
            set => _dashDirection = value;         
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
                //при движении наискосок с зажатым прыжком персонаж двигается по гипотенузе прямоугольно треугольника тем самым замедляясь
                //т.к. направления могут быть либо 0 либо 1 то при движении по гипотенузе скорость становиться на √2 меньше( с = √x*x+y*y, где x=y=1)
                //    /|
                // с / |
                //  /  |  y
                // /   |
                ///____|
                //   x
                //
                var xVelocity = Mathf.Abs(_movementDirection.y) > 0 ? _movementDirection.x * _speed * Mathf.Sqrt(2) : _movementDirection.x * _speed;
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
            var isJumpPressing = _movementDirection.y > 0;

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
            if (_isGrounded && !IsJumpButtonPressed)
            {
                yVelocity = _jumpSpeed;
                IsJumpButtonPressed = true;
            }
            else if (_allowDoubleJump && !IsJumpButtonPressed)
            {
                yVelocity = _jumpSpeed;
                _allowDoubleJump = false;
                IsJumpButtonPressed = true;
            }
            return yVelocity;
        }

        private void CalculateHeavyLanding(float yVelocity)
        {
            if (yVelocity < -12) _isHeavyLanding = true;
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
            _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
            _rigidbody.freezeRotation = true;
            UpdateSpriteRenderer();
            CreateDust("DashDust");
            _isDashing = true;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
            _rigidbody.AddForce(new Vector2(_dashSpeed * _dashDirection, 0f), ForceMode2D.Impulse);
            yield return new WaitForSeconds(_dashDuratation);
            _isDashing = false;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
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
            _animator.SetBool(IsRuningKey, _movementDirection.x != 0);
            _animator.SetFloat(VerticaVelocityKey, _rigidbody.velocity.y);
        }

        private void UpdateSpriteRenderer()
        {
            if (_movementDirection.x < 0 || _dashDirection < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (_movementDirection.x > 0 || _dashDirection > 0)
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

            var burst = _dropCoinsOnHitParticle.emission.GetBurst(0);
            burst.count = coinsToSpawn;
            _dropCoinsOnHitParticle.emission.SetBurst(0, burst);

            _dropCoinsOnHitParticle.gameObject.SetActive(true);
            _dropCoinsOnHitParticle.Play();
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