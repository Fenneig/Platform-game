using PixelCrew.UI.Windows.Pause;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Creatures.Hero
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;
        private PauseMenuCaller _pauseMenuCaller;

        private void Awake()
        {
            _pauseMenuCaller = FindObjectOfType<PauseMenuCaller>();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started) _hero.Attack();
        }
        public void OnMovement(InputAction.CallbackContext context) => _hero.Direction = context.ReadValue<Vector2>();

        public void OnDash(InputAction.CallbackContext context) => _hero.DashTrigger = context.ReadValue<float>();

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started) _hero.Interact();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.canceled) _hero.IsJumpButtonPressed = false;
        }
        
        public void OnUseItem(InputAction.CallbackContext context) 
        {
            if (context.started) _hero.StartThrowing();
            if (context.canceled) _hero.UseInventory();
        }

        public void OnPause(InputAction.CallbackContext context) 
        {
            if (context.started) _pauseMenuCaller.SwitchPauseCondition();
        }
       
        public void OnItemSelection(InputAction.CallbackContext context)
        {
            if (context.started) _hero.SelectItemByDirection(context.ReadValue<float>());
        }
    }
}