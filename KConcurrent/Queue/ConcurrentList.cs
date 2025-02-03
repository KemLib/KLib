namespace KConcurrent.Queue
{
    public class ConcurrentList<T>
    {
        #region Properties
        private const string ERROR_OPERATION_CANCELED_EXCEPTION = "CancellationToken was canceled",
            ERROR_LOCKER_ENTER_FAIL = "Locker enter fail";
        private static readonly T[] ARRAY_EMPTY = [];

        private readonly List<T> list;
        private SpinLock lockObject;
        private InterValueInt interCount;

        public int Count => interCount.Value;
        public T this[int index] => Get(index);
        #endregion

        #region Construction
        public ConcurrentList()
        {
            list = [];
            lockObject = new();
            interCount = new();
        }
        public ConcurrentList(int capacity)
        {
            list = new(Math.Max(0, capacity));
            lockObject = new();
            interCount = new();
        }
        #endregion

        #region In Add
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Add(T value)
        {
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    list.Add(value);
                    interCount.Increment();
                }
                else
                {
                    throw new Exception(ERROR_LOCKER_ENTER_FAIL);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (lockToken)
                    lockObject.Exit();
            }
        }
        public bool TryAdd(T value)
        {
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    list.Add(value);
                    interCount.Increment();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (lockToken)
                    lockObject.Exit();
            }
        }
        #endregion

        #region In Add Range
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void AddRange(T[] values)
        {
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    list.AddRange(values);
                    interCount.Exchange(list.Count);
                }
                else
                {
                    throw new Exception(ERROR_LOCKER_ENTER_FAIL);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (lockToken)
                    lockObject.Exit();
            }
        }
        public bool TryAddRange(T[] values)
        {
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    list.AddRange(values);
                    interCount.Exchange(list.Count);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (lockToken)
                    lockObject.Exit();
            }
        }
        #endregion

        #region Insert
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Insert(int index, T value)
        {
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    list.Insert(index, value);
                    interCount.Increment();
                }
                else
                {
                    throw new Exception(ERROR_LOCKER_ENTER_FAIL);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (lockToken)
                    lockObject.Exit();
            }
        }
        public bool TryInsert(int index, T value)
        {
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    list.Insert(index, value);
                    interCount.Increment();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (lockToken)
                    lockObject.Exit();
            }
        }
        #endregion

        #region Remove
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Remove(int index)
        {
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    list.RemoveAt(index);
                    interCount.Decrement();
                }
                else
                {
                    throw new Exception(ERROR_LOCKER_ENTER_FAIL);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (lockToken)
                    lockObject.Exit();
            }
        }
        public bool TryRemove(int index)
        {
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    list.RemoveAt(index);
                    interCount.Decrement();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (lockToken)
                    lockObject.Exit();
            }
        }
        #endregion

        #region Out
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception"></exception>
        public T Get(int index)
        {
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    return list[index];
                }
                else
                {
                    throw new Exception(ERROR_LOCKER_ENTER_FAIL);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (lockToken)
                    lockObject.Exit();
            }
        }
        public bool TryGet(int index, out T? value)
        {
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    value = list[index];
                    return true;
                }
                else
                {
                    value = default;
                    return false;
                }
            }
            catch (Exception)
            {
                value = default;
                return false;
            }
            finally
            {
                if (lockToken)
                    lockObject.Exit();
            }
        }
        public T[] ToArray()
        {
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    if (list.Count == 0)
                    {
                        return ARRAY_EMPTY;
                    }
                    //
                    T[] values = [.. list];
                    return values;
                }
                else
                {
                    return ARRAY_EMPTY;
                }
            }
            catch (Exception)
            {
                return ARRAY_EMPTY;
            }
            finally
            {
                if (lockToken)
                    lockObject.Exit();
            }
        }
        public bool ToArray([NotNullWhen(true)] out T[]? values)
        {
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    if (list.Count == 0)
                    {
                        values = ARRAY_EMPTY;
                        return true;
                    }
                    //
                    values = [.. list];
                    return true;
                }
                else
                {
                    values = null;
                    return false;
                }
            }
            catch (Exception)
            {
                values = null;
                return false;
            }
            finally
            {
                if (lockToken)
                    lockObject.Exit();
            }
        }
        #endregion

        #region Clear
        public T[] Clear()
        {
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    if (list.Count == 0)
                    {
                        return ARRAY_EMPTY;
                    }
                    //
                    T[] values = [.. list];
                    list.Clear();
                    interCount.Exchange(0);
                    return values;
                }
                else
                {
                    return ARRAY_EMPTY;
                }
            }
            catch (Exception)
            {
                return ARRAY_EMPTY;
            }
            finally
            {
                if (lockToken)
                    lockObject.Exit();
            }
        }
        public bool TryClear([NotNullWhen(true)] out T[]? values)
        {
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    if (list.Count == 0)
                    {
                        values = null;
                        return false;
                    }
                    //
                    values = [.. list];
                    list.Clear();
                    interCount.Exchange(0);
                    return true;
                }
                else
                {
                    values = null;
                    return false;
                }
            }
            catch (Exception)
            {
                values = null;
                return false;
            }
            finally
            {
                if (lockToken)
                    lockObject.Exit();
            }
        }
        #endregion
    }
}
