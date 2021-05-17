using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroInputReader : MonoBehaviour
{
    [SerializeField] private Hero _hero;

    private HeroInputActions _inputActions;
    private void Awake()
    {
        _inputActions = new HeroInputActions();
        _inputActions.Hero.HorizontalMovement.performed += OnHorizontalMovement;
        _inputActions.Hero.HorizontalMovement.canceled += OnHorizontalMovement;
        _inputActions.Hero.SaySomething.started += OnSaySomething;
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }
    private void OnHorizontalMovement(InputAction.CallbackContext context) 
    {
        var horizontal = context.ReadValue<float>();
        _hero.SetDirection(horizontal);
    }

    private void OnSaySomething(InputAction.CallbackContext context) 
    {
        _hero.SaySomething();
    }
}
