using System.Linq;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
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
            _trash.Retain(_selectButton.onClick.Subscribe(OnAddInQuickInventory));
            OnInventoryChanged();
        }

        private void OnAddInQuickInventory()
        {
            var selected = Session.InventoryModel.InterfaceSelection;
            var quickInventory = Session.InventoryModel.QuickInventorySelection;
            var itemId = new StringProperty {Value = selected.Value};

            if (quickInventory.Any(item => item.Value == itemId.Value)) return;

            if (quickInventory.Count == 3) quickInventory.Dequeue();
            quickInventory.Enqueue(itemId);
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