using UnityEngine;

namespace PixelCrew.Components
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string[] _tags;
        [SerializeField] private EnterEvent _action;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            foreach (string tag in _tags)
            {
                if (collision.gameObject.CompareTag(tag))
                {
                    _action?.Invoke(collision.gameObject);
                }
            }
        }
    }
}