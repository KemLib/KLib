namespace KLib.Json
{
    public static class ListUtilities
    {
        #region Properties

        #endregion

        #region Get
        /// <summary>
        /// Try get Struct value in List.
        /// </summary>
        private static bool TryGetStruct<T>(List<object?>? list, int index, out T value) where T : struct
        {
            if (list == null || index < 0 || index >= list.Count)
            {
                value = default;
                return false;
            }
            //
            object? obj = list[index];
            if (obj != null && obj is T tmpValue)
            {
                value = tmpValue;
                return true;
            }
            //
            value = default;
            return false;
        }
        /// <summary>
        /// Try get Class object in List.
        /// </summary>
        public static bool TryGetClass<T>(List<object?>? list, int index, out T? value) where T : class
        {
            if (list == null || index < 0 || index >= list.Count)
            {
                value = null;
                return false;
            }
            //
            object? obj = list[index];
            if (obj == null)
            {
                value = null;
                return true;
            }
            if (obj is T tmpValue)
            {
                value = tmpValue;
                return true;
            }
            //
            value = null;
            return false;
        }
        /// <summary>
        /// Try get Enum value in List.
        /// </summary>
        public static bool TryGetEnum<TEnum>(List<object?>? list, int index, out TEnum value) where TEnum : struct
        {
            if (TryGetClass(list, index, out string? stringValue))
            {
                if (Enum.TryParse(stringValue, out value))
                    return true;
                return false;
            }
            value = default;
            return false;
        }
        /// <summary>
        /// Try get bool value in List.
        /// </summary>
        public static bool TryGet(List<object?>? list, int index, out bool value)
        {
            return TryGetStruct(list, index, out value);
        }
        /// <summary>
        /// Try get int value in List.
        /// </summary>
        public static bool TryGet(List<object?>? list, int index, out int value)
        {
            bool isSuccess = TryGetStruct(list, index, out long valueJson);
            value = (int)Math.Clamp(valueJson, int.MinValue, int.MaxValue);
            return isSuccess;
        }
        /// <summary>
        /// Try get long value in List.
        /// </summary>
        public static bool TryGet(List<object?>? list, int index, out long value)
        {
            bool isSuccess = TryGetStruct(list, index, out value);
            return isSuccess;
        }
        /// <summary>
        /// Try get float value in List.
        /// </summary>
        public static bool TryGet(List<object?>? list, int index, out float value)
        {
            bool isSuccess = TryGetStruct(list, index, out double valueJson);
            value = (float)Math.Clamp(valueJson, float.MinValue, float.MaxValue);
            return isSuccess;
        }
        /// <summary>
        /// Try get double value  List.
        /// </summary>
        public static bool TryGet(List<object?>? list, int index, out double value)
        {
            bool isSuccess = TryGetStruct(list, index, out value);
            return isSuccess;
        }
        /// <summary>
        /// Try get string value in List.
        /// </summary>
        public static bool TryGet(List<object?>? list, int index, out string? value)
        {
            return TryGetClass(list, index, out value);
        }
        #endregion

        #region Set
        /// <summary>
        /// Set Struct value in to List.
        /// </summary>
        private static void SetStruct<T>(List<object?>? list, int index, T value) where T : struct
        {
            if (list == null || index < 0 || index >= list.Count)
                return;
            list[index] = value;
        }
        /// <summary>
        /// Set Class object in to List.
        /// </summary>
        public static void SetClass<T>(List<object?>? list, int index, T? value) where T : class
        {
            if (list == null || index < 0 || index >= list.Count)
                return;
            list[index] = value;
        }
        /// <summary>
        /// Set Enum value in to List.
        /// </summary>
        public static void SetEnum<TEnum>(List<object?>? list, int index, TEnum value) where TEnum : struct
        {
            string? valueString = value.ToString();
            valueString ??= string.Empty;
            SetClass(list, index, valueString);
        }
        /// <summary>
        /// Set bool value in to List.
        /// </summary>
        public static void Set(List<object?>? list, int index, bool value)
        {
            SetStruct(list, index, value);
        }
        /// <summary>
        /// Set int value in to List.
        /// </summary>
        public static void Set(List<object?>? list, int index, int value)
        {
            SetStruct(list, index, (long)value);
        }
        /// <summary>
        /// Set long value in to List.
        /// </summary>
        public static void Set(List<object?>? list, int index, long value)
        {
            SetStruct(list, index, value);
        }
        /// <summary>
        /// Set float value in to List.
        /// </summary>
        public static void Set(List<object?>? list, int index, float value)
        {
            SetStruct(list, index, (double)value);
        }
        /// <summary>
        /// Set double value in to List.
        /// </summary>
        public static void Set(List<object?>? list, int index, double value)
        {
            SetStruct(list, index, value);
        }
        /// <summary>
        /// Set string value in to List.
        /// </summary>
        public static void Set(List<object?>? list, int index, string? value)
        {
            SetClass(list, index, value);
        }
        #endregion
    }
}
