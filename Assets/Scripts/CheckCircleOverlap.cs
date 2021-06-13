using PixelCrew.Utils;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PixelCrew
{

    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] float _range = 0.2f;
        [SerializeField] LayerMask TrashLayer;

        private readonly Collider2D[] _objectsResult = new Collider2D[20];
        public GameObject[] getObjectsInRange()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, _range, _objectsResult);

            var overlap = new List<GameObject>();
            for (int i = 0; i < size; i++)
            {
                if (_objectsResult[i].gameObject.IsInLayer(TrashLayer)) continue;

                overlap.Add(_objectsResult[i].gameObject);
            }

            return overlap.ToArray();
        }

        private void OnDrawGizmos()
        {
            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _range);
        }
    }
}
