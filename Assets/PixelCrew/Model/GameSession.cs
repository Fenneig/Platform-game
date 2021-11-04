﻿using System.Collections.Generic;
using System.Linq;
using PixelCrew.Components.LevelManagement;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Model.Models;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        [SerializeField] private string _defaultCheckpoint;
        public PerksModel PerksModel { get; private set; }
        public QuickInventoryModel QuickInventory { get; private set; }
        public StatsModel StatsModel { get; private set; }
        public InventoryModel InventoryModel { get; private set; }

        public PlayerData Data => _data;
        private PlayerData _save;
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private readonly List<string> _checkpoints = new List<string>();
        private readonly List<string> _destroyedObjects = new List<string>();
        private readonly List<string> _savedDestroyedObjects = new List<string>();

        private void Awake()
        {
            var existSession = GetExistSession();
            
            if (existSession != null)
            {
                existSession.StartSession(_defaultCheckpoint);
                Destroy(gameObject);
            }
            else
            {
                Save();
                DontDestroyOnLoad(this);
                StartSession(_defaultCheckpoint);
                InitModels();
            }
        }

        private void StartSession(string checkpoint)
        {
            SetChecked(checkpoint);
            LoadHud();
            SpawnHero();
        }

        private void SpawnHero()
        {
            var checkpoints = FindObjectsOfType<CheckPointComponent>();
            var lastCheckpoint = _checkpoints.Last();
            foreach (var checkpoint in checkpoints)
            {
                if (checkpoint.Id != lastCheckpoint) continue;

                checkpoint.SpawnHero();
                break;
            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(_data);
            _trash.Retain(QuickInventory);

            PerksModel = new PerksModel(_data);
            _trash.Retain(PerksModel);

            StatsModel = new StatsModel(_data);
            _trash.Retain(StatsModel);

            InventoryModel = new InventoryModel(_data);
            _trash.Retain(InventoryModel);

            _data.Hp.Value = (int) StatsModel.GetValue(StatId.Hp);
        }

        private static void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        }

        private GameSession GetExistSession()
        {
            var sessions = FindObjectsOfType<GameSession>();
            return sessions.FirstOrDefault(gameSession => gameSession != this);
        }

        public void ClearCheckpoints()
        {
            _checkpoints.Clear();
        }

        public void Save()
        {
            _save = _data.Clone();
            _savedDestroyedObjects.AddRange(_destroyedObjects);
            _destroyedObjects.Clear();
        }

        public void Load()
        {
            _data = _save.Clone();
            InitModels();
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }

        public bool IsChecked(string id)
        {
            return _checkpoints.Contains(id);
        }

        public void SetChecked(string id)
        {
            if (_checkpoints.Contains(id)) return;

            Save();
            _checkpoints.Add(id);
        }

        public void StoreState(string id)
        {
            if (!_destroyedObjects.Contains(id)) _destroyedObjects.Add(id);
        }

        public bool IsItemDestroyed(string id) => _savedDestroyedObjects.Contains(id);
    }
}