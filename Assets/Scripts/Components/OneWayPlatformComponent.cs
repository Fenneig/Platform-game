using UnityEngine;

namespace PixelCrew.Components
{
    [RequireComponent(typeof(Collider2D))]
    public class OneWayPlatformComponent : MonoBehaviour
    {
        //Компонента проверяющий хочет ли игрок упасть с односторонних платформ.
        private Hero _hero;
        private void Awake()
        {
            _hero = FindObjectOfType<Hero>();
        }
        private void Update()
        {
            if (_hero.Direction.y < 0)
            {
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), _hero.GetComponent<Collider2D>(), true);
                Invoke("IgnoreOff", 0.2f);
            }
        }
        private void IgnoreOff()
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), _hero.GetComponent<Collider2D>(), false);
        }
    }
}

