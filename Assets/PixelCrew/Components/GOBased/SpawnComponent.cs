﻿using System;
using UnityEngine;

namespace PixelCrew.Components.GOBased
{
    [Serializable]
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            if (_prefab.CompareTag("Attached"))
            {
                Instantiate(_prefab, _target.position, Quaternion.identity, gameObject.transform);
            }
            else
            {
                var rotation = new Quaternion(_prefab.transform.rotation.x,
                    _prefab.transform.rotation.y,
                    _prefab.transform.rotation.z * _target.lossyScale.x,
                    _prefab.transform.rotation.w);

                var position = _target.transform.position + _prefab.transform.position;

                var instance = Instantiate(_prefab, position, rotation);
                instance.transform.localScale = gameObject.transform.lossyScale;
            }
        }
    }
}