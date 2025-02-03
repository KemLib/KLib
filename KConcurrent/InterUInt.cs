namespace KConcurrent
{
    public class InterUInt
    {
        #region Properties
        private uint value;

        public uint Value
        {
            get => Interlocked.CompareExchange(ref value, 0, 0);
            set => Interlocked.Exchange(ref this.value, value);
        }
        #endregion

        #region Construction
        public InterUInt()
        {
            value = 0;
        }
        public InterUInt(uint value)
        {
            this.value = value;
        }
        #endregion

        #region Method
        public uint Exchange(uint value)
        {
            return Interlocked.Exchange(ref this.value, value);
        }
        public bool TryExchange(uint value)
        {
            return Interlocked.Exchange(ref this.value, value) != value;
        }
        public bool TryExchange(uint value, out uint oldValue)
        {
            oldValue = Interlocked.Exchange(ref this.value, value);
            return value != oldValue;
        }
        public uint CompareExchange(uint value, uint comparand)
        {
            return Interlocked.CompareExchange(ref this.value, value, comparand);
        }
        public bool TryCompareExchange(uint value, uint comparand)
        {
            uint oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return oldValue == comparand && oldValue != value;
        }
        public bool TryCompareExchange(uint value, uint comparand, out uint oldValue)
        {
            oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return oldValue == comparand && oldValue != value;
        }
        public uint Increment()
        {
            return Interlocked.Increment(ref value);
        }
        public uint Decrement()
        {
            return Interlocked.Decrement(ref value);
        }
        public uint And(uint value)
        {
            return Interlocked.And(ref this.value, value);
        }
        public uint Or(uint value)
        {
            return Interlocked.Or(ref this.value, value);
        }
        #endregion

        #region Method Static
        public static implicit operator uint(InterUInt tmp) => tmp.Value;
        #endregion
    }
}
