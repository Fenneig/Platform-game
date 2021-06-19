using System;
using UnityEngine;

namespace PixelCrew.Components
{
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

    [Serializable]
    public class ComponentToSpawn
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

        public Transform Target => _target;

        public GameObject Prefab => _prefab;

    }
}
