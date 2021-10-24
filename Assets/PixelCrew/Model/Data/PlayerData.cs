using PixelCrew.Model.Data.Properties;
using System;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;
        [SerializeField] private InventoryData _quickInventory;

        public IntProperty Hp = new IntProperty();
        public PerkData Perks = new PerkData();
        public LevelData Levels = new LevelData();

        public InventoryData Inventory => _inventory;
        public InventoryData QuickInventory => _quickInventory;

        public PlayerData Clone() 
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }
}
