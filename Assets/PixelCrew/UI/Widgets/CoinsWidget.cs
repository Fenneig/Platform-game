using PixelCrew.Model;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class CoinsWidget : MonoBehaviour
    {
        [SerializeField] private Text _value;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _trash.Retain(_session.InventoryModel.Subscribe(OnChange));
            OnChange();
        }

        private void OnChange()
        {
            _value.text = $"X{_session.Data.Inventory.Count("Coin")}";
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}