using UnityEngine;

namespace PixelCrew.Components.GOBased
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        public void DestroyObject() => Destroy(gameObject);
    }
}