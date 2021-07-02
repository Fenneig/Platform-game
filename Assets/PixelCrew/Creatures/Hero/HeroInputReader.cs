using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Creatures.Hero
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started) _hero.Attack();
        }
        public void OnMovement(InputAction.CallbackContext context) => _hero.Direction = (context.ReadValue<Vector2>());

        public void OnDash(InputAction.CallbackContext contex) => _hero.DashTrigger = contex.ReadValue<float>();

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started) _hero.Interact();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.canceled) _hero.IsJumpButtonPressed = false;
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            if (context.started) _hero.ThrowPushed();
            if (context.canceled) _hero.ThrowReleased();
        }

        public void OnUseItem(InputAction.CallbackContext context) 
        {
            if (context.canceled) _hero.UseItem();
        }
    }
}