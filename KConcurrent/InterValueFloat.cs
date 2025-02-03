namespace KConcurrent
{
    /// <summary>
    /// Provides atomic operations for variables that are shared by multiple threads.
    /// </summary>
    public struct InterValueFloat
    {
        #region Properties
        private float value;

        /// <summary>
        /// Get or set current value
        /// </summary>
        public float Value
        {
            get => Interlocked.CompareExchange(ref value, 0, 0);
            set => Interlocked.Exchange(ref this.value, value);
        }
        #endregion

        #region Construction
        public InterValueFloat()
        {
            value = 0;
        }
        public InterValueFloat(float value)
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
        public float Exchange(float value)
        {
            return Interlocked.Exchange(ref this.value, value);
        }
        /// <summary>
        /// try sets a variable to a specified value as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <returns>true if current value is different from new value</returns>
        public bool TryExchange(float value)
        {
            return Interlocked.Exchange(ref this.value, value) != value;
        }
        /// <summary>
        /// try sets a variable to a specified value as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="currentValue">current value</param>
        /// <returns>true if current value is different from new value</returns>
        public bool TryExchange(float value, out float currentValue)
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
        public float CompareExchange(float value, float comparand)
        {
            return Interlocked.CompareExchange(ref this.value, value, comparand);
        }
        /// <summary>
        /// Try compares two values for equality and, if they are equal, replaces the first value, as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="comparand">comparand value</param>
        /// <returns>true if current value equals comparand value and current value is different from new value</returns>
        public bool TryCompareExchange(float value, float comparand)
        {
            float oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return oldValue == comparand && oldValue != value;
        }
        /// <summary>
        /// Try compares two values for equality and, if they are equal, replaces the first value, as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="comparand">comparand value</param>
        /// <param name="currentValue">current value</param>
        /// <returns>true if current value equals comparand value and current value is different from new value</returns>
        public bool TryCompareExchange(float value, float comparand, out float currentValue)
        {
            currentValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            return currentValue == comparand && currentValue != value;
        }
        #endregion

        #region Method Static
        public static implicit operator float(InterValueFloat tmp) => tmp.value;
        #endregion
    }
}
