using PixelCrew.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components.LevelManagement
{
    public class LevelChangeComponent : MonoBehaviour
    {
        [SerializeField] private string _levelName;

        public void ChangeLevel()
        {
            var session = FindObjectOfType<GameSession>();
            session?.Save();

            SceneManager.LoadScene(_levelName);
        }
    }
}