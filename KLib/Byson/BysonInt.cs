namespace KLib.Byson
{
    public static class BysonInt
    {
        #region properties
        private const byte
            NEGATIVE_NUMBER_LENGTH_1 = 0b_0000_0000,
            NEGATIVE_NUMBER_LENGTH_2 = 0b_0100_0000,
            NEGATIVE_NUMBER_LENGTH_3 = 0b_0110_0000,
            NEGATIVE_NUMBER_LENGTH_4 = 0b_0111_0000,
            NEGATIVE_NUMBER_LENGTH_5 = 0b_0111_1000,
            POSITIVE_NUMBER_LENGTH_1 = 0b_1000_0000,
            POSITIVE_NUMBER_LENGTH_2 = 0b_1100_0000,
            POSITIVE_NUMBER_LENGTH_3 = 0b_1110_0000,
            POSITIVE_NUMBER_LENGTH_4 = 0b_1111_0000,
            POSITIVE_NUMBER_LENGTH_5 = 0b_1111_1000;
        private const int COUNT_NUMBER_LENGTH_1 = NEGATIVE_NUMBER_LENGTH_2 - NEGATIVE_NUMBER_LENGTH_1,
            COUNT_NUMBER_LENGTH_2 = NEGATIVE_NUMBER_LENGTH_3 - NEGATIVE_NUMBER_LENGTH_2,
            COUNT_NUMBER_LENGTH_3 = NEGATIVE_NUMBER_LENGTH_4 - NEGATIVE_NUMBER_LENGTH_3,
            COUNT_NUMBER_LENGTH_4 = NEGATIVE_NUMBER_LENGTH_5 - NEGATIVE_NUMBER_LENGTH_4;
        private const int Power_0 = 0,
            Power_1 = 256,
            Power_2 = Power_1 * Power_1,
            Power_3 = Power_2 * Power_1;
        /// <summary>
        /// minimum byte count.
        /// </summary>
        public const int LENGTH_MIN = 1;
        /// <summary>
        /// maximum byte count.
        /// </summary>
        public const int LENGTH_MAX = 5;
        #endregion

        #region Method
        /// <summary>
        /// Try get required number byte in array.
        /// </summary>
        public static bool TryGetCount(byte[]? data, int offset, out int count)
        {
            if (data == null || offset < 0 || offset >= data.Length)
            {
                count = 0;
                return false;
            }
            //
            byte head = data[offset];
            if (head < POSITIVE_NUMBER_LENGTH_1)
            {
                if (head > NEGATIVE_NUMBER_LENGTH_5)
                {
                    count = 0;
                    return false;
                }
                //
                if (head < NEGATIVE_NUMBER_LENGTH_2)
                    count = 1;
                else if (head < NEGATIVE_NUMBER_LENGTH_3)
                    count = 2;
                else if (head < NEGATIVE_NUMBER_LENGTH_4)
                    count = 3;
                else if (head < NEGATIVE_NUMBER_LENGTH_5)
                    count = 4;
                else
                    count = 5;
            }
            else
            {
                if (head > POSITIVE_NUMBER_LENGTH_5)
                {
                    count = 0;
                    return false;
                }
                //
                if (head < POSITIVE_NUMBER_LENGTH_2)
                    count = 1;
                else if (head < POSITIVE_NUMBER_LENGTH_3)
                    count = 2;
                else if (head < POSITIVE_NUMBER_LENGTH_4)
                    count = 3;
                else if (head < POSITIVE_NUMBER_LENGTH_5)
                    count = 4;
                else
                    count = 5;
            }
            return true;
        }
        /// <summary>
        /// Try convert byte array to int.
        /// </summary>
        public static bool TryConvert(byte[]? data, int offset, out int value, out int count)
        {
            if (data == null || offset < 0 || offset >= data.Length)
            {
                count = 0;
                value = 0;
                return false;
            }
            //
            byte head = data[offset];
            bool direct;
            int valueHead;
            if (head < POSITIVE_NUMBER_LENGTH_1)
            {
                if (head > NEGATIVE_NUMBER_LENGTH_5)
                {
                    count = 0;
                    value = 0;
                    return false;
                }
                //
                direct = false;
                if (head < NEGATIVE_NUMBER_LENGTH_2)
                {
                    count = 1;
                    valueHead = head - NEGATIVE_NUMBER_LENGTH_1;
                }
                else if (head < NEGATIVE_NUMBER_LENGTH_3)
                {
                    count = 2;
                    valueHead = head - NEGATIVE_NUMBER_LENGTH_2;
                }
                else if (head < NEGATIVE_NUMBER_LENGTH_4)
                {
                    count = 3;
                    valueHead = head - NEGATIVE_NUMBER_LENGTH_3;
                }
                else if (head < NEGATIVE_NUMBER_LENGTH_5)
                {
                    count = 4;
                    valueHead = head - NEGATIVE_NUMBER_LENGTH_4;
                }
                else
                {
                    count = 5;
                    valueHead = 0;
                }
            }
            else
            {
                if (head > POSITIVE_NUMBER_LENGTH_5)
                {
                    count = 0;
                    value = 0;
                    return false;
                }
                //
                direct = true;
                if (head < POSITIVE_NUMBER_LENGTH_2)
                {
                    count = 1;
                    valueHead = head - POSITIVE_NUMBER_LENGTH_1;
                }
                else if (head < POSITIVE_NUMBER_LENGTH_3)
                {
                    count = 2;
                    valueHead = head - POSITIVE_NUMBER_LENGTH_2;
                }
                else if (head < POSITIVE_NUMBER_LENGTH_4)
                {
                    count = 3;
                    valueHead = head - POSITIVE_NUMBER_LENGTH_3;
                }
                else if (head < POSITIVE_NUMBER_LENGTH_5)
                {
                    count = 4;
                    valueHead = head - POSITIVE_NUMBER_LENGTH_4;
                }
                else
                {
                    count = 5;
                    valueHead = 0;
                }
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
                    if (!direct && data[offset + 1] == 255 && data[offset + 2] == 255 && data[offset + 3] == 255 && data[offset + 4] == 255)
                    {
                        value = int.MinValue;
                        return true;
                    }
                    value = valueHead * Power_0 + data[offset + 1] * Power_3 + data[offset + 2] * Power_2 + data[offset + 3] * Power_1 + data[offset + 4];
                    break;
                default:
                    count = 0;
                    value = 0;
                    return false;
            }
            if (!direct)
                value = -value;
            return true;
        }
        /// <summary>
        /// Convert int to byte array.
        /// </summary>
        public static byte[] Convert(int value)
        {
            if (value == int.MinValue)
                return [NEGATIVE_NUMBER_LENGTH_5, 255, 255, 255, 255];
            else if (value == 0)
                return [NEGATIVE_NUMBER_LENGTH_1];
            //
            bool direct;
            if (value < 0)
            {
                direct = false;
                value = -value;
            }
            else
                direct = true;
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
                        buffer[lastIndex] += direct ? POSITIVE_NUMBER_LENGTH_1 : NEGATIVE_NUMBER_LENGTH_1;
                    }
                    else
                    {
                        buffer[count] = direct ? POSITIVE_NUMBER_LENGTH_2 : NEGATIVE_NUMBER_LENGTH_2;
                        lastIndex = count;
                        count++;
                    }
                    break;
                case 2:
                    if (buffer[lastIndex] < COUNT_NUMBER_LENGTH_2)
                    {
                        buffer[lastIndex] += direct ? POSITIVE_NUMBER_LENGTH_2 : NEGATIVE_NUMBER_LENGTH_2;
                    }
                    else
                    {
                        buffer[count] = direct ? POSITIVE_NUMBER_LENGTH_3 : NEGATIVE_NUMBER_LENGTH_3;
                        lastIndex = count;
                        count++;
                    }
                    break;
                case 3:
                    if (buffer[lastIndex] < COUNT_NUMBER_LENGTH_3)
                    {
                        buffer[lastIndex] += direct ? POSITIVE_NUMBER_LENGTH_3 : NEGATIVE_NUMBER_LENGTH_3;
                    }
                    else
                    {
                        buffer[count] = direct ? POSITIVE_NUMBER_LENGTH_4 : NEGATIVE_NUMBER_LENGTH_4;
                        lastIndex = count;
                        count++;
                    }
                    break;
                case 4:
                    if (buffer[lastIndex] < COUNT_NUMBER_LENGTH_4)
                    {
                        buffer[lastIndex] += direct ? POSITIVE_NUMBER_LENGTH_4 : NEGATIVE_NUMBER_LENGTH_4;
                    }
                    else
                    {
                        buffer[count] = direct ? POSITIVE_NUMBER_LENGTH_5 : NEGATIVE_NUMBER_LENGTH_5;
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
