using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Audio
{
    public class PlaySfxSound : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        private AudioSource _source;

        private void Start()
        {
            _source = AudioUtils.FindSfxSource();
        }

        public void Play() 
        {
            _source.PlayOneShot(_clip);
        }

    }
}