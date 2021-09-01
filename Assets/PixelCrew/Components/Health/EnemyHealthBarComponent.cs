using PixelCrew.UI.Widgets;
using UnityEngine;

namespace PixelCrew.Components.Health
{
    public class EnemyHealthBarComponent : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;
        [SerializeField] private HealthComponent _healthComponent;

        private void Start()
        {
            _healthComponent.Health.OnChanged += OnHealthChange;
            GetComponent<Canvas>().enabled = false;
        }

        private void Update()
        {
            UpdateScale();
        }

        private void UpdateScale() 
        {
            if (gameObject.transform.root.localScale.x != gameObject.transform.lossyScale.x)
                gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }

        private void OnHealthChange(int newValue, int oldValue)
        {
            GetComponent<Canvas>().enabled = true;
            var maxHealth = _healthComponent.MaxHealth;
            var value = (float)newValue / maxHealth;
            if (value <= 0) Destroy(gameObject);
            _healthBar.SetProgress(value);
        }


        private void OnDestroy()
        {
            _healthComponent.Health.OnChanged -= OnHealthChange;
        }
    }
}
