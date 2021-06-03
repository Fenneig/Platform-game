using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class CollectObjectComponent : MonoBehaviour
    {
        //Сбор объектов, в зависимости от добавления тэгов будет расширяться функционал скрипта
        [SerializeField] private Hero _hero;
        //Заглушка, чтобы не собирать один предмет дважды пока идет анимация исчезновения.
        private bool collected = false;
        //[SerializeField] private UnityEvent _destroyAction;
        private void CollectObject() 
        {
            if (!collected)
            {
                switch (tag)
                {
                    case "Gold":
                        {
                            _hero.CollectCoin(10);
                            _hero.SayCoins();
                            break;
                        }
                    case "Silver":
                        {
                            _hero.CollectCoin(1);
                            _hero.SayCoins();
                            break;
                        }
                    case "hp":
                        {
                            _hero.SayHp();
                            break;
                        }
                    case "sp":
                        {
                            _hero.RecoverExtraMoves();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                collected = true;
            }
        }

    }
}