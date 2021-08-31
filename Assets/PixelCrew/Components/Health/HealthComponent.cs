using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private ChangeHealthEvent _onChange;


        public void ModifyHealthByDelta(int delta)
        {
            if (_health <= 0) return;

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
            else
            {
                UpdateHealth();
            }
        }

        private void UpdateHealth()
        {
            _onChange?.Invoke(_health);
        }

        public int Health
        {
            get => _health;
            set => _health = value;
        }

        [Serializable]
        public class ChangeHealthEvent : UnityEvent<int>
        {
        }

    }


}