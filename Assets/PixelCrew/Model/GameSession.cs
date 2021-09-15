﻿using PixelCrew.Model.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;
        private PlayerData _save;
        public QuickInventoryModel QuickInventory { get; private set; }

        private void Awake()
        {
            LoadHud();

            if (IsSessionExist())
            {
                Destroy(gameObject);
            }
            else
            {
                Save();
                DontDestroyOnLoad(this);
                InitModels();
            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(_data);
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        }

        private bool IsSessionExist()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                    return true;
            }

            return false;
        }


        public void Save()
        {
            _save = _data.Clone();
        }

        public void Load()
        {
            _data = _save.Clone();
        }
    }
}
