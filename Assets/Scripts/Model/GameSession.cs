using UnityEngine;

namespace PixelCrew.Model
{
    class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data
        {
            get => _data;
            set => _data = value;
        }

        private void Awake()
        {
            if (IsSessionExist())
            {
                Destroy(gameObject);
            }
            else
            {
                var savedData = FindObjectOfType<SavedState>().Load();
                if (savedData != null) Data = new PlayerData(savedData);

                DontDestroyOnLoad(this);
            }
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

    }
}
