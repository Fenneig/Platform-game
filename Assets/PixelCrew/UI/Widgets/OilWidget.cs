using PixelCrew.Model;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{
    public class OilWidget : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _oilBar;
        [SerializeField] private int _maxOilValue;
        private const string OilId = "CandleOil";
        private GameSession _session;

        private readonly CompositeDisposable _trash = new CompositeDisposable();


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();

            _trash.Retain(_session.InventoryModel.Subscribe(OilAmountChanged));
            _trash.Retain(_session.StatsModel.Subscribe(MaxOilValueChanged));
            MaxOilValueChanged();
        }

        private void MaxOilValueChanged()
        {
            _maxOilValue = (int) _session.StatsModel.GetValue(StatId.OilAmount);
        }

        private void OilAmountChanged()
        {
            var oil = _session.Data.Inventory.GetItem(OilId);
            if (oil == null)
            {
                _oilBar.gameObject.SetActive(false);
                return;
            }

            if (oil.Value > _maxOilValue) oil.Value = _maxOilValue;
            _oilBar.gameObject.SetActive(true);
            _oilBar.SetProgress(oil.Value / (float) _maxOilValue);
        }
        

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}