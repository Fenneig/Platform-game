using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Patroling
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();
    }
}
