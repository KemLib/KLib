namespace KLib.Concurrent
{
    /// <summary>
    /// Provides atomic operations for variables that are shared by multiple threads.
    /// </summary>
    public struct InterValueUInt
    {
        #region Properties
        private uint value;

        /// <summary>
        /// Get or set current value
        /// </summary>
        public uint Value
        {
            get => Interlocked.CompareExchange(ref value, 0, 0);
            set => Interlocked.Exchange(ref this.value, value);
        }
        #endregion

        #region Construction
        public InterValueUInt()
        {
            value = 0;
        }
        public InterValueUInt(uint value)
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
        public uint Exchange(uint value)
        {
            return Interlocked.Exchange(ref this.value, value);
        }
        /// <summary>
        /// try sets a variable to a specified value as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <returns>true if current value is different from new value</returns>
        public bool TryExchange(uint value)
        {
            return Interlocked.Exchange(ref this.value, value) != value;
        }
        /// <summary>
        /// try sets a variable to a specified value as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="currentValue">current value</param>
        /// <returns>true if current value is different from new value</returns>
        public bool TryExchange(uint value, out uint currentValue)
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
        public uint CompareExchange(uint value, uint comparand)
        {
            return Interlocked.CompareExchange(ref this.value, value, comparand);
        }
        /// <summary>
        /// Try compares two values for equality and, if they are equal, replaces the first value, as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="comparand">comparand value</param>
        /// <returns>true if current value equals comparand value and current value is different from new value</returns>
        public bool TryCompareExchange(uint value, uint comparand)
        {
            uint oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return oldValue == comparand && oldValue != value;
        }
        /// <summary>
        /// Try compares two values for equality and, if they are equal, replaces the first value, as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="comparand">comparand value</param>
        /// <param name="currentValue">current value</param>
        /// <returns>true if current value equals comparand value and current value is different from new value</returns>
        public bool TryCompareExchange(uint value, uint comparand, out uint currentValue)
        {
            currentValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return currentValue == comparand && currentValue != value;
        }
        /// <summary>
        /// Increments a specified variable and stores the result, as an atomic operation.
        /// </summary>
        /// <returns>new value</returns>
        public uint Increment()
        {
            return Interlocked.Increment(ref value);
        }
        /// <summary>
        /// Decrements a specified variable and stores the result, as an atomic operation.
        /// </summary>
        /// <returns>new value</returns>
        public uint Decrement()
        {
            return Interlocked.Decrement(ref value);
        }
        /// <summary>
        /// Bitwise "ands" two 32-bit signed integers and replaces the first integer with the result, as an atomic operation.
        /// </summary>
        /// <returns>new value</returns>
        public uint And(uint value)
        {
            return Interlocked.And(ref this.value, value);
        }
        /// <summary>
        /// Bitwise "ors" two 32-bit signed integers and replaces the first integer with the result, as an atomic operation.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public uint Or(uint value)
        {
            return Interlocked.Or(ref this.value, value);
        }
        #endregion

        #region Method Static
        public static implicit operator uint(InterValueUInt tmp) => tmp.Value;
        #endregion
    }
}
