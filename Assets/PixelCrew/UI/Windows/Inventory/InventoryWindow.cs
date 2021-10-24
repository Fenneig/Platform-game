using System.Linq;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Model.Definitions.Repository.Items;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.Inventory
{
    public class InventoryWindow : InGameAnimatedWindow
    {
        [SerializeField] private Button _selectButton;
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private Text _infoText;

        private PredefinedDataGroup<ItemData, InventoryWidget> _dataGroup;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private ItemData[] _inventory;

        protected override void Start()
        {
            base.Start();
            _inventory = Session.Data.Inventory.GetAll();
            if (_inventory.Length > 0) Session.InventoryModel.InterfaceSelection.Value = _inventory[0].Id;

            _dataGroup = new PredefinedDataGroup<ItemData, InventoryWidget>(_itemsContainer);

            _trash.Retain(Session.InventoryModel.Subscribe(OnInventoryChanged));
            _trash.Retain(_selectButton.onClick.Subscribe(OnSelect));
            OnInventoryChanged();
        }

        private void OnSelect()
        {
            var selected = Session.InventoryModel.InterfaceSelection;
            var itemId = new StringProperty {Value = selected.Value};
            var itemDef = DefsFacade.I.Items.Get(itemId.Value);
            if (!itemDef.HasTag(ItemTag.Usable)) return;
            
            var quickInventory = Session.InventoryModel.QuickInventorySelection;
            if (quickInventory.Any(item => item.Value == itemId.Value)) return;
        
            int amount;
            if (quickInventory.Count == 3)
            {
                 amount = Session.Data.QuickInventory.Count(quickInventory[0].Value);
                Session.Data.QuickInventory.Remove(quickInventory[0].Value, amount);
                quickInventory.Remove(quickInventory[0]);
            }
            
            amount = Session.Data.Inventory.Count(itemId.Value);
            Session.Data.QuickInventory.Add(itemId.Value, amount);
            quickInventory.Add(itemId);
        
            OnInventoryChanged();
        }

        private void OnInventoryChanged()
        {
            _dataGroup.SetData(_inventory);
            var selected = Session.InventoryModel.InterfaceSelection.Value;
            var def = DefsFacade.I.Items.Get(selected);
            _infoText.text = LocalizationManager.I.Localize(def.Info);
            _infoText.font = LocalizationManager.I.SetFont();

            var isInventoryEmpty = _inventory.Length == 0;
            if (isInventoryEmpty) _infoText.text = string.Empty;
            _selectButton.gameObject.SetActive(!isInventoryEmpty);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}