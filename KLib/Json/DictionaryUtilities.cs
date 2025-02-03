namespace KLib.Json
{
    public static class DictionaryUtilities
    {
        #region Properties

        #endregion

        #region Get
        private static bool TryGetStruct<T>(Dictionary<string, object?>? dic, string key, out T value) where T : struct
        {
            if (dic == null)
            {
                value = default;
                return false;
            }
            //
            if (dic.TryGetValue(key, out object? obj) && obj != null && obj is T tmpValue)
            {
                value = tmpValue;
                return true;
            }
            //
            value = default;
            return false;
        }
        public static bool TryGetClass<T>(Dictionary<string, object?>? dic, string key, out T? value) where T : class
        {
            if (dic == null)
            {
                value = null;
                return false;
            }
            //
            if (dic.TryGetValue(key, out object? obj))
            {
                if (obj == null)
                {
                    value = null;
                    return true;
                }
                //
                if (obj is T tmpValue)
                {
                    value = tmpValue;
                    return true;
                }
            }
            //
            value = null;
            return false;
        }
        public static bool TryGetEnum<TEnum>(Dictionary<string, object?>? dic, string key, out TEnum value) where TEnum : struct
        {
            if (TryGetClass(dic, key, out string? stringValue))
            {
                if (Enum.TryParse(stringValue, out value))
                    return true;
                return false;
            }
            value = default;
            return false;
        }
        public static bool TryGet(Dictionary<string, object?>? dic, string key, out bool value)
        {
            return TryGetStruct(dic, key, out value);
        }
        public static bool TryGet(Dictionary<string, object?>? dic, string key, out int value)
        {
            bool isSuccess = TryGetStruct(dic, key, out long valueJson);
            value = (int)Math.Clamp(valueJson, int.MinValue, int.MaxValue);
            return isSuccess;
        }
        public static bool TryGet(Dictionary<string, object?>? dic, string key, out long value)
        {
            bool isSuccess = TryGetStruct(dic, key, out value);
            return isSuccess;
        }
        public static bool TryGet(Dictionary<string, object?>? dic, string key, out float value)
        {
            bool isSuccess = TryGetStruct(dic, key, out double valueJson);
            value = (float)Math.Clamp(valueJson, float.MinValue, float.MaxValue);
            return isSuccess;
        }
        public static bool TryGet(Dictionary<string, object?>? dic, string key, out double value)
        {
            bool isSuccess = TryGetStruct(dic, key, out value);
            return isSuccess;
        }
        public static bool TryGet(Dictionary<string, object?>? dic, string key, [NotNullWhen(true)] out string? value)
        {
            return TryGetClass(dic, key, out value);
        }
        #endregion

        #region Set
        private static void SetStruct<T>(Dictionary<string, object?>? dic, string key, T value) where T : struct
        {
            if (dic == null || string.IsNullOrEmpty(key))
                return;
            if (dic.ContainsKey(key))
                dic[key] = value;
            else
                dic.Add(key, value);
        }
        public static void SetClass<T>(Dictionary<string, object?>? dic, string key, T? value) where T : class
        {
            if (dic == null || string.IsNullOrEmpty(key))
                return;
            if (dic.ContainsKey(key))
                dic[key] = value;
            else
                dic.Add(key, value);
        }
        public static void SetEnum<TEnum>(Dictionary<string, object?>? dic, string key, TEnum value) where TEnum : struct
        {
            string? valueString = value.ToString();
            valueString ??= string.Empty;
            SetClass(dic, key, valueString);
        }
        public static void Set(Dictionary<string, object?>? dic, string key, bool value)
        {
            SetStruct(dic, key, value);
        }
        public static void Set(Dictionary<string, object?>? dic, string key, int value)
        {
            SetStruct(dic, key, (long)value);
        }
        public static void Set(Dictionary<string, object?>? dic, string key, long value)
        {
            SetStruct(dic, key, value);
        }
        public static void Set(Dictionary<string, object?>? dic, string key, float value)
        {
            SetStruct(dic, key, (double)value);
        }
        public static void Set(Dictionary<string, object?>? dic, string key, double value)
        {
            SetStruct(dic, key, value);
        }
        public static void Set(Dictionary<string, object?>? dic, string key, string value)
        {
            SetClass(dic, key, value);
        }
        #endregion
    }
}
