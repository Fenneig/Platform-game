using System.Linq;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.Inventory
{
    public class InventoryWidget : MonoBehaviour, IItemRenderer<ItemData>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _value;
        [SerializeField] private GameObject _selector;
        [SerializeField] private GameObject _quickInventorySelected;

        private GameSession _session;
        private ItemData _data;

        private void Start()
        {
            _session = GameSession.Instance;
            UpdateView();
        }

        public void SetData(ItemData dataInfo, int index)
        {
            _data = dataInfo;
            if (_session != null) UpdateView();
        }

        private void UpdateView()
        {
            _icon.sprite = DefsFacade.I.Items.Get(_data.Id).Icon;
            var itemValue = _session.Data.Inventory.GetItem(_data.Id).Value;
            _value.text = itemValue > 1 ? itemValue.ToString() : string.Empty;
            _selector.SetActive(_session.InventoryModel.InterfaceSelection.Value == _data.Id);
            
            _quickInventorySelected.SetActive(_session.InventoryModel.QuickInventorySelection.Any(item => item.Value == _data.Id));
        }

        public void OnSelect()
        {
            _session.InventoryModel.InterfaceSelection.Value = _data.Id;
        }
    }
}