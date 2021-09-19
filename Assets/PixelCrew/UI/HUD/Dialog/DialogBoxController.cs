using PixelCrew.Model.Data.Dialog;
using PixelCrew.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.HUD.Dialog
{
    public class DialogBoxController : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private GameObject _container;
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _portraitContainer;
        [SerializeField] private Image _portraitSprite;

        [Space]
        [SerializeField] private float _textSpeed = 0.1f;
        [Header("Sounds")]
        [SerializeField] private AudioClip _typing;
        [SerializeField] private AudioClip _open;
        [SerializeField] private AudioClip _close;

        private static readonly int IsOpen = Animator.StringToHash("IsOpen");
        private DialogData _data;
        private int _currentSentence;
        private AudioSource _sfxSource;
        private Coroutine _typingRoutine;
        private RectTransform _containerRectTransform;
        private RectTransform _portraitRectTransform;

        private void Start()
        {
            _sfxSource = AudioUtils.FindSfxSource();
            _containerRectTransform = _container.GetComponent<RectTransform>();
            _portraitRectTransform = _portraitContainer.GetComponent<RectTransform>();
        }

        public void ShowDialog(DialogData data) 
        {
            _data = data;
            _currentSentence = 0;
            SetupDialogSettings();
            _text.text = string.Empty;

            _container.SetActive(true);
            _sfxSource.PlayOneShot(_open);
            _animator.SetBool(IsOpen, true);
        }

        private void OnStartDialogAnimation() 
        {
            SetupDialogSettings();
            _typingRoutine = StartCoroutine(TypeDialogText());
        }

        private void SetupDialogSettings()
        {
            if (_data.Sentences[_currentSentence].Portrait == null)
            {
                _portraitContainer.SetActive(false);
            }
            else
            {
                _portraitContainer.SetActive(true);
                _portraitSprite.sprite = _data.Sentences[_currentSentence].Portrait;
            }
            if (_data.Sentences[_currentSentence].IsHero && _containerRectTransform.anchoredPosition.x > 0)
            {
                SwitchDialogSide();
            }
            else if (!_data.Sentences[_currentSentence].IsHero && _containerRectTransform.anchoredPosition.x < 0)
            {
                SwitchDialogSide();
            }
        }

        private void SwitchDialogSide()
        {
            _containerRectTransform.anchoredPosition = new Vector2(-_containerRectTransform.anchoredPosition.x, _containerRectTransform.anchoredPosition.y);
            _portraitRectTransform.anchoredPosition = new Vector2(-_portraitRectTransform.anchoredPosition.x, _portraitRectTransform.anchoredPosition.y);
        }

        private IEnumerator TypeDialogText()
        {
            _text.text = string.Empty;
            var sentence = _data.Sentences[_currentSentence].Line;
            foreach(var letter in sentence)
            {
                _text.text += letter;
                _sfxSource.PlayOneShot(_typing);
                yield return new WaitForSeconds(_textSpeed);
            }

            _typingRoutine = null;
        }

        public void OnCloseAnimationComplete() 
        {

        }

        public void OnSkip() 
        {
            if (_typingRoutine == null) return;

            StopTypeAnimation();
            _text.text = _data.Sentences[_currentSentence].Line;
        }

        private void StopTypeAnimation()
        {
            if (_typingRoutine != null) StopCoroutine(_typingRoutine);
            _typingRoutine = null;
        }

        public void OnContinue() 
        {
            StopTypeAnimation();
            _currentSentence++;

            var isDialogComplete = _currentSentence >= _data.Sentences.Length;
            if (isDialogComplete)
            {
                HideDialogBox();
            }
            else 
            {
                OnStartDialogAnimation();
            }
        }

        private void HideDialogBox()
        {
            _animator.SetBool(IsOpen, false);
            _sfxSource.PlayOneShot(_close);
            OnCloseAnimationComplete();
        }
    }
}