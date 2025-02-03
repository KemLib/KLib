namespace KLib.Byson
{
    public static class BysonUInt
    {
        #region properties
        private const byte
            NUMBER_LENGTH_1 = 0b_0000_0000,
            NUMBER_LENGTH_2 = 0b_1000_0000,
            NUMBER_LENGTH_3 = 0b_1100_0000,
            NUMBER_LENGTH_4 = 0b_1110_0000,
            NUMBER_LENGTH_5 = 0b_1111_0000;
        private const int COUNT_NUMBER_LENGTH_1 = NUMBER_LENGTH_2 - NUMBER_LENGTH_1,
            COUNT_NUMBER_LENGTH_2 = NUMBER_LENGTH_3 - NUMBER_LENGTH_2,
            COUNT_NUMBER_LENGTH_3 = NUMBER_LENGTH_4 - NUMBER_LENGTH_3,
            COUNT_NUMBER_LENGTH_4 = NUMBER_LENGTH_5 - NUMBER_LENGTH_4;
        private const uint Power_0 = 0,
            Power_1 = 256,
            Power_2 = Power_1 * Power_1,
            Power_3 = Power_2 * Power_1;
        public const int LENGTH_MIN = BysonInt.LENGTH_MIN,
            LENGTH_MAX = BysonInt.LENGTH_MAX;
        #endregion

        #region Method
        public static bool TryGetCount(byte[]? data, int offset, out int count)
        {
            if (data == null || offset < 0 || offset >= data.Length)
            {
                count = 0;
                return false;
            }
            //
            byte head = data[offset];
            if (head > NUMBER_LENGTH_5)
            {
                count = 0;
                return false;
            }
            //
            if (head < NUMBER_LENGTH_2)
                count = 1;
            else if (head < NUMBER_LENGTH_3)
                count = 2;
            else if (head < NUMBER_LENGTH_4)
                count = 3;
            else if (head < NUMBER_LENGTH_5)
                count = 4;
            else
                count = 5;
            //
            return true;
        }
        public static bool TryConvert(byte[]? data, int offset, out uint value, out int count)
        {
            if (data == null || offset < 0 || offset >= data.Length)
            {
                count = 0;
                value = 0;
                return false;
            }
            //
            byte head = data[offset];
            uint valueHead;
            if (head > NUMBER_LENGTH_5)
            {
                count = 0;
                value = 0;
                return false;
            }
            //
            if (head < NUMBER_LENGTH_2)
            {
                count = 1;
                valueHead = (uint)(head - NUMBER_LENGTH_1);
            }
            else if (head < NUMBER_LENGTH_3)
            {
                count = 2;
                valueHead = (uint)(head - NUMBER_LENGTH_2);
            }
            else if (head < NUMBER_LENGTH_4)
            {
                count = 3;
                valueHead = (uint)(head - NUMBER_LENGTH_3);
            }
            else if (head < NUMBER_LENGTH_5)
            {
                count = 4;
                valueHead = (uint)(head - NUMBER_LENGTH_4);
            }
            else
            {
                count = 5;
                valueHead = 0;
            }
            //
            if (offset + count > data.Length)
            {
                count = 0;
                value = 0;
                return false;
            }
            //
            switch (count)
            {
                case 1:
                    value = valueHead;
                    break;
                case 2:
                    value = valueHead * Power_1 + data[offset + 1];
                    break;
                case 3:
                    value = valueHead * Power_2 + data[offset + 1] * Power_1 + data[offset + 2];
                    break;
                case 4:
                    value = valueHead * Power_3 + data[offset + 1] * Power_2 + data[offset + 2] * Power_1 + data[offset + 3];
                    break;
                case 5:
                    value = valueHead * Power_0 + data[offset + 1] * Power_3 + data[offset + 2] * Power_2 + data[offset + 3] * Power_1 + data[offset + 4];
                    break;
                default:
                    count = 0;
                    value = 0;
                    return false;
            }
            return true;
        }
        public static byte[] Convert(uint value)
        {
            if (value == 0)
                return [NUMBER_LENGTH_1];
            //
            int count = 0;
            byte[] buffer = new byte[5];
            while (value > 0)
            {
                buffer[count] = (byte)(value % 256);
                value /= 256;
                count++;
            }
            //
            int lastIndex = count - 1;
            switch (count)
            {
                case 1:
                    if (buffer[lastIndex] < COUNT_NUMBER_LENGTH_1)
                    {
                        buffer[lastIndex] += NUMBER_LENGTH_1;
                    }
                    else
                    {
                        buffer[count] = NUMBER_LENGTH_2;
                        lastIndex = count;
                        count++;
                    }
                    break;
                case 2:
                    if (buffer[lastIndex] < COUNT_NUMBER_LENGTH_2)
                    {
                        buffer[lastIndex] += NUMBER_LENGTH_2;
                    }
                    else
                    {
                        buffer[count] = NUMBER_LENGTH_3;
                        lastIndex = count;
                        count++;
                    }
                    break;
                case 3:
                    if (buffer[lastIndex] < COUNT_NUMBER_LENGTH_3)
                    {
                        buffer[lastIndex] += NUMBER_LENGTH_3;
                    }
                    else
                    {
                        buffer[count] = NUMBER_LENGTH_4;
                        lastIndex = count;
                        count++;
                    }
                    break;
                case 4:
                    if (buffer[lastIndex] < COUNT_NUMBER_LENGTH_4)
                    {
                        buffer[lastIndex] += NUMBER_LENGTH_4;
                    }
                    else
                    {
                        buffer[count] = NUMBER_LENGTH_5;
                        lastIndex = count;
                        count++;
                    }
                    break;
            }
            //
            byte[] data = new byte[count];
            for (int i = 0; i < count; i++)
                data[i] = buffer[lastIndex - i];
            return data;
        }
        #endregion
    }
}
