using PixelCrew.Creatures.Hero;
using UnityEngine;

namespace PixelCrew.Components.GOBased
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ThrowObjectFromHero : MonoBehaviour
    {
        private Hero _hero;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _hero = FindObjectOfType<Hero>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            var force = gameObject.transform.position - _hero.transform.position;
            force.Normalize();

            _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
