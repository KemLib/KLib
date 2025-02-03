namespace KConcurrent
{
    public class InterInt
    {
        #region Properties
        private int value;

        public int Value
        {
            get => Interlocked.CompareExchange(ref value, 0, 0);
            set => Interlocked.Exchange(ref this.value, value);
        }
        #endregion

        #region Construction
        public InterInt()
        {
            value = 0;
        }
        public InterInt(int value)
        {
            this.value = value;
        }
        #endregion

        #region Method
        public int Exchange(int value)
        {
            return Interlocked.Exchange(ref this.value, value);
        }
        public bool TryExchange(int value)
        {
            return Interlocked.Exchange(ref this.value, value) != value;
        }
        public bool TryExchange(int value, out int oldValue)
        {
            oldValue = Interlocked.Exchange(ref this.value, value);
            return value != oldValue;
        }
        public int CompareExchange(int value, int comparand)
        {
            return Interlocked.CompareExchange(ref this.value, value, comparand);
        }
        public bool TryCompareExchange(int value, int comparand)
        {
            int oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return oldValue == comparand && oldValue != value;
        }
        public bool TryCompareExchange(int value, int comparand, out int oldValue)
        {
            oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return oldValue == comparand && oldValue != value;
        }
        public int Increment()
        {
            return Interlocked.Increment(ref value);
        }
        public int Decrement()
        {
            return Interlocked.Decrement(ref value);
        }
        public int And(int value)
        {
            return Interlocked.And(ref this.value, value);
        }
        public int Or(int value)
        {
            return Interlocked.Or(ref this.value, value);
        }
        #endregion

        #region Method Static
        public static implicit operator int(InterInt tmp) => tmp.Value;
        #endregion
    }
}
