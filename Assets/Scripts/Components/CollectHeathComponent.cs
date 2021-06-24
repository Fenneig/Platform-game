using UnityEngine;
using PixelCrew.Creatures;

namespace PixelCrew.Components
{
    public class CollectHeathComponent : MonoBehaviour
    {
        private Hero _hero;
        private int _health;

        private void Start()
        {
            _health = GetComponent<ModifyHealthComponent>().Delta;
            _hero = FindObjectOfType<Hero>();
        }

        public void Collect() => _hero.OnHealthChanged(_health);
    }
}
