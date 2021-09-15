using System;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/UsableItemDef", fileName = "UsableItem")]
    public class UsableItemDef : ScriptableObject
    {
        [SerializeField] private UsableDef[] _items;

        public UsableDef Get(string id)
        {
            foreach (var itemDef in _items)
            {
                if (itemDef.Id == id) return itemDef;
            }

            return default;
        }

        [Serializable]
        public struct UsableDef
        {
            [InventoryId] [SerializeField] private string _id;
            [SerializeField] private GameObject _usableItem;
            [SerializeField] private UseTypeTag _tag;

            public string Id => _id;
            public GameObject UsableItem => _usableItem;

            public UseTypeTag Tag => _tag;
        }
    }
}