﻿using System;
using UnityEngine;

namespace PixelCrew.Utils
{
    //Таймер, после запуска(Reset) отсчитывает время до значения value 
    [Serializable]
    public class Timer
    {
        [SerializeField] private float _value;
        private float _timeUp;

        public void Reset()
        {
            _timeUp = Time.time + _value;
        }

        public void EarlyComplete() 
        {
            _timeUp -= _value;
        }

        public bool IsReady => _timeUp <= Time.time;
    }
}