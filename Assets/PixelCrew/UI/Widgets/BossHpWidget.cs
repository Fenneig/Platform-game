using PixelCrew.Components.Health;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{
    public class BossHpWidget : MonoBehaviour
    {
        [SerializeField] private HealthComponent _hp;
        [SerializeField] private ProgressBarWidget _hpBar;
        [SerializeField] private CanvasGroup _canvas;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private float _health;

        private void Start()
        {
            _health = _hp.Health.Value;
            _trash.Retain(_hp.Health.Subscribe(OnHealthChanged));
            _trash.Retain(_hp._onDie.Subscribe(HideUI));
        }


        private void OnHealthChanged(int value, int _)
        {
            _hpBar.SetProgress(value / _health);
        }

        [ContextMenu("showUi")]
        public void ShowUI()
        {
            this.LerpAnimation(0, 1, 1, SetAlpha);
        }

        private void SetAlpha(float alpha)
        {
            _canvas.alpha = alpha;
        }

        [ContextMenu("hideUi")]
        public void HideUI()
        {
            this.LerpAnimation(1, 0, 1, SetAlpha);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}