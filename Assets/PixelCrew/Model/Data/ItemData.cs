using System;
using PixelCrew.Model.Definitions.Repository.Items;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class ItemData
    {
        [InventoryId] public string Id;
        public int Value;

        public ItemData(string id)
        {
            Id = id;
        }
    }
}
