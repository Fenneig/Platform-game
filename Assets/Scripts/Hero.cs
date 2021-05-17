using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Vector2 _direction;
    public void SetDirection(Vector2 direction) 
    {
        _direction = direction;
    }

    public Vector2 GetDirection => _direction;

    public void SaySomething() 
    {
        Debug.Log("Something");
    }

    //перегрузка метода, использую для дебагинга 
    public void SaySomething(string message) 
    {
        Debug.Log(message);
    }

    private void Update()
    {
        if (_direction.magnitude != 0) 
        {
            var deltaX = _direction.x * _speed * Time.deltaTime;
            var newXPosition = transform.position.x + deltaX;

            var deltaY = _direction.y * _speed * Time.deltaTime;
            var newYPosition = transform.position.y + deltaY;

            transform.position = new Vector3(newXPosition, newYPosition, transform.position.z);
        }
    }

}
