namespace KConcurrent
{
    public class InterBool
    {
        #region Properties
        private int value;

        public bool Value
        {
            get => Interlocked.CompareExchange(ref value, 0, 0) == 1;
            set => Interlocked.Exchange(ref this.value, value ? 1 : 0);
        }
        #endregion

        #region Construction
        public InterBool()
        {
            value = 0;
        }
        public InterBool(bool value)
        {
            this.value = value ? 1 : 0;
        }
        #endregion

        #region Method
        public bool Exchange(bool value)
        {
            return Interlocked.Exchange(ref this.value, value ? 1 : 0) == 1;
        }
        public bool TryExchange(bool value)
        {
            int intValue = value ? 1 : 0;
            return Interlocked.Exchange(ref this.value, intValue) != intValue;
        }
        public bool TryExchange(bool value, out bool oldValue)
        {
            int intValue = value ? 1 : 0;
            oldValue = Interlocked.Exchange(ref this.value, intValue) == 1;
            return value != oldValue;
        }
        public int CompareExchange(bool value, bool comparand)
        {
            return Interlocked.CompareExchange(ref this.value, value ? 1 : 0, comparand ? 1 : 0);
        }
        public bool TryCompareExchange(bool value, bool comparand)
        {
            bool oldValue = Interlocked.CompareExchange(ref this.value, value ? 1 : 0, comparand ? 1 : 0) == 1;
            return oldValue == comparand && oldValue != value;
        }
        public bool TryCompareExchange(bool value, bool comparand, out bool oldValue)
        {
            oldValue = Interlocked.CompareExchange(ref this.value, value ? 1 : 0, comparand ? 1 : 0) == 1;
            return oldValue == comparand && oldValue != value;
        }
        #endregion

        #region Method Static
        public static implicit operator bool(InterBool tmp) => tmp.Value;
        #endregion
    }
}
