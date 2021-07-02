using PixelCrew.Creatures.Hero;
using PixelCrew.Model.Data;
using UnityEngine;

namespace PixelCrew.Components.Collectables
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [SerializeField] private InventoryItemData _item;

        public InventoryItemData Item => _item;

        public void Add(GameObject go) 
        {
            var hero = go.GetComponent<Hero>();
            if (hero != null) hero.AddInInventory(_item.Id, _item.Value, _item.IsStackable);
        }
    }
}