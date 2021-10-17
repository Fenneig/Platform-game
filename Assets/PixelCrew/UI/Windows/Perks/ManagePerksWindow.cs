﻿using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Model.Definitions.Repository;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.Perks
{
    public class ManagePerksWindow : AnimatedWindow
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _useButton;
        [SerializeField] private ItemWidget _price;
        [SerializeField] private Text _infoText;
        [SerializeField] private Transform _perksContainer;

        private float _defaultTimeScale;
        private PlayerInput _input;

        private PredefinedDataGroup<PerkDef, PerkWidget> _dataGroup;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private GameSession _session;

        protected override void Start()
        {
            base.Start();
            
            _input = FindObjectOfType<PlayerInput>();
            _defaultTimeScale = Time.timeScale;
            PauseGame();

            _dataGroup = new PredefinedDataGroup<PerkDef, PerkWidget>(_perksContainer);
            _session = FindObjectOfType<GameSession>();

            _trash.Retain(_session.PerksModel.Subscribe(OnPerksChanged));
            _trash.Retain(_buyButton.onClick.Subscribe(OnBuy));
            _trash.Retain(_useButton.onClick.Subscribe(OnUse));

            OnPerksChanged();
        }

        private void OnPerksChanged()
        {
            _dataGroup.SetData(DefsFacade.I.Perks.All);

            var selected = _session.PerksModel.InterfaceSelection.Value;

            _useButton.gameObject.SetActive(_session.PerksModel.IsUnlocked(selected));
            _useButton.interactable = _session.PerksModel.Used != selected;

            _buyButton.gameObject.SetActive(!_session.PerksModel.IsUnlocked(selected));
            _buyButton.interactable = _session.PerksModel.CanBuy(selected);

            var def = DefsFacade.I.Perks.Get(selected);
            _price.SetData(def.Price);

            _infoText.text = LocalizationManager.I.Localize(def.Info);
            _infoText.font = LocalizationManager.I.SetFont();
        }

        private void OnBuy()
        {
            var selected = _session.PerksModel.InterfaceSelection.Value;
            _session.PerksModel.Unlock(selected);
        }

        private void OnUse()
        {
            var selected = _session.PerksModel.InterfaceSelection.Value;
            _session.PerksModel.SelectPerk(selected);
        }
        
        private void PauseGame()
        {
            Time.timeScale = 0f;
            _input.enabled = false;
        }

        private void ResumeGame()
        {
            Time.timeScale = _defaultTimeScale;
            _input.enabled = true;
        }

        private void OnDestroy()
        {
            ResumeGame();
            _trash.Dispose();
        }
    }
}