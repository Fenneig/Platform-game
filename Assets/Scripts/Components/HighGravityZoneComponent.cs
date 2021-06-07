using UnityEngine;

namespace PixelCrew.Components
{
    public class HighGravityZoneComponent : MonoBehaviour
    {
        //при входе в зону увеличивает гравитацию работующую на персонажа, при выходе убирает её
        //Если буду использовать больше 1 раза gravityScale модификатор нужно будет в SerializeField вынести
        [SerializeField] private string[] _tags;
        [SerializeField] private float _gravityScale;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            foreach (string tag in _tags)
            {
                if (collision.CompareTag(tag))
                    collision.GetComponent<ChangeGravityScaleComponent>().MultiplyGravityByValue(_gravityScale);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            foreach (string tag in _tags)
            {
                if (collision.CompareTag(tag))
                    collision.GetComponent<ChangeGravityScaleComponent>().DivideGravityByValue(_gravityScale);
            }
        }
    }
}
