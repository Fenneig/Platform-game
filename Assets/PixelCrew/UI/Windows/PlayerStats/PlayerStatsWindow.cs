using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.PlayerStats
{
    public class PlayerStatsWindow : InGameAnimatedWindow
    {
        [SerializeField] private Transform _container;
        [SerializeField] private StatWidget _prefab;

        [SerializeField] private Button _upgradeButton;
        [SerializeField] private ItemWidget _price;

        private DataGroup<StatDef, StatWidget> _dataGroup;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        protected override void Start()
        {
            base.Start();
            
            _dataGroup = new DataGroup<StatDef, StatWidget>(_prefab, _container);

            Session = GameSession.Instance;
            Session.StatsModel.InterfaceSelectionStat.Value = DefsFacade.I.Player.Stats[0].ID;

            _trash.Retain(Session.StatsModel.Subscribe(OnStatsChange));
            _trash.Retain(_upgradeButton.onClick.Subscribe(OnUpgrade));
            OnStatsChange();
        }
        
        private void OnUpgrade()
        {
            var selected = Session.StatsModel.InterfaceSelectionStat.Value;

            Session.StatsModel.LevelUp(selected);
        }

        private void OnStatsChange()
        {
            var stats = DefsFacade.I.Player.Stats;
            _dataGroup.SetData(stats);

            var selected = Session.StatsModel.InterfaceSelectionStat.Value;
            var nextLevel = Session.StatsModel.GetCurrentLevel(selected) + 1;
            var def = Session.StatsModel.GetLevelDef(selected, nextLevel);
            _price.SetData(def.Price);
            
            _price.gameObject.SetActive(def.Price.Count != 0);
            _upgradeButton.gameObject.SetActive(def.Price.Count != 0);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}