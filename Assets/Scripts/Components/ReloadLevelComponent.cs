using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}