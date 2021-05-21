using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class PalmCrownComponent : MonoBehaviour
    {
        [SerializeField] private Hero _hero;
        private PlatformEffector2D _effector;
        private float _waitTime = 0.01f;

        private void Awake()
        {
            _effector = GetComponent<PlatformEffector2D>();
        }

        private void Update()
        {
            if (_hero.GetDirection().y < 0) 
            {
                if (_waitTime <= 0)
                {
                    _effector.rotationalOffset = 180f;
                    _waitTime = 0.01f;
                }
                else {
                    _waitTime -= Time.deltaTime;
                }
            }

            if (_hero.GetDirection().y > 0) 
            {
                _waitTime = 0.01f;
                _effector.rotationalOffset = 0f;
            }
        }
    }
}

