namespace KConcurrent
{
    public struct InterValueLong
    {
        #region Properties
        private long value;

        public long Value
        {
            get => Interlocked.CompareExchange(ref value, 0, 0);
            set => Interlocked.Exchange(ref this.value, value);
        }
        #endregion

        #region Construction
        public InterValueLong()
        {
            value = 0;
        }
        public InterValueLong(long value)
        {
            this.value = value;
        }
        #endregion

        #region Method
        public long Exchange(long value)
        {
            return Interlocked.Exchange(ref this.value, value);
        }
        public bool TryExchange(long value)
        {
            return Interlocked.Exchange(ref this.value, value) != value;
        }
        public bool TryExchange(long value, out long oldValue)
        {
            oldValue = Interlocked.Exchange(ref this.value, value);
            return value != oldValue;
        }
        public long CompareExchange(long value, long comparand)
        {
            return Interlocked.CompareExchange(ref this.value, value, comparand);
        }
        public bool TryCompareExchange(long value, long comparand)
        {
            long oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return oldValue == comparand && oldValue != value;
        }
        public bool TryCompareExchange(long value, long comparand, out long oldValue)
        {
            oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return oldValue == comparand && oldValue != value;
        }
        public long Increment()
        {
            return Interlocked.Increment(ref value);
        }
        public long Decrement()
        {
            return Interlocked.Decrement(ref value);
        }
        public long And(long value)
        {
            return Interlocked.And(ref this.value, value);
        }
        public long Or(long value)
        {
            return Interlocked.Or(ref this.value, value);
        }
        #endregion

        #region Method Static
        public static implicit operator long(InterValueLong tmp) => tmp.value;
        #endregion
    }
}
