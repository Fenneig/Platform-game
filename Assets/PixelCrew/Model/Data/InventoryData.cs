using PixelCrew.Model.Definitions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();

        public int InventorySize => _inventory.Count;

        public delegate void OnInventoryChanged(string id, int value);

        public OnInventoryChanged OnChanged;

        public void Add(string id, int value)
        {
            if (value <= 0) return;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;

            if (itemDef.IsStackable)
            {
                var item = GetItem(id);
                if (item == null)
                {
                    item = new InventoryItemData(id);
                    _inventory.Add(item);
                }

                item.Value += value;
            }
            else
            {
                for (int i = 0; i < value; i++)
                {
                    var item = new InventoryItemData(id) { Value = 1 };
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

            if (itemDef.IsStackable)
            {
                item.Value -= value;

                if (item.Value <= 0) _inventory.Remove(item);
            }
            else 
            {
                for (int i = 0; i < value; i++) 
                {
                    _inventory.Remove(item);
                }
            }

            OnChanged?.Invoke(id, Count(id));
        }

        public bool isContainStackableItem(InventoryItemData item)
        {
            if (DefsFacade.I.Items.Get(item.Id).IsStackable)
            {
                foreach (var itemData in _inventory)
                {
                    if (itemData.Id == item.Id) return true;
                }
            }
            return false;
        }

        private InventoryItemData GetItem(string id)
        {
            foreach (var itemData in _inventory)
            {
                if (itemData.Id == id) return itemData;
            }

            return null;
        }

        public int Count(string id)
        {
            var count = 0;

            foreach (var item in _inventory)
            {
                if (item.Id == id)
                    count += item.Value;
            }

            return count;
        }
    }
}