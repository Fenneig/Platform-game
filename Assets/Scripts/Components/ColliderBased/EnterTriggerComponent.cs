using PixelCrew.Utils;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.ColliderBased
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string[] _tags;
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private EnterEvent _action;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_tags.Length == 0)
            {
                if (collision.gameObject.IsInLayer(_layer)) InvokeAction(collision);
            }
            else
            {
                foreach (string tag in _tags)
                {
                    if (collision.gameObject.CompareTag(tag)) InvokeAction(collision);
                }
            }
        }

        private void InvokeAction(Collider2D collision)
        {
            _action?.Invoke(collision.gameObject);
        }

        [Serializable]
        public class EnterEvent : UnityEvent<GameObject>
        { }
    }
}