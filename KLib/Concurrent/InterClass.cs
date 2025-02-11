namespace KLib.Concurrent
{
    /// <summary>
    /// Provides atomic operations for variables that are shared by multiple threads.
    /// </summary>
    public class InterClass<T> where T : class
    {
        #region Properties
        private T? value;

        /// <summary>
        /// Get or set current value
        /// </summary>
        public T? Value
        {
            get => Interlocked.CompareExchange(ref value, null, null);
            set => Interlocked.Exchange(ref this.value, value);
        }
        #endregion

        #region Construction
        public InterClass()
        {
            value = null;
        }
        public InterClass(T? value)
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
        public T? Exchange(T? value)
        {
            return Interlocked.Exchange(ref this.value, value);
        }
        /// <summary>
        /// try sets a variable to a specified value as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <returns>true if current value is different from new value</returns>
        public bool TryExchange(T? value)
        {
            T? oldValue = Interlocked.Exchange(ref this.value, value);
            if (oldValue is null)
                return value is null;
            return !oldValue.Equals(value);
        }
        /// <summary>
        /// try sets a variable to a specified value as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="currentValue">current value</param>
        /// <returns>true if current value is different from new value</returns>
        public bool TryExchange(T? value, out T? currentValue)
        {
            currentValue = Interlocked.Exchange(ref this.value, value);
            if (currentValue is null)
                return value is null;
            return !currentValue.Equals(value);
        }
        /// <summary>
        /// Compares two values for equality and, if they are equal, replaces the first value, as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="comparand">comparand value</param>
        /// <returns>current value</returns>
        public T? CompareExchange(T? value, T? comparand)
        {
            return Interlocked.CompareExchange(ref this.value, value, comparand);
        }
        /// <summary>
        /// Try compares two values for equality and, if they are equal, replaces the first value, as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="comparand">comparand value</param>
        /// <returns>true if current value equals comparand value and current value is different from new value</returns>
        public bool TryCompareExchange(T? value, T? comparand)
        {
            T? oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            if (oldValue is null)
                return comparand is null && value is not null;
            else
                return oldValue.Equals(comparand) && !oldValue.Equals(value);
        }
        /// <summary>
        /// Try compares two values for equality and, if they are equal, replaces the first value, as an atomic operation.
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="comparand">comparand value</param>
        /// <param name="currentValue">current value</param>
        /// <returns>true if current value equals comparand value and current value is different from new value</returns>
        public bool TryCompareExchange(T? value, T? comparand, out T? currentValue)
        {
            currentValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            if (currentValue is null)
                return comparand is null && value is not null;
            else
                return currentValue.Equals(comparand) && !currentValue.Equals(value);
        }
        #endregion

        #region Method Static
        public static implicit operator T?(InterClass<T> tmp) => tmp.Value;
        #endregion
    }
}
