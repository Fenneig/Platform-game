using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Interactions
{
    public class PressingButtonComponent : MonoBehaviour
    {
        [SerializeField] private string[] _tags;
        [SerializeField] private GameObject _button;
        [SerializeField] private UnityEvent _pressedEvent;
        [SerializeField] private UnityEvent _unpressedEvent;

        private Animator _animator;
        private string _tagOnButton;
        private static readonly int IsPressingKey = Animator.StringToHash("is-pressing");

        private void Awake()
        {
            _animator = _button.GetComponent<Animator>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!string.IsNullOrEmpty(_tagOnButton)) return;
            
            foreach (var tag in _tags)
            {
                if (!collision.CompareTag(tag)) continue;
                
                _tagOnButton = tag;
                _animator.SetBool(IsPressingKey, true);
                _pressedEvent.Invoke();
                break;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!string.IsNullOrEmpty(_tagOnButton) || !collision.CompareTag(_tagOnButton)) return;
            
            _tagOnButton = null;
            _animator.SetBool(IsPressingKey, false);
            _unpressedEvent.Invoke();
        }
    }
}