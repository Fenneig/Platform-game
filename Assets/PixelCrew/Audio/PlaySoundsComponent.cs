using PixelCrew.Utils;
using System;
using UnityEngine;

namespace PixelCrew.Audio
{
    public class PlaySoundsComponent : MonoBehaviour
    {
        [SerializeField] private AudioData[] _data;
        private AudioSource _source;

        private void Start()
        {
            if (_source == null)
                _source = AudioUtils.FindSfxSource();
        }

        public void Play(string id) 
        {
            foreach (var audioData in _data)
            {
                if (audioData.Id != id) continue;

                _source.PlayOneShot(audioData.Clip);
                break;
            }
        }


        [Serializable]
        public class AudioData 
        {
            [SerializeField] private string _id;
            [SerializeField] private AudioClip _clip;

            public string Id => _id;
            public AudioClip Clip => _clip;

        }
    }
}