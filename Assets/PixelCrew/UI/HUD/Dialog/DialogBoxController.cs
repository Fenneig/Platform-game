using PixelCrew.Model.Data.Dialogue;
using PixelCrew.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
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
        [SerializeField] private GameObject _background;

        [Space] [SerializeField] private float _textSpeed = 0.1f;
        [Header("Sounds")] [SerializeField] private AudioClip _typing;
        [SerializeField] private AudioClip _open;
        [SerializeField] private AudioClip _close;

        private static readonly int IsOpen = Animator.StringToHash("IsOpen");
        private DialogueData _data;
        private int _currentSentence;
        private AudioSource _sfxSource;
        private Coroutine _typingRoutine;
        private RectTransform _containerRectTransform;
        private RectTransform _portraitRectTransform;
        private float _defaultTimeScale;
        private PlayerInput _input;

        private void Start()
        {
            _sfxSource = AudioUtils.FindSfxSource();
            _containerRectTransform = _container.GetComponent<RectTransform>();
            _portraitRectTransform = _portraitContainer.GetComponent<RectTransform>();
            _defaultTimeScale = Time.timeScale;
            _input = FindObjectOfType<PlayerInput>();
        }

        public void ShowDialogue(DialogueData data)
        {
            _data = data;
            _currentSentence = 0;
            Time.timeScale = 0f;
            _input.enabled = false;
            SetupDialogSettings();
            _text.text = string.Empty;

            _container.SetActive(true);
            _background.SetActive(true);
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
            var anchoredPosition = _containerRectTransform.anchoredPosition;
            anchoredPosition = new Vector2(-anchoredPosition.x, anchoredPosition.y);
            _containerRectTransform.anchoredPosition = anchoredPosition;

            var position = _portraitRectTransform.anchoredPosition;
            position = new Vector2(-position.x, position.y);
            _portraitRectTransform.anchoredPosition = position;
        }

        private IEnumerator TypeDialogText()
        {
            _text.text = string.Empty;
            _text.font = _data.Font;
            var sentence = _data.Sentences[_currentSentence].Line;
            foreach (var letter in sentence)
            {
                _text.text += letter;
                _sfxSource.PlayOneShot(_typing);
                yield return new WaitForSecondsRealtime(_textSpeed);
            }

            _typingRoutine = null;
        }

        private void OnCloseAnimationComplete()
        {
            _background.SetActive(false);
            Time.timeScale = _defaultTimeScale;
            _input.enabled = true;
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