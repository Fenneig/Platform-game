using PixelCrew.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components.LevelManagement
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void ReloadScene()
        {
            var session = GameSession.Instance;
            session.Load();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}