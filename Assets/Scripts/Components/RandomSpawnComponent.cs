﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Utils;

namespace PixelCrew.Components
{
    [Serializable]
    public class ChanceObjectToSpawn
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;
        [Range(0, 100)] [SerializeField] private float _chanceToSpawn;
        [Min(1)] [SerializeField] private int _amout = 1;

        public Transform Target { get => _target; }
        public GameObject Prefab { get => _prefab; }
        public float ChanceToSpawn { get => _chanceToSpawn; }
        public int Amout { get => _amout; }

    }
    public class RandomSpawnComponent : MonoBehaviour
    {
        [SerializeField] private ChanceObjectToSpawn[] _components;
        [SerializeField] private float _moveTime;

        public void Spawn()
        {
            foreach (ChanceObjectToSpawn component in _components)
            {
                for (int i = 0; i < component.Amout; i++)
                {
                    float chance = UnityEngine.Random.value * 100;
                    if (chance <= component.ChanceToSpawn)
                    {
                        Vector3 newPosition = component.Target.position + new Vector3(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value / 2, 0);
                        var instance = Instantiate(component.Prefab, gameObject.transform.position, Quaternion.identity);

                        instance?.GetComponent<MoveObjectComponent>().MoveToObject(instance, newPosition, _moveTime);
                    }
                }
            }
        }
    }
}