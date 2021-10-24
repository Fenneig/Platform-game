using PixelCrew.Model.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using PixelCrew.Model.Definitions.Repository;
using PixelCrew.Model.Definitions.Repository.Items;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<ItemData> _inventory = new List<ItemData>();

        public int Size => _inventory.Count;

        public delegate void OnInventoryChanged(string id, int value);

        public OnInventoryChanged OnChanged;

        public void Add(string id, int value)
        {
            if (value <= 0) return;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;

            if (itemDef.HasTag(ItemTag.Stackable))
            {
                var item = GetItem(id);
                if (item == null)
                {
                    item = new ItemData(id);
                    _inventory.Add(item);
                }

                item.Value += value;
            }
            else
            {
                for (var i = 0; i < value; i++)
                {
                    var item = new ItemData(id) {Value = 1};
                    _inventory.Add(item);
                }
            }

            OnChanged?.Invoke(id, Count(id));
        }

        public void Remove(string id, int value)
        {
            if (value <= 0) return;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;

            var item = GetItem(id);
            if (item == null) return;

            if (itemDef.HasTag(ItemTag.Stackable))
            {
                item.Value -= value;

                if (item.Value <= 0) _inventory.Remove(item);
            }
            else
            {
                for (var i = 0; i < value; i++)
                {
                    _inventory.Remove(item);
                }
            }

            OnChanged?.Invoke(id, Count(id));
        }

        public bool IsContainStackableItem(ItemData item)
        {
            return DefsFacade.I.Items.Get(item.Id).HasTag(ItemTag.Stackable) && _inventory.Any(itemData => itemData.Id == item.Id);
        }

        public ItemData GetItem(string id)
        {
            return _inventory.FirstOrDefault(itemData => itemData.Id == id);
        }

        public ItemData[] GetAll(params ItemTag[] tags)
        {
            return (from item in _inventory let itemDef = DefsFacade.I.Items.Get(item.Id)
                let isAllRequirementsMet = tags.All(itemDef.HasTag)
                where isAllRequirementsMet select item).ToArray();
        }

        public ItemData[] GetAll()
        {
            return _inventory.ToArray();
        }

        public int Count(string id)
        {
            return _inventory.Where(item => item.Id == id).Sum(item => item.Value);
        }

        public bool IsEnough(params ItemWithCount[] items)
        {
            var joined = new Dictionary<string, int>();

            foreach (var item in items)
            {
                if (joined.ContainsKey(item.ItemId)) joined[item.ItemId] += item.Count;
                else joined.Add(item.ItemId, item.Count);
            }

            return !(from kvp in joined let count = Count(kvp.Key) where count < kvp.Value select kvp).Any();
        }
    }
}