using UnityEngine;

namespace PixelCrew.Utils
{
    public class SpawnUtils : MonoBehaviour
    {
        private const string ContainerName = "###SPAWNED###";

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion quaternion) 
        {
            var container = GameObject.Find(ContainerName);
            if (container == null)
                container = new GameObject(ContainerName);

            return Instantiate(prefab, position, quaternion, container.transform);
        }
    }
}