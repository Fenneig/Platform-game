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

        private void Awake()
        {
            _animator = _button.GetComponent<Animator>();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            foreach (string tag in _tags)
            {
                if (collision.gameObject.CompareTag(tag))
                {
                    _animator.SetBool("is-pressing", true);
                    _pressedEvent.Invoke();
                }
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            foreach (string tag in _tags)
            {
                if (collision.gameObject.CompareTag(tag))
                {
                    _animator.SetBool("is-pressing", false);
                    _unpressedEvent.Invoke();
                }
            }
        }

    }
}