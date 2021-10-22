using System;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.Model.Models
{
    public class StatsModel : IDisposable
    {
        private readonly PlayerData _data;
        public event Action OnChange;
        public event Action<StatId> OnUpgraded;
        
        public readonly ObservableProperty<StatId> InterfaceSelectionStat = new ObservableProperty<StatId>();

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        public StatsModel(PlayerData data)
        {
            _data = data;
            _trash.Retain(InterfaceSelectionStat.Subscribe((x, y) => OnChange?.Invoke()));
        }

        public IDisposable Subscribe(Action call)
        {
            OnChange += call;
            return new ActionDisposable(() => OnChange -= call);
        }

        public void LevelUp(StatId id)
        {
            var def = DefsFacade.I.Player.GetStat(id);
            var nextLevel = GetCurrentLevel(id) + 1;
            if (def.Levels.Length <= nextLevel) return;

            var price = def.Levels[nextLevel].Price;

            if (!_data.Inventory.IsEnough(price)) return;

            _data.Inventory.Remove(price.ItemId, price.Count);
            _data.Levels.LevelUp(id);

            OnUpgraded?.Invoke(id);
            OnChange?.Invoke();
        }

        public float GetValue(StatId id, int level = -1)
        {
            if (level == -1) level = GetCurrentLevel(id);
            return GetLevelDef(id, level).Value;
        }

        public StatLevelDef GetLevelDef(StatId id, int level = -1)
        {
            if (level == -1) level = GetCurrentLevel(id);
            var def = DefsFacade.I.Player.GetStat(id);
            return def.Levels.Length > level ? def.Levels[level] : default;
        }

        public int GetCurrentLevel(StatId id) => _data.Levels.GetLevel(id);

        public void Dispose()
        {
            _trash.Dispose();
        }
    }
}