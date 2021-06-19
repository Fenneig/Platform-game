using UnityEngine;

namespace PixelCrew.Components
{
    public class ActivateComponent : MonoBehaviour
    {
        private ParticleSystem _ps;
        private Collider2D _collider;

        private void Awake()
        {
            _ps = GetComponent<ParticleSystem>();
            _collider = GetComponent<Collider2D>();
        }
        public void SwitchActive()
        {
            _collider.enabled = !_collider.enabled;
            if (_ps.isPlaying) _ps.Stop();
            else _ps.Play();
        }
    }
}