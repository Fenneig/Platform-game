using PixelCrew.Utils;
using System;
using UnityEngine;

namespace PixelCrew.Components.GOBased
{
    [Serializable]
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

        //объекты с тегом Attached имеют внутренюю привязку в проекте и их нельзя крутить при создании.
        [ContextMenu("Spawn")]
        public void Spawn()
        {
            if (_prefab.CompareTag("Attached"))
            {
                Instantiate(_prefab, _target.position, Quaternion.identity, gameObject.transform);
            }
            else
            {
                var quaternion = _prefab.transform.rotation;
                var rotation = new Quaternion(quaternion.x, quaternion.y, quaternion.z * _target.lossyScale.x,
                    quaternion.w);
                var position = _target.transform.position + _prefab.transform.position;

                var instance = SpawnUtils.Spawn(_prefab, position, rotation);
                instance.transform.localScale = gameObject.transform.lossyScale;
            }
        }

        internal void SetPrefab(GameObject prefab)
        {
            _prefab = prefab;
        }
    }
}