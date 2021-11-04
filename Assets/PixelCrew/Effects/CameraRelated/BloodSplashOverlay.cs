using PixelCrew.Model;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Effects.CameraRelated
{
    [RequireComponent(typeof(Animator))]
    public class BloodSplashOverlay : MonoBehaviour
    {
        [SerializeField] private Transform _overlay;

        private Animator _animator;
        private Vector3 _overScale;
        private GameSession _session;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private static readonly int Health = Animator.StringToHash("Health");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _overScale = _overlay.transform.localScale - Vector3.one;

            _session = FindObjectOfType<GameSession>();
            _trash.Retain(_session.Data.Hp.SubscribeAndInvoke(OnHpChanged));
        }

        private void OnHpChanged(int newValue, int _)
        {
            var maxHp = _session.StatsModel.GetValue(StatId.Hp);
            var hpNormalize = newValue / maxHp;
            _animator.SetFloat(Health, hpNormalize);

            var overlayModifier = Mathf.Max(hpNormalize - 0.3f, 0);
            _overlay.localScale = Vector3.one + _overScale * overlayModifier;
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}