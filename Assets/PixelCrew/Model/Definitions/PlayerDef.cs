using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int _maxInventorySize;
        [SerializeField] private int _maxHealth;

        public int MaxInventorySize => _maxInventorySize;
        public int MaxHealth => _maxHealth;
    }
}