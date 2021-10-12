using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.UI.Widgets;
using PixelCrew.UI.Windows.Perks;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;
        [SerializeField] private ActivePerkWidget _activePerk;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();

            _trash.Retain(_session.Data.Hp.SubscribeAndInvoke(OnHealthChange));
            _trash.Retain(_session.PerksModel.SubscribeAndInvoke(OnPerkChange));
        }

        private void OnPerkChange()
        {
            var usedPerk = _session.PerksModel.Used;
            if (string.IsNullOrEmpty(usedPerk))
            {
                _activePerk.gameObject.SetActive(false);
            }
            else
            {
                _activePerk.gameObject.SetActive(true);
                var perk = DefsFacade.I.Perks.Get(usedPerk);
                _activePerk.Set(perk);
            }
        }

        private void OnHealthChange(int newValue, int oldValue)
        {
            var maxHealth = DefsFacade.I.Player.MaxHealth;
            var value = (float) newValue / maxHealth;
            _healthBar.SetProgress(value);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}