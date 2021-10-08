using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Patrol
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();
    }
}
