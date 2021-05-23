using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class PalmCrownComponent : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        private void Update()
        {
            if (_hero.GetDirection().y < 0)
            {
                Physics2D.IgnoreLayerCollision(10, 11, true);
                Invoke("IgnoreOff", 0.2f);
            }
        }
        private void IgnoreOff()
        {
            Physics2D.IgnoreLayerCollision(10, 11, false);
        }
    }
}

