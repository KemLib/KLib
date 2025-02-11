namespace KLib.Concurrent
{
    /// <summary>
    /// Provides atomic operations for variables that are shared by multiple threads.
    /// </summary>
    public class InterLong
    {
        #region Properties
        private long value;

        /// <summary>
        /// Get or set current value
        /// </summary>
        public long Value
        {
            get => Interlocked.CompareExchange(ref value, 0, 0);
            set => Interlocked.Exchange(ref this.value, value);
        }
        #endregion

        #region Construction
        public InterLong()
        {
            value = 0;
        }
        public InterLong(long value)
        {
            this.value = value;
        }
        #endregion

        #region Method
        /// <summary>
        /// Sets a variable to a specified value as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <returns>current value</returns>
        public long Exchange(long value)
        {
            return Interlocked.Exchange(ref this.value, value);
        }
        /// <summary>
        /// try sets a variable to a specified value as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <returns>true if current value is different from new value</returns>
        public bool TryExchange(long value)
        {
            return Interlocked.Exchange(ref this.value, value) != value;
        }
        /// <summary>
        /// try sets a variable to a specified value as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="currentValue">current value</param>
        /// <returns>true if current value is different from new value</returns>
        public bool TryExchange(long value, out long currentValue)
        {
            currentValue = Interlocked.Exchange(ref this.value, value);
            return value != currentValue;
        }
        /// <summary>
        /// Compares two values for equality and, if they are equal, replaces the first value, as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="comparand">comparand value</param>
        /// <returns>current value</returns>
        public long CompareExchange(long value, long comparand)
        {
            return Interlocked.CompareExchange(ref this.value, value, comparand);
        }
        /// <summary>
        /// Try compares two values for equality and, if they are equal, replaces the first value, as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="comparand">comparand value</param>
        /// <returns>true if current value equals comparand value and current value is different from new value</returns>
        public bool TryCompareExchange(long value, long comparand)
        {
            long oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return oldValue == comparand && oldValue != value;
        }
        /// <summary>
        /// Try compares two values for equality and, if they are equal, replaces the first value, as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="comparand">comparand value</param>
        /// <param name="currentValue">current value</param>
        /// <returns>true if current value equals comparand value and current value is different from new value</returns>
        public bool TryCompareExchange(long value, long comparand, out long currentValue)
        {
            currentValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return currentValue == comparand && currentValue != value;
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
        public static implicit operator long(InterLong tmp) => tmp.value;
        #endregion
    }
}
