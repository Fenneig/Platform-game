using UnityEngine;

namespace PixelCrew.Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject[] _prefab;

        public void Spawn(string tag)
        {
            for (int i = 0; i < _prefab.Length; i++)
            {
                if (_prefab[i].CompareTag(tag))
                {
                    var prefabPosition = _target.position + _prefab[i].transform.position;
                    var prefabRotation = _prefab[i].transform.rotation;

                    if (_target.lossyScale.x < 0)
                    {
                        prefabPosition = new Vector3(prefabPosition.x - 2 * _prefab[i].transform.position.x, prefabPosition.y, prefabPosition.z);
                        prefabRotation = new Quaternion(_prefab[i].transform.rotation.x,
                            _prefab[i].transform.rotation.y,
                            _prefab[i].transform.rotation.z * -1,
                            _prefab[i].transform.rotation.w);

                    }

                    var instance = Instantiate(_prefab[i], prefabPosition, prefabRotation);
                    instance.transform.localScale = _target.lossyScale;
                }
            }
        }
    }
}