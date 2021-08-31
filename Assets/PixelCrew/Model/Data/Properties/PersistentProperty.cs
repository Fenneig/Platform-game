using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    public abstract class PersistentProperty<TPropertyType>
    {
        [SerializeField] protected TPropertyType _value;
        protected TPropertyType Stored;
        private TPropertyType _defaultValue;

        public PersistentProperty(TPropertyType value) 
        {
            _value = value;
        }

        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);

        public event OnPropertyChanged OnChanged;

        public TPropertyType Value 
        {
            get => Stored;
            set 
            {
                var isEquel = Stored.Equals(value);
                if (isEquel) return;
                var oldValue = Stored;
                Write(value);
                Stored = _value = value;
                OnChanged?.Invoke(value, oldValue);
                
            }
        }

        public void Validate() 
        {
            if (!Stored.Equals(_value)) Value = _value;
        }

        protected void Init() 
        {
            Stored = _value = Read(_defaultValue);
        }
        protected abstract void Write(TPropertyType value);

        protected abstract TPropertyType Read(TPropertyType defaultValue);
    }
}