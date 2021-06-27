using UnityEngine;
using PixelCrew.Creatures.Hero;

namespace PixelCrew.Components.Collectables
{
    public class CollectSwordComponent : MonoBehaviour
    {
        [SerializeField] private int _swordsAmount = 1;
        public void Collect(GameObject go)
        {
            var hero = go.GetComponent<Hero>();
            if (hero != null)
            {
                hero.CollectSword(_swordsAmount);
                hero.ArmHero();
            }
        }
    }
}