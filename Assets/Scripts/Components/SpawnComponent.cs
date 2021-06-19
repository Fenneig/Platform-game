using System;
using UnityEngine;

namespace PixelCrew.Components
{
    [Serializable]
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

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

                var instance = Instantiate(_prefab, _target.position, rotation);
                instance.transform.localScale = gameObject.transform.lossyScale;
            }
        }
    }
}
