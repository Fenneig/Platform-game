﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        [Serializable]
        public class AnimationClip
        {
            [SerializeField] private string _name;
            [SerializeField] private bool _loop;
            [SerializeField] private bool _allowNext;
            [SerializeField] private Sprite[] _sprites;
            [SerializeField] private UnityEvent _onComplete;
            public bool IsEqualName(string name) => _name == name;

            public string Name => _name;

            public bool Loop => _loop;

            public bool AllowNextClip => _allowNext;

            public Sprite[] Sprites => _sprites;

            public UnityEvent OnComplete => _onComplete;
        }

        [SerializeField] private int _frameRate;
        [SerializeField] private AnimationClip[] _clips;

        private float _secondsPerFrame;
        private float _nextFrameTime;
        private SpriteRenderer _renderer;
        private int _currentFrame;
        private int _currentClip;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _secondsPerFrame = 1f / _frameRate;
            StartAnimation();
        }

        private void StartAnimation()
        {
            _nextFrameTime = Time.time + _secondsPerFrame;
            _currentFrame = 0;
        }

        private void Update()
        {
            if (_nextFrameTime > Time.time) return;

            var clip = _clips[_currentClip];

            if (_currentFrame >= clip.Sprites.Length)
            {
                if (clip.Loop)
                {
                    _currentFrame = 0;
                }
                else
                {
                    clip.OnComplete?.Invoke();

                    if (clip.AllowNextClip)
                    {
                        _currentFrame = 0;
                        _currentClip = (int)Mathf.Repeat(_currentClip + 1, _clips.Length);
                    }
                }
                return;
            }

            _renderer.sprite = clip.Sprites[_currentFrame];
            _nextFrameTime += _secondsPerFrame;
            _currentFrame++;
        }

        public void SetClip(string name)
        {
            for (int i = 0; i < _clips.Length; i++)
            {
                if (_clips[i].IsEqualName(name))
                {
                    _currentClip = i;
                    StartAnimation();
                    return;
                }
            }
        }
    }
}