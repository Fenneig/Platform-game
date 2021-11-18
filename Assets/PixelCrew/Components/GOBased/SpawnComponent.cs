using PixelCrew.Utils;
using System;
using PixelCrew.Utils.ObjectPool;
using UnityEngine;

namespace PixelCrew.Components.GOBased
{
    [Serializable]
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private bool _usePool;

        public Transform InstanceTransform { get; set; }

        //объекты с тегом Attached имеют внутренюю привязку в проекте и их нельзя крутить при создании.
        [ContextMenu("Spawn")]
        public void Spawn()
        {
            if (_prefab.CompareTag("Attached"))
            {
                var instance = Instantiate(_prefab, _target.position, Quaternion.identity, gameObject.transform);
                InstanceTransform = instance.transform;
            }
            else
            {
                var quaternion = _prefab.transform.rotation;
                var rotation = new Quaternion(quaternion.x, quaternion.y, quaternion.z * _target.lossyScale.x,
                    quaternion.w);
                var position = _target.transform.position + _prefab.transform.position;

                var instance = _usePool 
                    ? Pool.Instance.Get(_prefab, position, rotation) 
                    : SpawnUtils.Spawn(_prefab, position, rotation);
                instance.transform.localScale = gameObject.transform.lossyScale;
                InstanceTransform = instance.transform;
            }
        }

        internal void SetPrefab(GameObject prefab)
        {
            _prefab = prefab;
        }
    }
}