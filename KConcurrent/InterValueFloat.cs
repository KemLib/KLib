namespace KConcurrent
{
    public struct InterValueFloat
    {
        #region Properties
        private float value;

        public float Value
        {
            get => Interlocked.CompareExchange(ref value, 0, 0);
            set => Interlocked.Exchange(ref this.value, value);
        }
        #endregion

        #region Construction
        public InterValueFloat()
        {
            value = 0;
        }
        public InterValueFloat(float value)
        {
            this.value = value;
        }
        #endregion

        #region Method
        public float Exchange(float value)
        {
            return Interlocked.Exchange(ref this.value, value);
        }
        public bool TryExchange(float value)
        {
            return Interlocked.Exchange(ref this.value, value) != value;
        }
        public bool TryExchange(float value, out float oldValue)
        {
            oldValue = Interlocked.Exchange(ref this.value, value);
            return value != oldValue;
        }
        public float CompareExchange(float value, float comparand)
        {
            return Interlocked.CompareExchange(ref this.value, value, comparand);
        }
        public bool TryCompareExchange(float value, float comparand)
        {
            float oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return oldValue == comparand && oldValue != value;
        }
        public bool TryCompareExchange(float value, float comparand, out float oldValue)
        {
            oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return oldValue == comparand && oldValue != value;
        }
        #endregion

        #region Method Static
        public static implicit operator float(InterValueFloat tmp) => tmp.value;
        #endregion
    }
}
