using System;
using PixelCrew.Model.Definitions.Repository.Items;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class InventoryItemData
    {
        [InventoryId] public string Id;
        public int Value;

        public InventoryItemData(string id)
        {
            Id = id;
        }
    }
}
