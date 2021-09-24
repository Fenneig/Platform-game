namespace PixelCrew.Model.Data.Properties
{
    public abstract class PersistentProperty<TPropertyType> : ObservableProperty<TPropertyType>
    {
        protected TPropertyType Stored;
        private TPropertyType _defaultValue;

        public PersistentProperty(TPropertyType defaultValue) 
        {
            _defaultValue = defaultValue;
        }
        
        public override TPropertyType Value 
        {
            get => Stored;
            set 
            {
                var isEquel = Stored.Equals(value);
                if (isEquel) return;
                var oldValue = Stored;
                Write(value);
                Stored = _value = value;
                InvokeChangedEvent(value, oldValue);                
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