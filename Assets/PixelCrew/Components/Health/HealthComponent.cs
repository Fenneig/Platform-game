using PixelCrew.Model.Data.Properties;
using System;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private IntProperty _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] public UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private ChangeHealthEvent _onChange;
        private readonly Lock _isInvulnerable = new Lock();

        public int MaxHealth { get; set; }

        public Lock IsInvulnerable => _isInvulnerable;

        private void Awake()
        {
            MaxHealth = _health.Value;
            _isInvulnerable.Release(this);
        }

        public IntProperty Health
        {
            get => _health;
            set => _health = value;
        }

        public void ModifyHealthByDelta(int delta)
        {
            if (_health.Value <= 0) return;
            if (delta < 0 && IsInvulnerable.IsLocked) return;
            if (_health.Value + delta > MaxHealth) delta = MaxHealth - _health.Value;

            _health.Value += delta;
            if (delta < 0) _onDamage?.Invoke();
            else if (delta > 0) _onHeal?.Invoke();

            if (_health.Value <= 0) _onDie?.Invoke();
            else UpdateHealth();
        }

        private void UpdateHealth()
        {
            _onChange?.Invoke(_health.Value);
        }

        [Serializable]
        public class ChangeHealthEvent : UnityEvent<int>
        {
        }
    }
}