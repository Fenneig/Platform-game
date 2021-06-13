using UnityEngine;

namespace PixelCrew.Components
{
    public class CollectCoinComponent : MonoBehaviour
    {
        [SerializeField] int value;
        private Hero _hero;

        private void Awake()
        {
            _hero = FindObjectOfType<Hero>();
        }

        public void Collect() => _hero.CollectCoin(value);
    }
}