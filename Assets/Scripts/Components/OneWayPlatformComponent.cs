using UnityEngine;

namespace PixelCrew.Components
{
    public class OneWayPlatformComponent : MonoBehaviour
    {
        //метод проверяющий хочет ли игрок упасть с односторонних платформ. При дабавление новых нужно будет переименновать метод (One Way collision platforms)
        [SerializeField] private Hero _hero;

        //при получении отрицательного вектора отключаем коллизию у двух объектов с указанными слоями.
        //!!важно!! не менять слои а если и менять, то исправлять данный скрипт
        //Нужно прочитать документацию и узнать можно ли сделать иную привязку
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

