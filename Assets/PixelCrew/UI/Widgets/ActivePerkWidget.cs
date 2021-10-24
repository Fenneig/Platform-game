using PixelCrew.Model;
using PixelCrew.Model.Definitions.Repository;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class ActivePerkWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Slider _cooldownSlider;

        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }

        private void Update()
        {
            var cooldown = _session.PerksModel.Cooldown;
            _cooldownSlider.value = cooldown.RemainingTime / cooldown.Value;
        }

        public void Set(PerkDef perk)
        {
            _icon.sprite = perk.Icon;
        }
    }
}