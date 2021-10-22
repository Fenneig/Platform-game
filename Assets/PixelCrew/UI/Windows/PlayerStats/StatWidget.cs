using System.Globalization;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.PlayerStats
{
    public class StatWidget : MonoBehaviour, IItemRenderer<StatDef>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _name;
        [SerializeField] private Text _currentValue;
        [SerializeField] private Text _increaseValue;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private GameObject _selector;

        private GameSession _session;
        private StatDef _data;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            UpdateView();
        }

        public void SetData(StatDef data, int index)
        {
            _data = data;
            if (_session != null) UpdateView();
        }

        private void UpdateView()
        {
            var statsModel = _session.StatsModel;

            var currentLevel = statsModel.GetCurrentLevel(_data.ID);
            var nextLevel = currentLevel + 1;
            var currentValue = statsModel.GetValue(_data.ID);
            var increaseValue = statsModel.GetValue(_data.ID, nextLevel);
            var maxLevel = DefsFacade.I.Player.GetStat(_data.ID).Levels.Length - 1;

            _icon.sprite = _data.Icon;
            _name.text = LocalizationManager.I.Localize(_data.Name);
            _name.font = LocalizationManager.I.SetFont();
            _currentValue.text = currentValue.ToString(CultureInfo.InvariantCulture);
            _increaseValue.text = $"+ {increaseValue - currentValue}";
            _increaseValue.gameObject.SetActive(increaseValue > 0);
            _progressBar.value = currentLevel / (float) maxLevel;
            _selector.SetActive(statsModel.InterfaceSelectionStat.Value == _data.ID);
        }

        public void OnSelect()
        {
            _session.StatsModel.InterfaceSelectionStat.Value = _data.ID;
        }
    }
}