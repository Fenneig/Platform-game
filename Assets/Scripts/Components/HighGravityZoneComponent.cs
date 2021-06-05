using UnityEngine;

namespace PixelCrew.Components
{
    public class HighGravityZoneComponent : MonoBehaviour
    {
        //при входе в зону увеличивает гравитацию работующую на персонажа, при выходе убирает её
        //Если буду использовать больше 1 раза gravityScale модификатор нужно будет в SerializeField вынести
        [SerializeField] private Hero _hero;
        [SerializeField] private float _gravityScale;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(_hero.tag))
            {
                _hero.CurrentGravity = _hero.CurrentGravity * _gravityScale;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(_hero.tag))
            {
                _hero.CurrentGravity = _hero.CurrentGravity / _gravityScale;
            }
        }
    }
}
