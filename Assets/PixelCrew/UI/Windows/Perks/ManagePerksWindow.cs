using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Model.Definitions.Repository;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.Perks
{
    public class ManagePerksWindow : InGameAnimatedWindow
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _useButton;
        [SerializeField] private ItemWidget _price;
        [SerializeField] private Text _infoText;
        [SerializeField] private Transform _perksContainer;

        private PredefinedDataGroup<PerkDef, PerkWidget> _dataGroup;

        private readonly CompositeDisposable _trash = new CompositeDisposable();


        protected override void Start()
        {
            base.Start();
            
            _dataGroup = new PredefinedDataGroup<PerkDef, PerkWidget>(_perksContainer);

            _trash.Retain(Session.PerksModel.Subscribe(OnPerksChanged));
            _trash.Retain(_buyButton.onClick.Subscribe(OnBuy));
            _trash.Retain(_useButton.onClick.Subscribe(OnUse));

            OnPerksChanged();
        }

        private void OnPerksChanged()
        {
            _dataGroup.SetData(DefsFacade.I.Perks.All);

            var selected = Session.PerksModel.InterfaceSelection.Value;

            _useButton.gameObject.SetActive(Session.PerksModel.IsUnlocked(selected));
            _useButton.interactable = Session.PerksModel.Used != selected;

            _buyButton.gameObject.SetActive(!Session.PerksModel.IsUnlocked(selected));
            _buyButton.interactable = Session.PerksModel.CanBuy(selected);

            var def = DefsFacade.I.Perks.Get(selected);
            _price.SetData(def.Price);

            _infoText.text = LocalizationManager.I.Localize(def.Info);
            _infoText.font = LocalizationManager.I.SetFont();
        }

        private void OnBuy()
        {
            var selected = Session.PerksModel.InterfaceSelection.Value;
            Session.PerksModel.Unlock(selected);
        }

        private void OnUse()
        {
            var selected = Session.PerksModel.InterfaceSelection.Value;
            Session.PerksModel.SelectPerk(selected);
        }
  

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}