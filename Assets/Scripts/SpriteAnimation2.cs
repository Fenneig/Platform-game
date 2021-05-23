using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew
{

    public class SpriteAnimation2 : MonoBehaviour
    {
        [Serializable]
        public struct State
        {
            public string _name;
            public bool _loop;
            public bool _allowNext;
            public Sprite[] _sprites;
            public bool IsEqualName(string name) => _name == name;
        }

        [SerializeField] private int _frameRate;
        [SerializeField] private UnityEvent _onComplete;
        [SerializeField] private State[] _states;


        private float _secondsPerFrame;
        private float _nextFrameTime;

        private SpriteRenderer _renderer;
        private int _currentSpriteIndex;
        private State _currentState;
        private int _currentStateIndex;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _secondsPerFrame = 1f / _frameRate;
            _nextFrameTime = Time.time + _secondsPerFrame;
            _currentSpriteIndex = 0;
            _currentStateIndex = 0;
            _currentState = _states[_currentStateIndex];
        }
        private void Update()
        {
            if (_nextFrameTime > Time.time) return;

            if (_currentSpriteIndex >= _currentState._sprites.Length)
            {
                if (_currentState._loop)
                {
                    _currentSpriteIndex = 0;
                }
                else if (_currentState._allowNext && !_currentState._loop)
                {
                    SetClip(_states[++_currentStateIndex]._name);
                }
                else if (!_currentState._allowNext && !_currentState._loop)
                {
                    enabled = false;
                    _onComplete?.Invoke();
                    return;
                }
            }

            _renderer.sprite = _currentState._sprites[_currentSpriteIndex];
            _nextFrameTime += _secondsPerFrame;
            _currentSpriteIndex++;
        }

        public void SetClip(string name)
        {
            foreach (State state in _states)
            {
                if (state.IsEqualName(name)) _currentState = state;
            }
        }
    }
}
