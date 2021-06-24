using UnityEngine;
using PixelCrew.Creatures;

namespace PixelCrew.Components
{
    public class ArmHeroComponent : MonoBehaviour
    {
        [SerializeField] private int _swordsAmount = 1;
        public void ArmHero(GameObject go)
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