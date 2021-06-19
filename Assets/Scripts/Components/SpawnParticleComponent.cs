using UnityEngine;

namespace PixelCrew.Components
{
    public class SpawnParticleComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject[] _prefab;

        public void Spawn(string tag)
        {
            for (int i = 0; i < _prefab.Length; i++)
            {
                if (!_prefab[i].CompareTag(tag)) continue;

                var rotation = new Quaternion(_prefab[i].transform.rotation.x,
                    _prefab[i].transform.rotation.y,
                    _prefab[i].transform.rotation.z * _target.lossyScale.x,
                    _prefab[i].transform.rotation.w);


                if (tag == "Attack")
                {
                    Instantiate(_prefab[i], _target.position, Quaternion.identity, gameObject.transform);
                }
                else
                {
                    GameObject instance;
                    var position = _target.position + _prefab[i].transform.position;
                    instance = Instantiate(_prefab[i], position, rotation);
                    instance.transform.localScale = _target.lossyScale;
                }
            }
        }
    }
}