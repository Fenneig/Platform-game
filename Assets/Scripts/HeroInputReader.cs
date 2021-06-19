using UnityEngine;
using UnityEngine.InputSystem;
using PixelCrew.Creatures;

namespace PixelCrew
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started) _hero.Attack();
        }
        public void OnMovement(InputAction.CallbackContext context) => _hero.Direction = (context.ReadValue<Vector2>());

        public void OnDash(InputAction.CallbackContext contex) => _hero.DashDirection = contex.ReadValue<float>();

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started) _hero.Interact();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.canceled) _hero.IsJumpButtonPressed = false;
        }
    }
}