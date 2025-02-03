namespace KConcurrent
{
    public struct InterValueDouble
    {
        #region Properties
        private double value;

        public double Value
        {
            get => Interlocked.CompareExchange(ref value, 0, 0);
            set => Interlocked.Exchange(ref this.value, value);
        }
        #endregion

        #region Construction
        public InterValueDouble()
        {
            value = 0;
        }
        public InterValueDouble(double value)
        {
            this.value = value;
        }
        #endregion

        #region Method
        public double Exchange(double value)
        {
            return Interlocked.Exchange(ref this.value, value);
        }
        public bool TryExchange(double value)
        {
            return Interlocked.Exchange(ref this.value, value) != value;
        }
        public bool TryExchange(double value, out double oldValue)
        {
            oldValue = Interlocked.Exchange(ref this.value, value);
            return value != oldValue;
        }
        public double CompareExchange(double value, double comparand)
        {
            return Interlocked.CompareExchange(ref this.value, value, comparand);
        }
        public bool TryCompareExchange(double value, double comparand)
        {
            double oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return oldValue == comparand && oldValue != value;
        }
        public bool TryCompareExchange(double value, double comparand, out double oldValue)
        {
            oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return oldValue == comparand && oldValue != value;
        }
        #endregion

        #region Method Static
        public static implicit operator double(InterValueDouble tmp) => tmp.value;
        #endregion
    }
}
