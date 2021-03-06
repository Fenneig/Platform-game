using UnityEngine;
using PixelCrew.Creatures.Hero;

namespace PixelCrew.Components.Collectables
{
    public class CollectStaminaComponent : MonoBehaviour
    {
        private Hero _hero;

        private void Start()
        {
            _hero = FindObjectOfType<Hero>();
        }

        public void Collect() => _hero.RecoverExtraMoves();
    }
}