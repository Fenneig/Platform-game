using UnityEngine;

namespace PixelCrew.Components
{
    public class ActivateComponent : MonoBehaviour
    {
        private ParticleSystem ps;

        private void Awake()
        {
            ps = GetComponent<ParticleSystem>();
        }
        public void switchActive()
        {
            GetComponent<Collider2D>().enabled = !gameObject.GetComponent<Collider2D>().enabled;
            if (ps.isPlaying) ps.Stop();
            else ps.Play();
        }
    }
}