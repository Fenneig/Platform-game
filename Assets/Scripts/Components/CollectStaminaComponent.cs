using UnityEngine;
using PixelCrew.Creatures;

namespace PixelCrew.Components
{
    public class CollectStaminaComponent : MonoBehaviour
    {
        private Hero _hero;

        private void Awake()
        {
            _hero = FindObjectOfType<Hero>();
        }

        public void Collect() => _hero.RecoverExtraMoves();

    }
}