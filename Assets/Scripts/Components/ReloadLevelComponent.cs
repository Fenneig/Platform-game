using PixelCrew.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void ReloadScene()
        {
            var session = FindObjectOfType<GameSession>();
            Destroy(session.gameObject);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}