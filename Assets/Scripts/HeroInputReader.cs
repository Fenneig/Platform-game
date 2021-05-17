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
        _hero.SetDirection(horizontal);
    }

    public void OnSaySomething(InputAction.CallbackContext context) 
    {
        if (context.started) _hero.SaySomething();
    }
}
