using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHeal;

        public void ModifyHealthByDelta(int delta)
        {
            _health += delta;
            if (delta < 0)
            {
                _onDamage?.Invoke();
            }
            else if (delta > 0) 
            {
                _onHeal?.Invoke();
            }
            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
        }

        public int GetHealth() => _health; 
    }
}