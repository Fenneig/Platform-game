using System;
using UnityEngine;

namespace PixelCrew.Utils
{
    [Serializable]
    public class Timer
    {
        [SerializeField] private float _value;
        private float _timeUp;

        public void Reset()
        {
            _timeUp = Time.time + _value;
        }

        public bool IsReady => _timeUp <= Time.time;
    }
}
