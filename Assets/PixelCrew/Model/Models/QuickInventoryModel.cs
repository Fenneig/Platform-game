using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;
using System;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions.Repository;
using UnityEngine;

namespace PixelCrew.Model.Models
{
    public class QuickInventoryModel : IDisposable
    {
        private readonly PlayerData _data;
        public ItemData[] Inventory { get; private set; }

        public readonly IntProperty SelectedIndex = new IntProperty();

        public event Action OnChanged;

        public ItemData SelectedItem
        {
            get
            {
                if (Inventory.Length > 0 && Inventory.Length > SelectedIndex.Value)
                    return Inventory[SelectedIndex.Value];
                
                return null;
            }
        }

        public ItemDef SelectedDef => DefsFacade.I.Items.Get(SelectedItem?.Id);

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public QuickInventoryModel(PlayerData data)
        {
            _data = data;
            Inventory = _data.QuickInventory.GetAll();
            _data.QuickInventory.OnChanged += OnChangedInventory;
        }

        private void OnChangedInventory(string id, int value)
        {
            Inventory = _data.QuickInventory.GetAll();
            SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, Inventory.Length - 1);
            OnChanged?.Invoke();
        }

        public void SetNextItem(float direction)
        {
            SelectedIndex.Value = (int) Mathf.Repeat(SelectedIndex.Value + direction, Inventory.Length);
        }

        public void Dispose()
        {
            _data.QuickInventory.OnChanged -= OnChangedInventory;
        }
    }
}