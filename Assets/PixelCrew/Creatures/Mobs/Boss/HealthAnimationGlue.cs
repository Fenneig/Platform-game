using PixelCrew.Components.Health;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss
{
    public class HealthAnimationGlue : MonoBehaviour
    {
        [SerializeField] private HealthComponent _hp;
        [SerializeField] private Animator _animator;
        private static readonly int Health = Animator.StringToHash("Health");

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Awake()
        {
            _animator.SetInteger(Health, _hp.Health.Value);
            _trash.Retain(_hp.Health.Subscribe(OnHealthChanged));
        }

        private void OnHealthChanged(int value, int _)
        {
            _animator.SetInteger(Health, value);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}