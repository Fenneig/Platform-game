using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroInputReader : MonoBehaviour
{
    [SerializeField] private Hero _hero;

    public void OnHorizontalMovement(InputAction.CallbackContext context) 
    {
        var horizontal = context.ReadValue<float>();
        _hero.SaySomething(context.ReadValue<float>().ToString());
        _hero.SetDirection(new Vector2(horizontal, _hero.GetDirection.y));
    }

    public void OnVerticalMovement(InputAction.CallbackContext context)
    {
        var vertical = context.ReadValue<float>();
        _hero.SaySomething(context.ReadValue<float>().ToString());
        _hero.SetDirection(new Vector2(_hero.GetDirection.x, vertical));
    }

    public void OnSaySomething(InputAction.CallbackContext context) 
    {
        if (context.started) _hero.SaySomething();
    }
}
