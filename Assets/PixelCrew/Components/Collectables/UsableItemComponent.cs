using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Collectables
{
    public class UsableItemComponent : MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _itemId;
        [SerializeField] private UnityEvent _action;
        [SerializeField] private bool _removeAfterUse;

        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }

        public void Use() 
        {
            if (_session.Data.Inventory.Count(_itemId) > 0) 
            {
                _action?.Invoke();
                if (_removeAfterUse) _session.Data.Inventory.Remove(_itemId, 1);
            }
        }
    }
}