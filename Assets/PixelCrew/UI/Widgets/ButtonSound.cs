using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.PixelCrew.UI.Widgets
{
    public class ButtonSound : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
    {
        [SerializeField] private AudioClip _clickClip;
        [SerializeField] private AudioClip _selectClip;
        private AudioSource _source;
        private void Start()
        {
            if (_source == null)
                _source = GameObject.FindWithTag("SFXAudioSource").GetComponent<AudioSource>();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            _source.PlayOneShot(_clickClip);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _source.PlayOneShot(_selectClip);
        }
    }
}
