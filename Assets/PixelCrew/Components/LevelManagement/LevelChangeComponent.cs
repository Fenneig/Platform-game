using PixelCrew.Model;
using PixelCrew.UI.LevelsLoader;
using UnityEngine;

namespace PixelCrew.Components.LevelManagement
{
    public class LevelChangeComponent : MonoBehaviour
    {
        [SerializeField] private string _levelName;

        public void ChangeLevel()
        {
            var session = GameSession.Instance;
            session.ClearCheckpoints();
            session.Save();
            
            var loader = FindObjectOfType<LevelLoader>();
            loader.LoadLevel(_levelName);
            
        }

    }
}