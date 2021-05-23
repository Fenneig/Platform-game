using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class LowGravityZoneComponent : MonoBehaviour
    {
        [SerializeField] Hero _hero;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(_hero.tag))
                _hero.GetComponent<Rigidbody2D>().gravityScale *= 1.75f;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(_hero.tag))
                _hero.GetComponent<Rigidbody2D>().gravityScale /= 1.75f;
        }
    }
}
