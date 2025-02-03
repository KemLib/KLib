namespace KConcurrent
{
    public class InterClass<T> where T : class
    {
        #region Properties
        private T? value;

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
        public T? Exchange(T? value)
        {
            return Interlocked.Exchange(ref this.value, value);
        }
        public bool TryExchange(T? value)
        {
            T? oldValue = Interlocked.Exchange(ref this.value, value);
            if (oldValue is null)
                return value is null;
            return !oldValue.Equals(value);
        }
        public bool TryExchange(T? value, out T? oldValue)
        {
            oldValue = Interlocked.Exchange(ref this.value, value);
            if (oldValue is null)
                return value is null;
            return !oldValue.Equals(value);
        }
        public T? CompareExchange(T? value, T? comparand)
        {
            return Interlocked.CompareExchange(ref this.value, value, comparand);
        }
        public bool TryCompareExchange(T? value, T? comparand)
        {
            T? oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            if (oldValue is null)
                return comparand is null && value is not null;
            else
                return oldValue.Equals(comparand) && !oldValue.Equals(value);
        }
        public bool TryCompareExchange(T? value, T? comparand, out T? oldValue)
        {
            oldValue = Interlocked.CompareExchange(ref this.value, value, comparand);
            if (oldValue is null)
                return comparand is null && value is not null;
            else
                return oldValue.Equals(comparand) && !oldValue.Equals(value);
        }
        #endregion

        #region Method Static
        public static implicit operator T?(InterClass<T> tmp) => tmp.Value;
        #endregion
    }
}
