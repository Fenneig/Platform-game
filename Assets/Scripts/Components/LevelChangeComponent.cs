using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components
{
    public class LevelChangeComponent : MonoBehaviour
    {
        [SerializeField] string _levelName;

        public void ChangeLevel() => SceneManager.LoadScene(_levelName);
    }
}