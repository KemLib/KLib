namespace KLib.Json
{
    public static class ListUtilities
    {
        #region Properties

        #endregion

        #region Get
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
        public static bool TryGetClass<T>(List<object?>? list, int index, out T? value) where T : class
        {
            if (list == null || index < 0 || index >= list.Count)
            {
                value = default;
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
            value = default;
            return false;
        }
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
        public static bool TryGet(List<object?>? list, int index, out bool value)
        {
            return TryGetStruct(list, index, out value);
        }
        public static bool TryGet(List<object?>? list, int index, out int value)
        {
            bool isSuccess = TryGetStruct(list, index, out long valueJson);
            value = (int)Math.Clamp(valueJson, int.MinValue, int.MaxValue);
            return isSuccess;
        }
        public static bool TryGet(List<object?>? list, int index, out long value)
        {
            bool isSuccess = TryGetStruct(list, index, out value);
            return isSuccess;
        }
        public static bool TryGet(List<object?>? list, int index, out float value)
        {
            bool isSuccess = TryGetStruct(list, index, out double valueJson);
            value = (float)Math.Clamp(valueJson, float.MinValue, float.MaxValue);
            return isSuccess;
        }
        public static bool TryGet(List<object?>? list, int index, out double value)
        {
            bool isSuccess = TryGetStruct(list, index, out value);
            return isSuccess;
        }
        public static bool TryGet(List<object?>? list, int index, [NotNullWhen(true)] out string? value)
        {
            return TryGetClass(list, index, out value);
        }
        #endregion

        #region Set
        private static void SetStruct<T>(List<object?>? list, int index, T value) where T : struct
        {
            if (list == null || index < 0 || index >= list.Count)
                return;
            list[index] = value;
        }
        public static void SetClass<T>(List<object?>? list, int index, T? value) where T : class
        {
            if (list == null || index < 0 || index >= list.Count)
                return;
            list[index] = value;
        }
        public static void SetEnum<TEnum>(List<object?>? list, int index, TEnum value) where TEnum : struct
        {
            string? valueString = value.ToString();
            valueString ??= string.Empty;
            SetClass(list, index, valueString);
        }
        public static void Set(List<object?>? list, int index, bool value)
        {
            SetStruct(list, index, value);
        }
        public static void Set(List<object?>? list, int index, int value)
        {
            SetStruct(list, index, (long)value);
        }
        public static void Set(List<object?>? list, int index, long value)
        {
            SetStruct(list, index, value);
        }
        public static void Set(List<object?>? list, int index, float value)
        {
            SetStruct(list, index, (double)value);
        }
        public static void Set(List<object?>? list, int index, double value)
        {
            SetStruct(list, index, value);
        }
        public static void Set(List<object?>? list, int index, string value)
        {
            SetClass(list, index, value);
        }
        #endregion
    }
}
