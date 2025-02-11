namespace KLib.Byson
{
    public static class BysonUIntConvert
    {
        #region Properties

        #endregion

        #region TryDecode
        /// <summary>
        /// Try decode byson to data.
        /// </summary>
        public static bool TryDecode(byte[]? byson, int offset, [NotNullWhen(true)] out byte[]? plainData, out int count)
        {
            if (byson == null || byson.Length == 0)
            {
                plainData = null;
                count = 0;
                return false;
            }
            //
            count = 0;
            if (!BysonUInt.TryConvert(byson, offset, out uint length, out int readCount) || length < 0)
            {
                plainData = null;
                count = 0;
                return false;
            }
            offset += readCount;
            count += readCount;
            //
            if (length == 0)
            {
                plainData = [];
                return true;
            }
            //
            if (offset + length > byson.Length)
            {
                plainData = null;
                count = 0;
                return false;
            }
            //
            plainData = new byte[length];
            for (int i = 0; i < length; i++)
                plainData[i] = byson[offset + i];
            count += (int)length;
            //
            return true;
        }
        /// <summary>
        /// Try decode byson to data collection.
        /// </summary>
        public static bool TryDecode(byte[]? byson, int offset, [NotNullWhen(true)] out byte[][]? plainData, out int count)
        {
            if (byson == null || byson.Length == 0)
            {
                plainData = null;
                count = 0;
                return false;
            }
            //
            count = 0;
            if (!BysonUInt.TryConvert(byson, offset, out uint capacity, out int readCount) || capacity < 0)
            {
                plainData = null;
                count = 0;
                return false;
            }
            offset += readCount;
            count += readCount;
            //
            plainData = new byte[capacity][];
            int bytesLength = byson.Length,
                index = 0;
            while (index < capacity && offset < bytesLength)
            {
                if (!BysonUInt.TryConvert(byson, offset, out uint length, out readCount) || length < 0)
                {
                    plainData = null;
                    count = 0;
                    return false;
                }
                offset += readCount;
                count += readCount;
                //
                if (length == 0)
                {
                    plainData[index] = [];
                }
                else
                {
                    if (offset + length > bytesLength)
                    {
                        plainData = null;
                        count = 0;
                        return false;
                    }
                    //
                    byte[] tmp = new byte[length];
                    for (int i = 0; i < length; i++)
                        tmp[i] = byson[offset + i];
                    plainData[index] = tmp;
                    offset += (int)length;
                    count += (int)length;
                }
                //
                index++;
            }
            //
            if (index < capacity)
            {
                plainData = null;
                count = 0;
                return false;
            }
            //
            return true;
        }
        #endregion

        #region Encode
        /// <summary>
        /// Encode data to byson.
        /// </summary>
        public static void Encode(byte[]? plainData, out byte[] byson)
        {
            if (plainData == null || plainData.Length == 0)
            {
                byson = BysonUInt.Convert(0);
                return;
            }
            //
            byte[] byLength = BysonUInt.Convert((uint)plainData.Length);
            byson = new byte[byLength.Length + plainData.Length];
            byLength.CopyTo(byson, 0);
            plainData.CopyTo(byson, byLength.Length);
        }
        /// <summary>
        /// Encode data to byson.
        /// </summary>
        public static void Encode(byte[]? plainData, List<byte> byson)
        {
            if (plainData == null || plainData.Length == 0)
            {
                byson.AddRange(BysonUInt.Convert(0));
                return;
            }
            //
            byte[] byLength = BysonUInt.Convert((uint)plainData.Length);
            byson.AddRange(byLength);
            if (plainData.Length > 0)
                byson.AddRange(plainData);
        }
        /// <summary>
        /// Encode data to byson.
        /// </summary>
        public static void Encode(ICollection<byte>? plainData, out byte[] byson)
        {
            if (plainData == null || plainData.Count == 0)
            {
                byson = BysonUInt.Convert(0);
                return;
            }
            //
            byte[] byLength = BysonUInt.Convert((uint)plainData.Count);
            byson = new byte[byLength.Length + plainData.Count];
            byLength.CopyTo(byson, 0);
            plainData.CopyTo(byson, byLength.Length);
        }
        /// <summary>
        /// Encode data to byson.
        /// </summary>
        public static void Encode(ICollection<byte>? plainData, List<byte> byson)
        {
            if (plainData == null || plainData.Count == 0)
            {
                byson.AddRange(BysonUInt.Convert(0));
                return;
            }
            //
            byte[] byLength = BysonUInt.Convert((uint)plainData.Count);
            byson.AddRange(byLength);
            if (plainData.Count > 0)
                byson.AddRange(plainData);
        }
        #endregion

        #region Encode Collection
        /// <summary>
        /// Encode data array to byson.
        /// </summary>
        public static void Encode(byte[][]? plainData, List<byte> byson)
        {
            if (plainData == null || plainData.Length == 0)
            {
                byson.AddRange(BysonUInt.Convert(0));
                return;
            }
            //
            byte[] byCount = BysonUInt.Convert((uint)plainData.Length);
            byson.AddRange(byCount);
            //
            foreach (byte[] data in plainData)
            {
                byte[] byLength = BysonUInt.Convert((uint)data.Length);
                byson.AddRange(byLength);
                if (data.Length > 0)
                    byson.AddRange(data);
            }
        }
        /// <summary>
        /// Encode data collection to byson.
        /// </summary>
        public static void Encode(ICollection<byte[]>? plainData, List<byte> byson)
        {
            if (plainData == null || plainData.Count == 0)
            {
                byson.AddRange(BysonUInt.Convert(0));
                return;
            }
            //
            byte[] byCount = BysonUInt.Convert((uint)plainData.Count);
            byson.AddRange(byCount);
            //
            foreach (byte[] data in plainData)
            {
                byte[] byLength = BysonUInt.Convert((uint)data.Length);
                byson.AddRange(byLength);
                if (data.Length > 0)
                    byson.AddRange(data);
            }
        }
        #endregion
    }
}
