using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Collectables
{
    public class ItemToUseComponent : MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _itemId;
        [SerializeField] private UnityEvent _action;
        [SerializeField] private bool _removeAfterUse;

        public void Use() 
        {
            var session = FindObjectOfType<GameSession>();

            if (session.Data.Inventory.Count(_itemId) > 0) 
            {
                _action?.Invoke();
                if (_removeAfterUse) session.Data.Inventory.Remove(_itemId, 1);
            }
        }


    }
}