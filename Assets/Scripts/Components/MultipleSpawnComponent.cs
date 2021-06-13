using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Components
{
    [Serializable]
    public class ComponentToSpawn
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

        public Transform Target { get => _target; }

        public GameObject Prefab { get => _prefab; }

    }
    public class MultipleSpawnComponent : MonoBehaviour
    {
        [SerializeField] private ComponentToSpawn[] _components;

        public void Spawn()
        {
            foreach (ComponentToSpawn component in _components)
            {
                Instantiate(component.Prefab, component.Target.position, Quaternion.identity);
            }
        }
    }

}
