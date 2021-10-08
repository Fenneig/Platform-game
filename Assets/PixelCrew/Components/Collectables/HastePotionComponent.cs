using PixelCrew.Creatures.Hero;
using UnityEngine;

namespace PixelCrew.Components.Collectables
{
    public class HastePotionComponent : MonoBehaviour, IUsable
    {
        [SerializeField] private float _speedBonus = 6f;
        [SerializeField] private float _hasteTime = 3f;

        public void Use(GameObject target)
        {
            target.GetComponent<Hero>().DrinkHastePotion(_speedBonus, _hasteTime);
        }
    }
}