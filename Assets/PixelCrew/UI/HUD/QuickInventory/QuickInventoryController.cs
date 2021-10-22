using PixelCrew.Model;
using PixelCrew.Utils.Disposables;
using PixelCrew.Model.Data;
using PixelCrew.UI.Widgets;
using UnityEngine;

namespace PixelCrew.UI.HUD.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private QuickInventoryItemWidget _prefab;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private GameSession _session;
        private DataGroup<ItemData, QuickInventoryItemWidget> _dataGroup; 

        private void Start()
        {
            _dataGroup = new DataGroup<ItemData, QuickInventoryItemWidget>(_prefab, _container);
            _session = FindObjectOfType<GameSession>();

            if (_session == null) return;
            
            _trash.Retain(_session.QuickInventory.Subscribe(Rebuild));
            Rebuild();
        }
        
        private void Rebuild()
        {
            var inventory = _session.QuickInventory.Inventory;
            _dataGroup.SetData(inventory);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}