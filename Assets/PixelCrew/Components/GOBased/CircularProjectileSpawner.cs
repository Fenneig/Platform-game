using System;
using System.Collections;
using PixelCrew.Creatures.Weapons;
using PixelCrew.Utils;
using Unity.Mathematics;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PixelCrew.Components.GOBased
{
    public class CircularProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private CircularProjectileSettings[] _settings;
        [SerializeField] private float _sectorAngle = 60;
        [SerializeField] private int _itemPerBurst = 3;

        public int Stage { get; set; }

        [ContextMenu("Launch!")]
        public void LaunchProjectile()
        {
            StartCoroutine(SpawnProjectile());
        }

        private IEnumerator SpawnProjectile()
        {
            var settings = _settings[Stage];
            var sectorStep = _sectorAngle / settings.BurstCount;

            for (var i = 0; i < settings.BurstCount;)
            {
                var itemToBurst = Mathf.Min(_itemPerBurst, settings.BurstCount - i);
                for (var j = 0; j < itemToBurst; j++)
                {
                    var angle = (180 - _sectorAngle) / 2 + sectorStep * (i + j);
                    var direction = GetUnitOnCircle(angle);

                    var instance = SpawnUtils.Spawn(settings.Prefab, transform.position, quaternion.identity);
                    var projectile = instance.GetComponent<DirectionalProjectile>();
                    projectile.Launch(direction);
                }

                i += itemToBurst;
                yield return new WaitForSeconds(settings.Delay);
            }
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var position = transform.position;

            var middleAngleDelta = (180 - _sectorAngle) / 2;
            var temp = GetUnitOnCircle(middleAngleDelta);
            var rightBound = new Vector3(temp.x, temp.y, 0);
            Handles.DrawLine(position, position + rightBound);

            temp = GetUnitOnCircle(middleAngleDelta + _sectorAngle);
            var leftBound = new Vector3(temp.x, temp.y, 0);
            Handles.DrawLine(position, position + leftBound);
            Handles.DrawWireArc(position, Vector3.forward, rightBound, _sectorAngle, 1);

            Handles.color = new Color(1f, 1f, 1f, 0.1f);
            Handles.DrawSolidArc(position, Vector3.forward, rightBound, _sectorAngle, 1);
        }
#endif
        private Vector2 GetUnitOnCircle(float angle)
        {
            var angleRadians = angle * Mathf.PI / 180.0f;
            var x = Mathf.Cos(angleRadians);
            var y = Mathf.Sin(angleRadians);

            return new Vector2(x, y);
        }
    }

    [Serializable]
    public struct CircularProjectileSettings
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _burstCount;
        [SerializeField] private float _delay;

        public GameObject Prefab => _prefab;

        public int BurstCount => _burstCount;

        public float Delay => _delay;
    }
}