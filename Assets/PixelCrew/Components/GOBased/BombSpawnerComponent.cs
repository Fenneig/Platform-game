using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PixelCrew.Components.GOBased
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BombSpawnerComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _area;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _bombsAmount = 1;
        [SerializeField] private float _timeBetweenBombs = 1f;

        private float _halfAreaSize;

        private void Start()
        {
            _halfAreaSize = _area.GetComponent<BoxCollider2D>().size.x / 2f;
        }

        private void Spawn()
        {
            var randomXPos = Random.Range(-_halfAreaSize, _halfAreaSize);
            var areaTransform = _area.transform.position;
            var position = new Vector3(areaTransform.x + randomXPos, areaTransform.y, areaTransform.z);
            
            Instantiate(_prefab, position, Quaternion.identity);
        }
        
        public void StartSpawn()
        {
            StartCoroutine(MultiplySpawn());
        }

        private IEnumerator MultiplySpawn()
        {
            for (var i = 0; i < _bombsAmount; i++)
            {
                Spawn();
                yield return new WaitForSeconds(_timeBetweenBombs);
            }
        }
    }
}