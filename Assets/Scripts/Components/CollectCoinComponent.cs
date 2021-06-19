using UnityEngine;
using PixelCrew.Creatures;

namespace PixelCrew.Components
{
    public class CollectCoinComponent : MonoBehaviour
    {
        [SerializeField] int _value;
        private Hero _hero;

        private void Awake()
        {
            _hero = FindObjectOfType<Hero>();
        }

        public void Collect() => _hero.CollectCoin(_value);
    }
}