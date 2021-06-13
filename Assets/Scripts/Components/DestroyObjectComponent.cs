using UnityEngine;

namespace PixelCrew.Components
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        public void DestroyObject() => Destroy(gameObject);
    }
}