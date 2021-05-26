using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Hero : MonoBehaviour
    {
        //Скрипт персонажа, содержащий основные механики взаимодействия с персонажем
        //переменные настраиваемые из Unity
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private LayerCheck _groundCheck;
        //переменные получаемые из методов
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Vector2 _direction;
        private int _coins;
        private bool _allowDoubleJump;
        private bool _isGrounded;


        private static readonly int IsGroundedKey = Animator.StringToHash("is-ground");
        private static readonly int IsRuningKey = Animator.StringToHash("is-running");
        private static readonly int VerticaVelocityKey = Animator.StringToHash("vertical-velocity");

        //в начале жизненного цикла объекта получаем rigidBody привязанный к объекту для дальнейшего использования
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        //метод используемый в InputReader для понимания в какую сторону должен идти пресонаж
        public void SetDirection(Vector2 direction) => _direction = direction;

        //метод передает направление движения героя в другие места при необходимости
        public Vector2 GetDirection() => _direction;

        public void SaySomething() => Debug.Log("Something");

        public void CollectCoin(int value) => _coins += value;

        public void SayCoins() => Debug.Log($"I have {_coins} coins!");

        //проверяем наличие земли под нагами героя. Реализация в классе LayerCheck
        private bool IsGrounded() => _groundCheck.IsTouchingGround();

        private void Update()
        {
            _isGrounded = IsGrounded();
        }

        //В FixedUpdate обрабатывается движения персонажа, с использованием физики используется FixedUpdate метод.
        private void FixedUpdate()
        {
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();

            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            AnimatorSettings();

            UpdateSpriteRenderer();
        }

        private float CalculateYVelocity() 
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded) _allowDoubleJump = true;
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

        private void AnimatorSettings()
        {
            _animator.SetBool(IsGroundedKey, _isGrounded);
            _animator.SetBool(IsRuningKey, _direction.x != 0);
            _animator.SetFloat(VerticaVelocityKey, _rigidbody.velocity.y);
        }

        private void UpdateSpriteRenderer()
        {
            if (_direction.x < 0)
            {
                _spriteRenderer.flipX = true;
            }
            else if (_direction.x > 0)
            {
                _spriteRenderer.flipX = false;
            }
        }
    }
}