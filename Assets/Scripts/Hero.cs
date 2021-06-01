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
        //переменные получаемые из методов
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Vector2 _direction;
        private float _dashDirection;
        private int _coins;
        private bool _allowDoubleJump;
        private bool _isGrounded;
        private bool _isDashing;
        private float _gravity;
        private bool _allowDashInJump = true;
        private Collider2D[] _interactionResult = new Collider2D[1];

        private static readonly int IsGroundedKey = Animator.StringToHash("is-ground");
        private static readonly int IsRuningKey = Animator.StringToHash("is-running");
        private static readonly int VerticaVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int Hit = Animator.StringToHash("hit");

        //в начале жизненного цикла объекта получаем rigidBody привязанный к объекту для дальнейшего использования
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _gravity = _rigidbody.gravityScale;
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        //Установка вектора направления движения
        public void SetDirection(Vector2 direction) => _direction = direction;

        //Установка вектора направления дэша  
        public void SetDashDirection(float direction) => _dashDirection = direction;

        //метод передает направление движения героя в другие места при необходимости
        public Vector2 GetDirection() => _direction;

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
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.velocity.y > 0)
            {
                yVelocity /= 2f;
            }

            return yVelocity;
        }

        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;
            if (!isFalling) return yVelocity;

            if (_isGrounded)
            {
                yVelocity += _jumpSpeed;
            }
            else if (_allowDoubleJump)
            {
                yVelocity = _jumpSpeed;
                _allowDoubleJump = false;
            }
            return yVelocity;
        }
        //Механика рывка: при нажатии рывка отключаю гравитацию действующую на героя, добавляю импульс в направлении рывка
        //жду время рывка и включаю гравитацию обратно, так же во время рывка отключаю возможность двигаться (условие в FixedUpdate)
        IEnumerator Dash()
        {
            UpdateSpriteRenderer();
            _isDashing = true;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
            _rigidbody.AddForce(new Vector2(_dashSpeed * _dashDirection, 0f), ForceMode2D.Impulse);
            _rigidbody.gravityScale = 0f;
            yield return new WaitForSeconds(_dashDuratation);
            _rigidbody.gravityScale = _gravity;
            _isDashing = false;
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
                _spriteRenderer.flipX = true;
            }
            else if (_direction.x > 0 || _dashDirection > 0)
            {
                _spriteRenderer.flipX = false;
            }
        }
              
        public void TakeDamage()
        {
            SayHp();
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);
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
    }
}