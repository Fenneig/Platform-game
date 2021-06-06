using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Components
{
    [Serializable]
    public class ComponentToSpawn
    {
        public Transform _target;
        public GameObject _prefab;
    }

    public class MultipleSpawnComponent : MonoBehaviour
    {
        [SerializeField] private ComponentToSpawn[] _components;

        public void Spawn() 
        {
            foreach (ComponentToSpawn component in _components) 
            {
                Instantiate(component._prefab, component._target.position, Quaternion.identity);
            }
        }
    }

}
