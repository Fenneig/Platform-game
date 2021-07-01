using UnityEngine;
using PixelCrew.Utils;

namespace PixelCrew.Components.ColliderBased
{
    public class JumpFromPlatformComponent : MonoBehaviour
    {
        //Компонента проверяющий хочет ли существо упасть с односторонних платформ.
        [SerializeField] private LayerMask _platformLayer;
        private Collider2D _platformToIgnore;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.IsInLayer(_platformLayer))
            {
                _platformToIgnore = collision.gameObject.GetComponent<Collider2D>();
            }
        }
        public void JumpOff()
        {
            if (_platformToIgnore != null)
            {
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), _platformToIgnore, true);
                Invoke(nameof(StopIgnore), 0.2f);
            }
        }
        private void StopIgnore()
        {
            if (_platformToIgnore != null)
            {
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), _platformToIgnore, false);
                _platformToIgnore = null;
            }
        }
    }
}

