using System.Collections;
using UnityEngine;

namespace PixelCrew.Animations
{
    public static class MoveAnimation
    {
        public static IEnumerator MoveToTarget(GameObject targetObject, Vector3 targetPosition, float moveDuration)
        {
            var moveTime = 0f;

            while (moveTime < moveDuration)
            {
                moveTime += Time.deltaTime;
                var progress = moveTime / moveDuration;
                targetObject.transform.position =
                    Vector3.Lerp(targetObject.transform.position, targetPosition, progress);

                yield return null;
            }
        }
    }
}