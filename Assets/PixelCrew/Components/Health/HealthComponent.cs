﻿using PixelCrew.Model.Data.Properties;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private IntProperty _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private ChangeHealthEvent _onChange;

        private int _maxHealth;

        public int MaxHealth => _maxHealth;

        public bool IsInvulnerable { get; set; }

        private void Start()
        {
            _maxHealth = _health.Value;
            IsInvulnerable = false;
        }

        public IntProperty Health
        {
            get => _health;
            set => _health = value;
        }


        public void ModifyHealthByDelta(int delta)
        {
            if (_health.Value <= 0) return;
            if (delta < 0 && IsInvulnerable) return;

            _health.Value += delta;
            if (delta < 0)
            {
                _onDamage?.Invoke();
            }
            else if (delta > 0)
            {
                _onHeal?.Invoke();
            }

            if (_health.Value <= 0)
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
            _onChange?.Invoke(_health.Value);
        }

        [Serializable]
        public class ChangeHealthEvent : UnityEvent<int>
        {
        }
    }
}