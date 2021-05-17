using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroInputReader : MonoBehaviour
{
    [SerializeField] private Hero _hero;
    
    private void OnHorizontalMovement(InputValue context) 
    {
        var horizontal = context.Get<float>();
        _hero.SetDirection(horizontal);
        //_hero.SaySomething(context.Get<float>().ToString());
    }

    private void OnSaySomething() 
    {
        _hero.SaySomething();
    }
}
