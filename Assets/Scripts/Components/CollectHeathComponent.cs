using UnityEngine;

namespace PixelCrew.Components
{
    public class CollectHeathComponent : MonoBehaviour
    {
        private Hero _hero;
        private void Awake()
        {
            _hero = FindObjectOfType<Hero>();
        }

        public void Collect() => _hero.SayHp();
    }
}
