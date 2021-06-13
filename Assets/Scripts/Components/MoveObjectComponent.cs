using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components
{
    public class MoveObjectComponent : MonoBehaviour
    {
        public void MoveToObject(GameObject instance, Vector3 newPosition, float _moveTime) 
        {
            StartCoroutine(MoveAnimation.MoveToTarget(instance, newPosition, _moveTime));
        }
    }
}