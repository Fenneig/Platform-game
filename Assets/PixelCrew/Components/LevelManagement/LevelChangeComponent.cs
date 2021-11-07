using PixelCrew.Model;
using PixelCrew.UI.LevelsLoader;
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
            session.ClearCheckpoints();
            session.Save();
            
            var loader = FindObjectOfType<LevelLoader>();
            loader.LoadLevel(_levelName);
            
        }
    }
}