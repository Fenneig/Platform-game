﻿using System.Collections;
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
        private Vector2 _direction;
        private int _coins;
        //в начале жизненного цикла объекта получаем rigidBody привязанный к объекту для дальнейшего использования
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        //метод используемый в InputReader для понимания в какую сторону должен идти пресонаж
        public void SetDirection(Vector2 direction) => _direction = direction;
        //метод передает направление движения героя в другие места при необходимости
        public Vector2 GetDirection() => _direction;
        public void SaySomething() => Debug.Log("Something");
        public void CollectCoin(int value) => _coins += value;
        public void SayCoins() => Debug.Log($"I have {_coins} coins!");
        //проверяем наличие земли под нагами героя. Реализация в классе LayerCheck
        private bool IsGrounded() => _groundCheck.IsTouchingLayer();
        //В FixedUpdate обрабатывается движения персонажа, с использованием физики используется FixedUpdate метод.
        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);

            Jump();
        }
        //вынес реализацию прыжка в отдельный метод, если понадобиться изменять либо сам метод либо дополнять FixedUpdate
        private void Jump()
        {
            var isJumping = _direction.y > 0;

            if (isJumping)
            {
                if (IsGrounded() && _rigidbody.velocity.y <= 0)
                    _rigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
            }
            else if (_rigidbody.velocity.y > 0)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y / 2);
            }

        }
    }
}