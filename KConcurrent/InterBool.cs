namespace KConcurrent
{
    /// <summary>
    /// Provides atomic operations for variables that are shared by multiple threads.
    /// </summary>
    public class InterBool
    {
        #region Properties
        private int value;

        /// <summary>
        /// Get or set current value
        /// </summary>
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
        /// <summary>
        /// Sets a variable to a specified value as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <returns>current value</returns>
        public bool Exchange(bool value)
        {
            return Interlocked.Exchange(ref this.value, value ? 1 : 0) == 1;
        }
        /// <summary>
        /// try sets a variable to a specified value as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <returns>true if current value is different from new value</returns>
        public bool TryExchange(bool value)
        {
            int intValue = value ? 1 : 0;
            return Interlocked.Exchange(ref this.value, intValue) != intValue;
        }
        /// <summary>
        /// try sets a variable to a specified value as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="currentValue">current value</param>
        /// <returns>true if current value is different from new value</returns>
        public bool TryExchange(bool value, out bool currentValue)
        {
            int intValue = value ? 1 : 0;
            currentValue = Interlocked.Exchange(ref this.value, intValue) == 1;
            return value != currentValue;
        }
        /// <summary>
        /// Compares two values for equality and, if they are equal, replaces the first value, as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="comparand">comparand value</param>
        /// <returns>current value</returns>
        public int CompareExchange(bool value, bool comparand)
        {
            return Interlocked.CompareExchange(ref this.value, value ? 1 : 0, comparand ? 1 : 0);
        }
        /// <summary>
        /// Try compares two values for equality and, if they are equal, replaces the first value, as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="comparand">comparand value</param>
        /// <returns>true if current value equals comparand value and current value is different from new value</returns>
        public bool TryCompareExchange(bool value, bool comparand)
        {
            bool oldValue = Interlocked.CompareExchange(ref this.value, value ? 1 : 0, comparand ? 1 : 0) == 1;
            return oldValue == comparand && oldValue != value;
        }
        /// <summary>
        /// Try compares two values for equality and, if they are equal, replaces the first value, as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="comparand">comparand value</param>
        /// <param name="currentValue">current value</param>
        /// <returns>true if current value equals comparand value and current value is different from new value</returns>
        public bool TryCompareExchange(bool value, bool comparand, out bool currentValue)
        {
            currentValue = Interlocked.CompareExchange(ref this.value, value ? 1 : 0, comparand ? 1 : 0) == 1;
            return currentValue == comparand && currentValue != value;
        }
        #endregion

        #region Method Static
        public static implicit operator bool(InterBool tmp) => tmp.Value;
        #endregion
    }
}
