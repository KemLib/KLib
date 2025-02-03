namespace KLib.Byson
{
    public static class BysonUIntConvert
    {
        #region Properties

        #endregion

        #region TryConvert
        public static bool TryConvert(byte[]? bytes, int offset, [NotNullWhen(true)] out byte[]? datas, out int count)
        {
            if (bytes == null || bytes.Length == 0)
            {
                datas = null;
                count = 0;
                return false;
            }
            //
            count = 0;
            if (!BysonUInt.TryConvert(bytes, offset, out uint length, out int readCount) || length < 0)
            {
                datas = null;
                count = 0;
                return false;
            }
            offset += readCount;
            count += readCount;
            //
            if (length == 0)
            {
                datas = [];
                return true;
            }
            //
            if (offset + length > bytes.Length)
            {
                datas = null;
                count = 0;
                return false;
            }
            //
            datas = new byte[length];
            for (int i = 0; i < length; i++)
                datas[i] = bytes[offset + i];
            count += (int)length;
            //
            return true;
        }
        public static bool TryConvert(byte[]? bytes, int offset, [NotNullWhen(true)] out byte[][]? datas, out int count)
        {
            if (bytes == null || bytes.Length == 0)
            {
                datas = null;
                count = 0;
                return false;
            }
            //
            count = 0;
            if (!BysonUInt.TryConvert(bytes, offset, out uint capacity, out int readCount) || capacity < 0)
            {
                datas = null;
                count = 0;
                return false;
            }
            offset += readCount;
            count += readCount;
            //
            datas = new byte[capacity][];
            int bytesLength = bytes.Length,
                index = 0;
            while (index < capacity && offset < bytesLength)
            {
                if (!BysonUInt.TryConvert(bytes, offset, out uint length, out readCount) || length < 0)
                {
                    datas = null;
                    count = 0;
                    return false;
                }
                offset += readCount;
                count += readCount;
                //
                if (length == 0)
                {
                    datas[index] = [];
                }
                else
                {
                    if (offset + length > bytesLength)
                    {
                        datas = null;
                        count = 0;
                        return false;
                    }
                    //
                    byte[] tmp = new byte[length];
                    for (int i = 0; i < length; i++)
                        tmp[i] = bytes[offset + i];
                    datas[index] = tmp;
                    offset += (int)length;
                    count += (int)length;
                }
                //
                index++;
            }
            //
            if (index < capacity)
            {
                datas = null;
                count = 0;
                return false;
            }
            //
            return true;
        }
        #endregion

        #region Convert
        public static void Convert(byte[]? datas, out byte[] bytes)
        {
            if (datas == null || datas.Length == 0)
            {
                bytes = BysonUInt.Convert(0);
                return;
            }
            //
            byte[] byLength = BysonUInt.Convert((uint)datas.Length);
            bytes = new byte[byLength.Length + datas.Length];
            byLength.CopyTo(bytes, 0);
            datas.CopyTo(bytes, byLength.Length);
        }
        public static void Convert(byte[]? datas, List<byte> bytes, out int length)
        {
            if (datas == null || datas.Length == 0)
            {
                bytes.AddRange(BysonUInt.Convert(0));
                length = 1;
                return;
            }
            //
            length = 0;
            byte[] byLength = BysonUInt.Convert((uint)datas.Length);
            bytes.AddRange(byLength);
            if (datas.Length > 0)
            {
                bytes.AddRange(datas);
                length += byLength.Length + datas.Length;
            }
            else
                length += byLength.Length;
        }
        public static void Convert(ICollection<byte>? datas, out byte[] bytes)
        {
            if (datas == null || datas.Count == 0)
            {
                bytes = BysonUInt.Convert(0);
                return;
            }
            //
            byte[] byLength = BysonUInt.Convert((uint)datas.Count);
            bytes = new byte[byLength.Length + datas.Count];
            byLength.CopyTo(bytes, 0);
            datas.CopyTo(bytes, byLength.Length);
        }
        public static void Convert(ICollection<byte>? datas, List<byte> bytes, out int length)
        {
            if (datas == null || datas.Count == 0)
            {
                bytes.AddRange(BysonUInt.Convert(0));
                length = 1;
                return;
            }
            //
            length = 0;
            //
            byte[] byLength = BysonUInt.Convert((uint)datas.Count);
            bytes.AddRange(byLength);
            if (datas.Count > 0)
            {
                bytes.AddRange(datas);
                length += byLength.Length + datas.Count;
            }
            else
                length += byLength.Length;
        }
        #endregion

        #region Convert Collection
        public static void Convert(byte[][]? datas, List<byte> bytes, out int length)
        {
            if (datas == null || datas.Length == 0)
            {
                bytes.AddRange(BysonUInt.Convert(0));
                length = 1;
                return;
            }
            //
            length = 0;
            //
            byte[] byCount = BysonUInt.Convert((uint)datas.Length);
            bytes.AddRange(byCount);
            length += byCount.Length;
            //
            foreach (byte[] data in datas)
            {
                byte[] byLength = BysonUInt.Convert((uint)data.Length);
                bytes.AddRange(byLength);
                if (data.Length > 0)
                {
                    bytes.AddRange(data);
                    length += byLength.Length + data.Length;
                }
                else
                    length += byLength.Length;
            }
        }
        public static void Convert(ICollection<byte[]>? datas, List<byte> bytes, out int length)
        {
            if (datas == null || datas.Count == 0)
            {
                bytes.AddRange(BysonUInt.Convert(0));
                length = 1;
                return;
            }
            //
            length = 0;
            //
            byte[] byCount = BysonUInt.Convert((uint)datas.Count);
            bytes.AddRange(byCount);
            length += byCount.Length;
            //
            foreach (byte[] data in datas)
            {
                byte[] byLength = BysonUInt.Convert((uint)data.Length);
                bytes.AddRange(byLength);
                if (data.Length > 0)
                {
                    bytes.AddRange(data);
                    length += byLength.Length + data.Length;
                }
                else
                    length += byLength.Length;
            }
        }
        #endregion
    }
}
