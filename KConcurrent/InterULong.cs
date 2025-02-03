namespace KConcurrent
{
    public class InterULong
    {
        #region Properties
        private ulong value;

        public ulong Value
        {
            get => Interlocked.CompareExchange(ref value, 0, 0);
            set => Interlocked.Exchange(ref this.value, value);
        }
        #endregion

        #region Construction
        public InterULong()
        {
            value = 0;
        }
        public InterULong(ulong value)
        {
            this.value = value;
        }
        #endregion

        #region Method
        public ulong Exchange(ulong value)
        {
            return Interlocked.Exchange(ref this.value, value);
        }
        public bool TryExchange(ulong value)
        {
            return Interlocked.Exchange(ref this.value, value) != value;
        }
        public bool TryExchange(ulong value, out ulong oldValue)
        {
            oldValue = Interlocked.Exchange(ref this.value, value);
            return value != oldValue;
        }
        public ulong CompareExchange(ulong value, ulong comparand)
        {
            return Interlocked.CompareExchange(ref this.value, value, comparand);
        }
        public bool TryCompareExchange(ulong value, ulong comparand)
        {
            ulong oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return oldValue == comparand && oldValue != value;
        }
        public bool TryCompareExchange(ulong value, ulong comparand, out ulong oldValue)
        {
            oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return oldValue == comparand && oldValue != value;
        }
        public ulong Increment()
        {
            return Interlocked.Increment(ref value);
        }
        public ulong Decrement()
        {
            return Interlocked.Decrement(ref value);
        }
        public ulong And(ulong value)
        {
            return Interlocked.And(ref this.value, value);
        }
        public ulong Or(ulong value)
        {
            return Interlocked.Or(ref this.value, value);
        }
        #endregion

        #region Method Static
        public static implicit operator ulong(InterULong tmp) => tmp.value;
        #endregion
    }
}
