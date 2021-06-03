using UnityEngine;

namespace PixelCrew.Components
{
    public class HighGravityZoneComponent : MonoBehaviour
    {
        //при входе в зону увеличивает гравитацию работующую на персонажа, при выходе убирает её
        //Если буду использовать больше 1 раза gravityScale модификатор нужно будет в SerializeField вынести
        [SerializeField] Hero _hero;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(_hero.tag))
                _hero.GetComponent<Rigidbody2D>().gravityScale *= 1.75f;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(_hero.tag))
                _hero.GetComponent<Rigidbody2D>().gravityScale = _hero.GetDefaultGravity();
        }
    }
}
