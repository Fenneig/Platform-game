using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.PlayerStats
{
    public class PlayerStatsWindow : AnimatedWindow
    {
        [SerializeField] private Transform _container;
        [SerializeField] private StatWidget _prefab;

        [SerializeField] private Button _upgradeButton;
        [SerializeField] private ItemWidget _price;

        private DataGroup<StatDef, StatWidget> _dataGroup;

        private GameSession _session;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        private float _defaultTimeScale;
        private PlayerInput _input;

        
        protected override void Start()
        {
            base.Start();
            
            _input = FindObjectOfType<PlayerInput>();
            _defaultTimeScale = Time.timeScale;
            PauseGame();
            
            _dataGroup = new DataGroup<StatDef, StatWidget>(_prefab, _container);

            _session = FindObjectOfType<GameSession>();
            _session.StatsModel.InterfaceSelectionStat.Value = DefsFacade.I.Player.Stats[0].ID;

            _trash.Retain(_session.StatsModel.Subscribe(OnStatsChange));
            _trash.Retain(_upgradeButton.onClick.Subscribe(OnUpgrade));
            OnStatsChange();
        }
  
        private void PauseGame()
        {
            Time.timeScale = 0f;
            _input.enabled = false;
        }

        private void ResumeGame()
        {
            Time.timeScale = _defaultTimeScale;
            _input.enabled = true;
        }
        
        private void OnUpgrade()
        {
            var selected = _session.StatsModel.InterfaceSelectionStat.Value;

            _session.StatsModel.LevelUp(selected);
        }

        private void OnStatsChange()
        {
            var stats = DefsFacade.I.Player.Stats;
            _dataGroup.SetData(stats);

            var selected = _session.StatsModel.InterfaceSelectionStat.Value;
            var nextLevel = _session.StatsModel.GetCurrentLevel(selected) + 1;
            var def = _session.StatsModel.GetLevelDef(selected, nextLevel);
            _price.SetData(def.Price);
            
            _price.gameObject.SetActive(def.Price.Count != 0);
            _upgradeButton.gameObject.SetActive(def.Price.Count != 0);
        }

        private void OnDestroy()
        {
            ResumeGame();
            _trash.Dispose();
        }
    }
}