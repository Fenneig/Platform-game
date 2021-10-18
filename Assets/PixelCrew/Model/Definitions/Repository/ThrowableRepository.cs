using System;
using PixelCrew.Model.Definitions.Repository.Items;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repository
{
    [CreateAssetMenu(menuName = "Defs/Repository/Throwable", fileName = "Throwable")]
    public class ThrowableRepository : DefRepository<ThrowableDef>
    {
    }

    [Serializable]
    public struct ThrowableDef : IHaveId
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private GameObject _projectile;
        [SerializeField] private int _baseDamage;
    
        public string Id => _id;
        public GameObject Projectile => _projectile;
        public int BaseDamage => _baseDamage;
    }
}