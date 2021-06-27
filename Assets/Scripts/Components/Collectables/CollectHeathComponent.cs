using UnityEngine;
using PixelCrew.Creatures.Hero;

namespace PixelCrew.Components.Collectables
{
    public class CollectHeathComponent : MonoBehaviour
    {
        private Hero _hero;

        private void Start()
        {
            _hero = FindObjectOfType<Hero>();
        }

        public void Collect() => _hero.OnHealthChanged();
    }
}
