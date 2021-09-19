using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            if (_session != null)
            {
                _trash.Retain(_session.Data.Hp.Subscibe(OnHealthChange));
                OnHealthChange(_session.Data.Hp.Value, 0);
            }
        }

        private void OnHealthChange(int newValue, int oldValue)
        {
            var maxHealth = DefsFacade.I.Player.MaxHealth;
            var value = (float)newValue / maxHealth;
            _healthBar.SetProgress(value);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}