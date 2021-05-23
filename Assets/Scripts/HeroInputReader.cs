using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;
        public void OnMovement(InputAction.CallbackContext context) => _hero.SetDirection(context.ReadValue<Vector2>());
        public void OnSaySomething(InputAction.CallbackContext context)
        {
            if (context.started) _hero.SaySomething();
        }
    }
}