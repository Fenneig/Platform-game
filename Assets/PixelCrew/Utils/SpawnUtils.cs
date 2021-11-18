using UnityEngine;

namespace PixelCrew.Utils
{
    public class SpawnUtils : MonoBehaviour
    {
        private const string ContainerName = "###SPAWNED###";

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, string containerName = ContainerName)
        {
            var container = GameObject.Find(containerName);
            if (container == null)
                container = new GameObject(containerName);

            return Instantiate(prefab, position, rotation, container.transform);
        }
    }
}