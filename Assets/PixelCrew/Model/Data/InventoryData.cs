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

        private int _maxInventorySize;

        public int MaxInventorySize
        {
            get { return _maxInventorySize; }
            set { _maxInventorySize = value; }
        }

        public int InventorySize => _inventory.Count;

        public delegate void OnInventoryChanged(string id, int value);

        public OnInventoryChanged OnChanged;

        public void Add(string id, int value, bool isStackable)
        {
            if (value <= 0) return;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;

            var item = GetItem(id);

            if (item == null || !isStackable)
            {
                item = new InventoryItemData(id);
                _inventory.Add(item);
                item.IsStackable = isStackable;
            }

            item.Value += value;

            OnChanged?.Invoke(id, Count(id));
        }

        public void Remove(string id, int value)
        {
            if (value <= 0) return;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;

            var item = GetItem(id);

            if (item == null) return;

            item.Value -= value;

            OnChanged?.Invoke(id, Count(id));


            if (item.Value <= 0) _inventory.Remove(item);
        }

        public bool isContainStackableItem(InventoryItemData item)
        {
            if (item.IsStackable)
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