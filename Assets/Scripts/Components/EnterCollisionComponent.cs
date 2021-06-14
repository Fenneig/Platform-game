using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class EnterCollisionComponent : MonoBehaviour
    {
        [SerializeField] private string[] _tags;
        [SerializeField] private EnterEvent _action;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            foreach (string tag in _tags)
            {
                if (collision.gameObject.CompareTag(tag))
                {
                    _action?.Invoke(collision.gameObject);
                }
            }
        }

        [Serializable]
        public class EnterEvent : UnityEvent<GameObject>
        { }
    }
}