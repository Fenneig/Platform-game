using System;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.Model.Models
{
    [Serializable]
    public class PerksModel : IDisposable
    {
        private readonly PlayerData _data;
        public readonly StringProperty InterfaceSelection = new StringProperty();

        public Timer Cooldown = new Timer();
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public event Action OnChanged;

        public string Used => _data.Perks.Used.Value;

        public bool IsDoubleJumpSupported => _data.Perks.Used.Value == "doubleJump" && Cooldown.IsReady;
        public bool IsChargedThrowSupported => _data.Perks.Used.Value == "chargedThrow" && Cooldown.IsReady;
        public bool IsDashSupported => _data.Perks.Used.Value == "dash" && Cooldown.IsReady;
        public bool IsShieldSupported => _data.Perks.Used.Value == "shield" && Cooldown.IsReady;


        public PerksModel(PlayerData data)
        {
            _data = data;
            InterfaceSelection.Value = DefsFacade.I.Perks.All[0].Id;
            
            _trash.Retain(_data.Perks.Used.Subscribe((x, y) => OnChanged?.Invoke()));
            _trash.Retain(InterfaceSelection.Subscribe((x, y) => OnChanged?.Invoke()));
            
            Cooldown.Value = DefsFacade.I.Perks.Get(InterfaceSelection.Value).Cooldown.Value;
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }
        
        public IDisposable SubscribeAndInvoke(Action call)
        {
            OnChanged += call;
            var dispose = new ActionDisposable(() => OnChanged -= call);
            call();
            return dispose;
        }

        public void Unlock(string id)
        {
            var def = DefsFacade.I.Perks.Get(id);
            var isEnoughResources = _data.Inventory.IsEnough(def.Price);

            if (isEnoughResources)
            {
                _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);
                _data.Perks.AddPerk(id);

                OnChanged?.Invoke();
            }
        }

        public void SelectPerk(string selected)
        {
            var perkDef = DefsFacade.I.Perks.Get(selected);
            _data.Perks.Used.Value = selected;
            Cooldown.Value = perkDef.Cooldown.Value;
        }

        public bool IsUsed(string perkId)
        {
            return _data.Perks.Used.Value == perkId;
        }

        public bool IsUnlocked(string perkId)
        {
            return _data.Perks.IsUnlocked(perkId);
        }

        public bool CanBuy(string perkId)
        {
            var def = DefsFacade.I.Perks.Get(perkId);
            return _data.Inventory.IsEnough(def.Price);
        }

        public void Dispose()
        {
            _trash.Dispose();
        }
    }
}