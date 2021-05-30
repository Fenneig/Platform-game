using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class ActivateComponent : MonoBehaviour
    {
        public void switchActive()
        {
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
            GetComponent<Collider2D>().enabled = !GetComponent<Collider2D>().enabled;
        }
    }
}