using UnityEngine;

namespace PixelCrew.Components
{
    public class OneWayPlatformComponent : MonoBehaviour
    {
        //Компонента проверяющий хочет ли игрок упасть с односторонних платформ.
        //при получении отрицательного вектора отключаем коллизию у двух объектов с указанными слоями.
        //!!важно!! не менять слои а если и менять, то исправлять данный скрипт
        private Hero _hero;
        private void Awake()
        {
            _hero = GameObject.Find("Hero").GetComponent<Hero>();
        }
        private void Update()
        {
            if (_hero.Direction.y < 0)
            {
                Physics2D.IgnoreLayerCollision(10, 11, true);
                Invoke("IgnoreOff", 0.2f);
            }
        }
        private void IgnoreOff()
        {
            Physics2D.IgnoreLayerCollision(10, 11, false);
        }
    }
}

