using System.Linq;
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

        public StatDef GetStat(StatId id) => _stats.FirstOrDefault(x => x.ID == id);

    }
}