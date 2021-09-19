﻿using Assets.PixelCrew.UI.Widgets;
using PixelCrew.Model;
using PixelCrew.Utils.Disposables;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.UI.HUD.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private InventoryItemWidget _prefab;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private GameSession _session;
        private List<InventoryItemWidget> _createdItems = new List<InventoryItemWidget>();

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            if (_session != null)
            {
                _trash.Retain(_session.QuickInventory.Subscribe(Rebuild));
                Rebuild();
            }
        }
        
        private void Rebuild()
        {
            var inventory = _session.QuickInventory.Inventory;
            
            for (var i = _createdItems.Count; i < inventory.Length; i++)
            {
                var item = Instantiate(_prefab, _container);
                _createdItems.Add(item);
            }

            for (var i = 0; i < inventory.Length; i++) 
            {
                _createdItems[i].SetData(inventory[i], i);
                _createdItems[i].gameObject.SetActive(true);
            }

            for (var i = inventory.Length; i < _createdItems.Count; i++)
            {
                _createdItems[i].gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}