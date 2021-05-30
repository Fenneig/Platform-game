﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class DamageComponent : MonoBehaviour
    {
        [SerializeField] private int _damageValue;

        public void ApplyDamage(GameObject target) 
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            
            if (healthComponent != null) healthComponent.ApplyDamage(_damageValue);
        }
    }
}