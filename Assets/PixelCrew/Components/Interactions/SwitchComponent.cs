using System;
using UnityEngine;

namespace PixelCrew.Components.Interactions
{
    public class SwitchComponent : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private bool _state;
        [SerializeField] private string _animationKey;

        private bool _isLocked;

        private void Start()
        {
            Animate();
        }

        public void Switch()
        {
            if (_isLocked) return;
            
            _state = !_state;
            Animate();
        }

        private void Animate()
        {
            _animator.SetBool(_animationKey, _state);
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