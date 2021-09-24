using UnityEngine;

namespace PixelCrew.UI.Windows
{
    public class AnimatedWindow : MonoBehaviour
    {
        private Animator _animator;

        private static readonly int ShowKey = Animator.StringToHash("Show");
        private static readonly int HideKey = Animator.StringToHash("Hide");

        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();

            _animator.SetTrigger(ShowKey);
        }

        public void Close() 
        {
            _animator.SetTrigger(HideKey);
        }

        public virtual void OnCloseAnimationComplete() 
        {
            Destroy(gameObject);
        }
    }
}