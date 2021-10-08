using PixelCrew.Animations;
using UnityEngine;

namespace PixelCrew.Components.GOBased
{
    public class MoveObjectComponent : MonoBehaviour
    {
        public void MoveToObject(GameObject instance, Vector3 newPosition, float moveTime)
        {
            StartCoroutine(MoveAnimation.MoveToTarget(instance, newPosition, moveTime));
        }
    }
}