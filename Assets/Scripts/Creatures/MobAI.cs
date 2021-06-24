﻿using PixelCrew.Components;
using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures
{
    class MobAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;

        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private float _missHeroCooldown = 1f;

        private Coroutine _current;
        private GameObject _target;

        private Creature _creature;
        private Animator _animator;
        private SpawnListComponent _particles;
        private bool _isDead;
        private Patrol _patrol;

        private static readonly int IsDeadKey = Animator.StringToHash("is-dead");
        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
            _isDead = false;
        }

        private void Start()
        {
            StartState(_patrol?.DoPatrol());
        }

        public void OnHeroInVision(GameObject go)
        {
            if (_isDead) return;
            _target = go;
            StartState(AgroToHero());
        }

        private IEnumerator AgroToHero()
        {
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_alarmDelay);
            StartState(GoToHero());
        }

        private IEnumerator GoToHero()
        {
            while (_vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();
                }
                yield return null;
            }
            if (!_isDead)
            {
                _particles.Spawn("MissHero");
                yield return new WaitForSeconds(_missHeroCooldown);
                StartState(_patrol?.DoPatrol());
            }
        }

        private IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                StopMoving();
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }

            StartState(GoToHero());
        }

        public void OnDie()
        {
            _isDead = true;
            _animator.SetBool(IsDeadKey, true);

            StopAllCoroutines();
        }

        private void SetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;

            _creature.Direction = direction.normalized;
        }

        private void StartState(IEnumerator coroutine)
        {
            _creature.Direction = Vector2.zero;
            if (_current != null)
                StopCoroutine(_current);

            _current = StartCoroutine(coroutine);
        }

        private void StopMoving()
        {
            _creature.Direction = Vector2.zero;
        }

    }
}