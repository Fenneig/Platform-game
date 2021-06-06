using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class CollectObjectComponent : MonoBehaviour
    {
        //Заглушка, чтобы не собирать один предмет дважды пока идет анимация исчезновения.
        private bool collected = false;

        public void CollectObject()
        {
            Hero hero = GameObject.Find("Hero").GetComponent<Hero>();
            if (!collected)
            {
                switch (tag)
                {
                    case "Gold":
                        {
                            hero.CollectCoin(10);
                            hero.SayCoins();
                            break;
                        }
                    case "Silver":
                        {
                            hero.CollectCoin(1);
                            hero.SayCoins();
                            break;
                        }
                    case "hp":
                        {
                            hero.SayHp();
                            break;
                        }
                    case "sp":
                        {
                            hero.RecoverExtraMoves();
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