using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpSpeed;

    [SerializeField] private LayerCheck _groundCheck;

    private Rigidbody2D _rigidbody;
    private Vector2 _direction;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 direction) 
    {
        _direction = direction;
    }

    public void SaySomething() 
    {
        Debug.Log("Something");
    }

    private bool IsGrounded() 
    {
        return _groundCheck.IsTouchingLayer();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);

        var isJumping = _direction.y > 0;

        //if (Mathf.Abs(_rigidbody.velocity.y) > 1f) isJumping = false;

        if (isJumping && IsGrounded()) 
        {
            _rigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
        }
    }

}
