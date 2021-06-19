using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew
{
    public class PressingButtonComponent : MonoBehaviour
    {
        [SerializeField] private string[] _tags;
        [SerializeField] private GameObject _button;
        [SerializeField] private UnityEvent _pressedEvent;
        [SerializeField] private UnityEvent _unpressedEvent;

        private Animator _animator;
        private string _tagOnButton = null;

        private void Awake()
        {
            _animator = _button.GetComponent<Animator>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_tagOnButton == null)
            {
                foreach (string tag in _tags)
                {
                    if (collision.CompareTag(tag))
                    {
                        _tagOnButton = tag;
                        _animator.SetBool("is-pressing", true);
                        _pressedEvent.Invoke();
                        break;
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_tagOnButton != null)
            {
                if (collision.CompareTag(_tagOnButton))
                {
                    _tagOnButton = null;
                    _animator.SetBool("is-pressing", false);
                    _unpressedEvent.Invoke();
                }
            }
        }
    }
}