using UnityEngine;
using System;

namespace PixelCrew.Components.GOBased
{
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
                        Vector3 newPosition = component.Target.position + new Vector3(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value, 0);
                        var instance = Instantiate(component.Prefab, component.Target.position, Quaternion.identity);

                        var rigidbody = instance.GetComponent<Rigidbody2D>();
                        if (rigidbody != null) rigidbody.AddForce(Vector2.up);

                        var moveObject = instance.GetComponent<MoveObjectComponent>();
                        if (moveObject != null) moveObject.MoveToObject(instance, newPosition, _moveTime);
                    }
                }
            }
        }
    }

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
}
