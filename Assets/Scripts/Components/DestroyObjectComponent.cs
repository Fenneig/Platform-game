using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        private GameObject _objectToDestroy;

        private void Awake()
        {
            _objectToDestroy = this.gameObject;
        }

        public void DestroyObject() => Destroy(_objectToDestroy);
    }
}