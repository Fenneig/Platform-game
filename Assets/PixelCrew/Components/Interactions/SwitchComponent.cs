using UnityEngine;

namespace PixelCrew.Components.Interactions
{
    public class SwitchComponent : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private bool _state;
        [SerializeField] private string _animationKey;

        private bool _isLocked;

        public void Switch()
        {
            if (!_isLocked)
            {
                _state = !_state;
                _animator.SetBool(_animationKey, _state);
            }
        }

        public void LockSwitching() 
        {
            _isLocked = true;
        }

        [ContextMenu("Switch")]
        public void SwitchIt()
        {
            Switch();
        }
    }
}