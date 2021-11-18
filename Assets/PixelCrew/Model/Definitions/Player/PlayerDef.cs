using UnityEngine;

namespace PixelCrew.Model.Definitions.Player
{
    [CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int _maxInventorySize;
        [SerializeField] private StatDef[] _stats;

        public int MaxInventorySize => _maxInventorySize;
        public StatDef[] Stats => _stats;

        public StatDef GetStat(StatId id)
        {
            foreach (var stat in _stats)
            {
                if (stat.ID == id) return stat;
            }

            return default;
        }
    }
}