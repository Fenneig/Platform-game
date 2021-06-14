using PixelCrew.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components
{
    public class LevelChangeComponent : MonoBehaviour
    {
        [SerializeField] string _levelName;

        public void ChangeLevel()
        {
            FindObjectOfType<SavedState>().Save(FindObjectOfType<GameSession>().Data);
            SceneManager.LoadScene(_levelName);
        }
    }
}