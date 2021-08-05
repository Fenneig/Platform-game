using PixelCrew.Audio;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GOBased;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(JumpFromPlatformComponent))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlaySoundsComponent))]
    public class Creature : MonoBehaviour
    {
        [Space]
        [Header("Stats")]
        [SerializeField] private float _speed;
        [SerializeField] private float _damageVelocity;
        [SerializeField] private float _heavyLandingVelocity;
        [SerializeField] protected float JumpSpeed;

        [Space]
        [Header("Checkers")]
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private CheckCircleOverlap _attackRange;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] protected SpawnListComponent Particles;

        private Vector2 _direction;

        protected Rigidbody2D Rigidbody;
        protected Animator Animator;
        protected bool IsGrounded;
        protected PlaySoundsComponent Sounds;
        private bool _isJumping;

        //переменные-ключи для аниматора
        protected static readonly int IsGroundedKey = Animator.StringToHash("is-ground");
        protected static readonly int IsRuningKey = Animator.StringToHash("is-running");
        protected static readonly int VerticaVelocityKey = Animator.StringToHash("vertical-velocity");
        protected static readonly int HitKey = Animator.StringToHash("hit");
        protected static readonly int JumpKey = Animator.StringToHash("jump");
        protected static readonly int AttackKey = Animator.StringToHash("attack");


        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            Sounds = GetComponent<PlaySoundsComponent>();
        }

        public Vector2 Direction
        {
            get => _direction;
            set => _direction = value;
        }

        private void Update()
        {
            IsGrounded = _groundCheck.IsTouchingLayer;
        }

        protected virtual void FixedUpdate()
        {
            if (IsGrounded)
            {
                _isJumping = false;
            }

            var velocity = CalculateVelocity();

            Rigidbody.velocity = velocity;

            AnimatorSettings();

            UpdateSpriteDirection();
        }

        protected virtual Vector2 CalculateVelocity()
        {
            var xVelocity = CalculateXVelocity();
            var yVelocity = CalculateYVelocity();

            return new Vector2(xVelocity, yVelocity);

        }

        private void AnimatorSettings()
        {
            Animator.SetBool(IsGroundedKey, IsGrounded);
            Animator.SetBool(IsRuningKey, _direction.x != 0);
            Animator.SetFloat(VerticaVelocityKey, Rigidbody.velocity.y);
        }

        protected virtual void UpdateSpriteDirection()
        {
            if (Rigidbody.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (Rigidbody.velocity.x > 0)
            {
                transform.localScale = Vector3.one;
            }
        }

        protected virtual float CalculateXVelocity()
        {
            //при движении наискосок с зажатым прыжком персонаж двигается по гипотенузе прямоугольно треугольника тем самым замедляясь
            //т.к. направления могут быть либо 0 либо 1 то при движении по гипотенузе скорость становиться на √2 меньше( с = √x*x+y*y, где x=y=1)
            //    /|
            // с / |
            //  /  |  y
            // /   |
            ///____|
            //   x
            return Mathf.Abs(_direction.y) > 0 ? _direction.x * _speed * Mathf.Sqrt(2) : _direction.x * _speed;
        }

        protected virtual float CalculateYVelocity()
        {
            if (_direction.y < 0) gameObject.GetComponent<JumpFromPlatformComponent>().JumpOff();
            var yVelocity = Rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (isJumpPressing)
            {
                Animator.SetTrigger("jump");

                _isJumping = true;

                var isFalling = Rigidbody.velocity.y <= 0.001f;

                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (Rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity /= 2f;
            }
            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            Particles.Spawn("Jump");
            Sounds.Play("Jump");

            if (IsGrounded)
            {
                yVelocity = JumpSpeed;
            }

            return yVelocity;
        }

        public virtual void TakeDamage()
        {
            _isJumping = false;
            Animator.SetTrigger(HitKey);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageVelocity);
        }

        //При падении на GroundLayer с высокой скоростью создается партикл тяжелого падения
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.IsInLayer(_groundLayer))
            {
                var contact = collision.contacts[0];
                if (contact.relativeVelocity.y >= _heavyLandingVelocity)
                {
                    Particles.Spawn("HeavyLanding");
                }
            }
        }

        public virtual void Attack()
        {
            Animator.SetTrigger(AttackKey);
        }

        public void OnDoAttack()
        {
            _attackRange.Check();
            Sounds.Play("Melee");
        }

        public void FreezeGravity()
        {
            Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
            Rigidbody.freezeRotation = true;
        }

        public void UnFreezeGravity()
        {
            Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

    }
}
