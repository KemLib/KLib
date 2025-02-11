namespace KLib.Concurrent
{
    /// <summary>
    /// Provides atomic operations for variables that are shared by multiple threads.
    /// </summary>
    public class InterULong
    {
        #region Properties
        private ulong value;

        /// <summary>
        /// Get or set current value
        /// </summary>
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
        /// <summary>
        /// Sets a variable to a specified value as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <returns>current value</returns>
        public ulong Exchange(ulong value)
        {
            return Interlocked.Exchange(ref this.value, value);
        }
        /// <summary>
        /// try sets a variable to a specified value as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <returns>true if current value is different from new value</returns>
        public bool TryExchange(ulong value)
        {
            return Interlocked.Exchange(ref this.value, value) != value;
        }
        /// <summary>
        /// try sets a variable to a specified value as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="currentValue">current value</param>
        /// <returns>true if current value is different from new value</returns>
        public bool TryExchange(ulong value, out ulong currentValue)
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
        public ulong CompareExchange(ulong value, ulong comparand)
        {
            return Interlocked.CompareExchange(ref this.value, value, comparand);
        }
        /// <summary>
        /// Try compares two values for equality and, if they are equal, replaces the first value, as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="comparand">comparand value</param>
        /// <returns>true if current value equals comparand value and current value is different from new value</returns>
        public bool TryCompareExchange(ulong value, ulong comparand)
        {
            ulong oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return oldValue == comparand && oldValue != value;
        }
        /// <summary>
        /// Try compares two values for equality and, if they are equal, replaces the first value, as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="comparand">comparand value</param>
        /// <param name="currentValue">current value</param>
        /// <returns>true if current value equals comparand value and current value is different from new value</returns>
        public bool TryCompareExchange(ulong value, ulong comparand, out ulong currentValue)
        {
            currentValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return currentValue == comparand && currentValue != value;
        }
        /// <summary>
        /// Increments a specified variable and stores the result, as an atomic operation.
        /// </summary>
        /// <returns>new value</returns>
        public ulong Increment()
        {
            return Interlocked.Increment(ref value);
        }
        /// <summary>
        /// Decrements a specified variable and stores the result, as an atomic operation.
        /// </summary>
        /// <returns>new value</returns>
        public ulong Decrement()
        {
            return Interlocked.Decrement(ref value);
        }
        /// <summary>
        /// Bitwise "ands" two  64-bit signed integers and replaces the first integer with the result, as an atomic operation.
        /// </summary>
        /// <returns>new value</returns>
        public ulong And(ulong value)
        {
            return Interlocked.And(ref this.value, value);
        }
        /// <summary>
        /// Bitwise "ors" two 64-bit signed integers and replaces the first integer with the result, as an atomic operation.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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
