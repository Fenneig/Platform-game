using System.Collections;
using UnityEngine;

namespace PixelCrew.Animations
{
    public static class MoveAnimation
    {
        public static IEnumerator MoveToTarget(GameObject targetObject, Vector3 targetPosition, float moveDuratation)
        {
            var moveTime = 0f;

            while (moveTime < moveDuratation)
            {
                moveTime += Time.deltaTime;
                var progress = moveTime / moveDuratation;
                targetObject.transform.position = Vector3.Lerp(targetObject.transform.position, targetPosition, progress);

                yield return null;
            }
        }
    }
}
