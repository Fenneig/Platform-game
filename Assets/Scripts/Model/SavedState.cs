using UnityEngine;

namespace PixelCrew.Model
{
    class SavedState : MonoBehaviour
    {
        private PlayerData _savedData;

        private void Awake()
        {
            if (IsStatesExist())
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
            }
        }

        private bool IsStatesExist()
        {
            var states = FindObjectsOfType<SavedState>();

            foreach (var state in states)
            {
                if (state != this)
                    return true;
            }

            return false;
        }
        public void Save(PlayerData data)  => _savedData = new PlayerData(data);

        public PlayerData Load() => _savedData;
    }
}
