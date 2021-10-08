using System.Collections.Generic;
using System.Linq;
using PixelCrew.Components.LevelManagement;
using PixelCrew.Model.Data;
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
        [SerializeField] private PerksModel _perksModel;

        public PerksModel PerksModel { get; private set; }
        public PlayerData Data => _data;
        private PlayerData _save;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public QuickInventoryModel QuickInventory { get; private set; }

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
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        }

        private GameSession GetExistSession()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this) return gameSession;
            }

            return null;
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
            InitModels();
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
            if (!_checkpoints.Contains(id))
            {
                Save();
                _checkpoints.Add(id);
            }
        }

        public void StoreState(string id)
        {
            if (!_destroyedObjects.Contains(id)) _destroyedObjects.Add(id);
        }

        public bool IsItemDestroyed(string id) => _savedDestroyedObjects.Contains(id);
    }
}