using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Collectables
{
    [RequireComponent(typeof(InventoryAddComponent))]
    public class IsInventoryFullCheckComponent : MonoBehaviour
    {
        [SerializeField] private EnterEvent _action;
        private InventoryItemData _itemData;
        private GameSession _session;

        private void Awake()
        {
            _session = FindObjectOfType<GameSession>();
            _itemData = GetComponent<InventoryAddComponent>().Item;
        }

        public void Check(GameObject go) 
        {
            if (CanInvoke()) _action?.Invoke(go);
            else Debug.Log("Inventory is full!");
        }

        private bool CanInvoke()
        {
            var inventory = _session.Data.Inventory;
            return inventory.Size < DefsFacade.I.Player.MaxInventorySize || inventory.IsContainStackableItem(_itemData);
        }


        [Serializable]
        public class EnterEvent : UnityEvent<GameObject>
        { }
    }
}