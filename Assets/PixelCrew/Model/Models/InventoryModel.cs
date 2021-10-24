using System;
using System.Collections.Generic;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.Model.Models
{
    [Serializable]
    public class InventoryModel : IDisposable
    {
        private readonly PlayerData _data;

        public readonly StringProperty InterfaceSelection = new StringProperty();
        public readonly List<StringProperty> QuickInventorySelection = new List<StringProperty>();
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        public event Action OnChanged;

        public InventoryModel(PlayerData data)
        {
            _data = data;

            InterfaceSelection.Value = DefsFacade.I.Items.All[0].Id;
            _data.Inventory.OnChanged += OnChangedInventory;
            _trash.Retain(InterfaceSelection.Subscribe((x, y) => OnChanged?.Invoke()));
            foreach (var quickInventory in QuickInventorySelection)
            {
                quickInventory.Value = DefsFacade.I.Items.All[0].Id;
                _trash.Retain(quickInventory.Subscribe((x, y) => OnChanged?.Invoke()));
            }
        }

        private void OnChangedInventory(string id, int value)
        {
            OnChanged?.Invoke();
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public void Dispose()
        {
            _data.Inventory.OnChanged -= OnChangedInventory;       

            _trash.Dispose();
        }
    }
}