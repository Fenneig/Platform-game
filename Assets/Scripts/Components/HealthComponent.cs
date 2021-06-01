using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{

    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;

        public void ChangeHealth(int value)
        {
            _health += value;
            if (_health <= 0)
            {
                _onDie.Invoke();
            }
            else 
            {
                if (value < 0) _onDamage.Invoke();
            }

        }

        public int GetHealth() => _health; 
    }
}