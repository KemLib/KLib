namespace KLib.Concurrent.Queue
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

        /// <summary>
        /// Gets the number of elements contained in the ConcurrentList<T>.
        /// </summary>
        public int Count => interCount.Value;
        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
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
        /// Adds an object to the end of the List<T>.
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
        /// <summary>
        /// Try adds an object to the end of the List<T>.
        /// </summary>
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
        /// Adds the elements of the specified collection to the end of the List<T>.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void AddRange(IEnumerable<T> values)
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
        /// <summary>
        /// Try adds the elements of the specified collection to the end of the List<T>.
        /// </summary>
        public bool TryAddRange(IEnumerable<T> values)
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
        /// Inserts an element into the List<T> at the specified index.
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
        /// <summary>
        /// Try inserts an element into the List<T> at the specified index.
        /// </summary>
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

        #region Insert Range
        /// <summary>
        /// Inserts the elements of a collection into the List<T> at the specified index.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void InsertRange(int index, IEnumerable<T> value)
        {
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    list.InsertRange(index, value);
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
        /// <summary>
        /// Inserts the elements of a collection into the List<T> at the specified index.
        /// </summary>
        public bool TryInsertRange(int index, IEnumerable<T> value)
        {
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    list.InsertRange(index, value);
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
        /// Removes the first occurrence of a specific object from the List<T>.
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
        /// <summary>
        /// Try removes the first occurrence of a specific object from the List<T>.
        /// </summary>
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

        #region Remove At
        /// <summary>
        /// Removes a range of elements from the List<T>.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void RemoveAt(int index, int count)
        {
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    list.RemoveRange(index, count);
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
        /// <summary>
        /// Removes a range of elements from the List<T>.
        /// </summary>
        public bool TryRemoveAt(int index, int count)
        {
            bool lockToken = false;
            try
            {
                lockObject.Enter(ref lockToken);
                if (lockToken)
                {
                    list.RemoveRange(index, count);
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
        /// Gets the element at the specified index.
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
        /// <summary>
        /// Try gets the element at the specified index.
        /// </summary>
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
        /// <summary>
        /// Copies the elements of the List<T> to a new array.
        /// </summary>
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
        /// <summary>
        /// Try copies the elements of the List<T> to a new array.
        /// </summary>
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
        /// <summary>
        /// Removes all elements from the List<T>
        /// </summary>
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
        /// <summary>
        /// Try removes all elements from the List<T>
        /// </summary>
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
