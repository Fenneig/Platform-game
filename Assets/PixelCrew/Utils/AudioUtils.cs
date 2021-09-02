using UnityEngine;

namespace PixelCrew.Utils
{
    public static class AudioUtils
    {
        public static AudioSource FindSfxSource() 
        {
            return GameObject.FindWithTag("SFXAudioSource").GetComponent<AudioSource>();   
        }
    }
}