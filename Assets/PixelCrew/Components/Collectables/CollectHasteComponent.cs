using PixelCrew.Creatures.Hero;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Collectables
{
    public class CollectHasteComponent : MonoBehaviour
    {
        [SerializeField] private float _hasteMultiplier = 3f;
        [SerializeField] private float _hasteTime = 1f;
        [SerializeField] private UnityEvent _destroyObject;
        private Hero _hero;

        private void Awake()
        {
            _hero = FindObjectOfType<Hero>();
        }

        public void Collect()
        {
            _hero.Speed *= _hasteMultiplier;
            StartCoroutine(HasteExpires());
        }

        private IEnumerator HasteExpires()
        {
            yield return new WaitForSeconds(_hasteTime);
            _hero.Speed /= _hasteMultiplier;
            Destroy();
        }

        private void Destroy() 
        {
            _destroyObject?.Invoke();
        }
    }
}