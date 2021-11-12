using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Components.Health
{
    public class EnemyHealthBarComponent : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;
        [SerializeField] private HealthComponent _healthComponent;

        private CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _trash.Retain(_healthComponent.Health.Subscribe(OnHealthChange));
            GetComponent<Canvas>().enabled = false;
        }

        private void Update()
        {
            UpdateScale();
        }

        private void UpdateScale()
        {
            if (gameObject.transform.root.localScale.x == gameObject.transform.lossyScale.x) return;

            var transform = gameObject.transform.localScale;
            transform = new Vector3(-transform.x, transform.y, transform.z);
            gameObject.transform.localScale = transform;
        }

        private void OnHealthChange(int newValue, int oldValue)
        {
            GetComponent<Canvas>().enabled = true;
            var maxHealth = _healthComponent.MaxHealth;
            var value = (float) newValue / maxHealth;
            if (value <= 0) Destroy(gameObject);
            _healthBar.SetProgress(value);
        }


        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}