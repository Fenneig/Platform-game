using PixelCrew.Creatures;
using UnityEngine;

namespace PixelCrew.Utils
{
    public class Debugger : MonoBehaviour
    {
        private Creature _creature;

        private void Start()
        {
            _creature = GetComponent<Creature>();
        }

        private void Update()
        {
            Debug.Log(_creature.Speed);
        }
    }
}