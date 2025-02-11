/*
 * Copyright (c) 2013 Calvin Rien
 *
 * Based on the JSON parser by Patrick van Bergen
 * http://techblog.procurios.nl/k/618/news/view/14605/14863/How-do-I-write-my-own-parser-for-JSON.html
 *
 * Simplified it so that it doesn't throw exceptions
 * and can be used in Unity iPhone with maximum code stripping.
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.IO;

namespace KLibStandard.Json
{
    public static class MiniJson
    {
        #region Properties
        private const string STRING_NULL = "null",
            STRING_BOOL_TRUE = "true",
            STRING_BOOL_FALSE = "false",
            STRING_R = "R",
            STRING_X4 = "x4",
            STRING_WORD_BREAK = "{}[],:\"";
        private const string STRING_ARRAY_EMPTY = "[]",
            STRING_DICTINARY_EMPTY = "{}";
        private const char CHAR_ARRAY_OPEN = '[',
            CHAR_ARRAY_CLOSE = ']',
            CHAR_DICTINARY_OPEN = '{',
            CHAR_DICTINARY_CLOSE = '}';
        private const char CHAR_0 = ':',
            CHAR_1 = ',',
            CHAR_2 = ' ',
            CHAR_3 = '/',
            CHAR_4 = 'b',
            CHAR_5 = 'f',
            CHAR_6 = 'n',
            CHAR_7 = 'r',
            CHAR_8 = 't',
            CHAR_9 = 'u',
            CHAR_10 = '_';
        private const char CHAR_NUMBER_0 = '0',
            CHAR_NUMBER_1 = '1',
            CHAR_NUMBER_2 = '2',
            CHAR_NUMBER_3 = '3',
            CHAR_NUMBER_4 = '4',
            CHAR_NUMBER_5 = '5',
            CHAR_NUMBER_6 = '6',
            CHAR_NUMBER_7 = '7',
            CHAR_NUMBER_8 = '8',
            CHAR_NUMBER_9 = '9';
        private const string STRING_SPECIAL_CHAR_1 = "\\\"",
            STRING_SPECIAL_CHAR_2 = "\\\\",
            STRING_SPECIAL_CHAR_3 = "\\b",
            STRING_SPECIAL_CHAR_4 = "\\f",
            STRING_SPECIAL_CHAR_5 = "\\n",
            STRING_SPECIAL_CHAR_6 = "\\r",
            STRING_SPECIAL_CHAR_7 = "\\t",
            STRING_SPECIAL_CHAR_8 = "\\u";
        private const char SPECIAL_CHAR_0 = '\"',
            SPECIAL_CHAR_1 = '"',
            SPECIAL_CHAR_2 = '\\',
            SPECIAL_CHAR_3 = '\b',
            SPECIAL_CHAR_4 = '\f',
            SPECIAL_CHAR_5 = '\n',
            SPECIAL_CHAR_6 = '\r',
            SPECIAL_CHAR_7 = '\t';

        public const string DEFAULT_JSON = STRING_DICTINARY_EMPTY;
        public const bool DEFAULT_PRETTY = false;
        public const string DEFAULT_INDENT_TEXT = "  ";
        #endregion

        #region Method
        /// <summary>
        /// Parses the string json into a value.
        /// </summary>
        /// <param name="json">A JSON string.</param>
        /// <returns>An List&lt;object&gt;, a Dictionary&lt;string, object&gt;, a double, an integer,a string, null, true, or false</returns>
        public static object Deserialize(string json)
        {
            // save the string for debug information
            if (string.IsNullOrEmpty(json))
                return null;
            //
            object value;
            using (StringReader reader = new StringReader(json))
            {
                value = ParseValue(reader);
            }
            return value;
        }
        /// <summary>
        /// Parses the string json into a value.
        /// </summary>
        /// <param name="json">A JSON string.</param>
        /// <returns>An List&lt;object&gt;, a Dictionary&lt;string, object&gt;, a double, an integer,a string, null, true, or false</returns>
        public static bool TryDeserialize(string json, [NotNullWhen(true)] out object value)
        {
            // save the string for debug information
            if (string.IsNullOrEmpty(json))
            {
                value = null;
                return false;
            }
            //
            using (StringReader reader = new StringReader(json))
            {
                value = ParseValue(reader);
            }
            return value != null;
        }

        /// <summary>
        /// Converts a IDictionary / IList object or a simple type (string, int, etc.) into a JSON string.
        /// </summary>
        /// <param name="json">A Dictionary&lt;string, object&gt; / List&lt;object&gt;</param>
        /// <param name="pretty">A boolean to indicate whether or not JSON should be prettified, default is false.</param>
        /// <param name="indentText">A string to ibe used as indentText, default is 2 spaces.</param>
        /// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
        public static string Serialize(object obj, bool pretty = DEFAULT_PRETTY, string indentText = DEFAULT_INDENT_TEXT)
        {
            if (obj == null)
                return DEFAULT_JSON;
            //
            StringBuilder builder = new StringBuilder();
            SerializeValue(builder, pretty, indentText, obj, 0);
            return builder.ToString();
        }
        #endregion
        #region Deserialize
        private static Dictionary<string, object> ParseObject(StringReader reader)
        {
            Dictionary<string, object> table = new Dictionary<string, object>();

            // ditch opening brace
            reader.Read();

            // {
            while (true)
            {
                switch (NextToken(reader))
                {
                    case TOKEN.NONE:
                        return null;
                    case TOKEN.COMMA:
                        continue;
                    case TOKEN.CURLY_CLOSE:
                        return table;
                    default:
                        // name
                        string name = ParseString(reader);
                        if (name == null)
                        {
                            return null;
                        }

                        // :
                        if (NextToken(reader) != TOKEN.COLON)
                        {
                            return null;
                        }
                        // ditch the colon
                        reader.Read();

                        // value
                        table[name] = ParseValue(reader);
                        break;
                }
            }
        }
        private static List<object> ParseArray(StringReader reader)
        {
            List<object> array = new List<object>();

            // ditch opening bracket
            reader.Read();

            // [
            var parsing = true;
            while (parsing)
            {
                TOKEN nextToken = NextToken(reader);

                switch (nextToken)
                {
                    case TOKEN.NONE:
                        return null;
                    case TOKEN.COMMA:
                        continue;
                    case TOKEN.SQUARED_CLOSE:
                        parsing = false;
                        break;
                    default:
                        object value = ParseByToken(reader, nextToken);

                        array.Add(value);
                        break;
                }
            }

            return array;
        }
        private static object ParseValue(StringReader reader)
        {
            TOKEN nextToken = NextToken(reader);
            return ParseByToken(reader, nextToken);
        }
        private static object ParseByToken(StringReader reader, TOKEN token)
        {
            return token switch
            {
                TOKEN.STRING => ParseString(reader),
                TOKEN.NUMBER => ParseNumber(reader),
                TOKEN.CURLY_OPEN => ParseObject(reader),
                TOKEN.SQUARED_OPEN => ParseArray(reader),
                TOKEN.TRUE => true,
                TOKEN.FALSE => false,
                TOKEN.NULL => null,
                _ => null,
            };
        }
        private static string ParseString(StringReader reader)
        {
            StringBuilder s = new StringBuilder();
            char c;

            // ditch opening quote
            reader.Read();

            bool parsing = true;
            while (parsing)
            {
                if (reader.Peek() == -1)
                {
                    break;
                }
                //
                c = NextChar(reader);
                switch (c)
                {
                    case SPECIAL_CHAR_1:
                        parsing = false;
                        break;
                    case SPECIAL_CHAR_2:
                        if (reader.Peek() == -1)
                        {
                            parsing = false;
                            break;
                        }

                        c = NextChar(reader);
                        switch (c)
                        {
                            case SPECIAL_CHAR_1:
                            case SPECIAL_CHAR_2:
                            case CHAR_3:
                                s.Append(c);
                                break;
                            case CHAR_4:
                                s.Append(SPECIAL_CHAR_3);
                                break;
                            case CHAR_5:
                                s.Append(SPECIAL_CHAR_4);
                                break;
                            case CHAR_6:
                                s.Append(SPECIAL_CHAR_5);
                                break;
                            case CHAR_7:
                                s.Append(SPECIAL_CHAR_6);
                                break;
                            case CHAR_8:
                                s.Append(SPECIAL_CHAR_7);
                                break;
                            case CHAR_9:
                                var hex = new char[4];

                                for (int i = 0; i < 4; i++)
                                {
                                    hex[i] = NextChar(reader);
                                }

                                s.Append((char)Convert.ToInt32(new string(hex), 16));
                                break;
                        }
                        break;
                    default:
                        s.Append(c);
                        break;
                }
            }

            return s.ToString();
        }
        private static object ParseNumber(StringReader reader)
        {
            string number = NextWord(reader);

            if (Int64.TryParse(number, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var parsedInt))
            {
                return parsedInt;
            }

            Double.TryParse(number, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var parsedDouble);
            return parsedDouble;
        }
        private static void EatWhitespace(StringReader reader)
        {
            while (Char.IsWhiteSpace(PeekChar(reader)))
            {
                reader.Read();

                if (reader.Peek() == -1)
                {
                    break;
                }
            }
        }
        private static char PeekChar(StringReader reader)
        {
            return Convert.ToChar(reader.Peek());
        }
        private static char NextChar(StringReader reader)
        {
            return Convert.ToChar(reader.Read());
        }
        private static string NextWord(StringReader reader)
        {
            StringBuilder word = new StringBuilder();

            while (!IsWordBreak(PeekChar(reader)))
            {
                word.Append(NextChar(reader));

                if (reader.Peek() == -1)
                {
                    break;
                }
            }

            return word.ToString();
        }
        private static TOKEN NextToken(StringReader reader)
        {
            EatWhitespace(reader);

            if (reader.Peek() == -1)
            {
                return TOKEN.NONE;
            }

            switch (PeekChar(reader))
            {
                case CHAR_DICTINARY_OPEN:
                    return TOKEN.CURLY_OPEN;
                case CHAR_DICTINARY_CLOSE:
                    reader.Read();
                    return TOKEN.CURLY_CLOSE;
                case CHAR_ARRAY_OPEN:
                    return TOKEN.SQUARED_OPEN;
                case CHAR_ARRAY_CLOSE:
                    reader.Read();
                    return TOKEN.SQUARED_CLOSE;
                case CHAR_1:
                    reader.Read();
                    return TOKEN.COMMA;
                case SPECIAL_CHAR_1:
                    return TOKEN.STRING;
                case CHAR_0:
                    return TOKEN.COLON;
                case CHAR_NUMBER_0:
                case CHAR_NUMBER_1:
                case CHAR_NUMBER_2:
                case CHAR_NUMBER_3:
                case CHAR_NUMBER_4:
                case CHAR_NUMBER_5:
                case CHAR_NUMBER_6:
                case CHAR_NUMBER_7:
                case CHAR_NUMBER_8:
                case CHAR_NUMBER_9:
                case CHAR_10:
                    return TOKEN.NUMBER;
            }

            return NextWord(reader) switch
            {
                STRING_BOOL_FALSE => TOKEN.FALSE,
                STRING_BOOL_TRUE => TOKEN.TRUE,
                STRING_NULL => TOKEN.NULL,
                _ => TOKEN.NONE,
            };
        }
        private static bool IsWordBreak(char c)
        {
            return Char.IsWhiteSpace(c) || STRING_WORD_BREAK.Contains(c);
        }
        #endregion

        #region Serializer
        private static void SerializeValue(StringBuilder builder, bool pretty, string indentText, object value, int indent)
        {

            if (value == null || value is Delegate)
            {
                builder.Append(STRING_NULL);
                return;
            }
            if (value is string asStr)
            {
                SerializeString(builder, asStr);
                return;
            }
            if (value is bool asBool)
            {
                builder.Append(asBool ? STRING_BOOL_TRUE : STRING_BOOL_FALSE);
                return;
            }
            if (value is IList asList)
            {
                SerializeArray(builder, pretty, indentText, asList, indent);
                return;
            }
            if (value is IDictionary asDict)
            {
                SerializeObject(builder, pretty, indentText, asDict, indent);
                return;
            }
            if (value is char asChar)
            {
                SerializeString(builder, new string(asChar, 1));
                return;
            }
            //
            SerializeOther(builder, pretty, indentText, value, indent);
        }
        private static void SerializeObject(StringBuilder builder, bool pretty, string indentText, IDictionary obj, int indent)
        {
            if (pretty && obj.Keys.Count == 0)
            {
                builder.Append(STRING_DICTINARY_EMPTY);
                return;
            }

            bool first = true;
            string indentLine = null;

            builder.Append(CHAR_DICTINARY_OPEN);
            if (pretty)
            {
                builder.Append(SPECIAL_CHAR_5);
                indentLine = string.Concat(Enumerable.Repeat(indentText, indent).ToArray());
            }

            foreach (object key in obj.Keys)
            {
                string strKey = key.ToString();
                if (strKey == null)
                    continue;
                //
                if (!first)
                {
                    builder.Append(CHAR_1);
                    if (pretty)
                        builder.Append(SPECIAL_CHAR_5);
                }

                if (pretty)
                {
                    builder.Append(indentLine);
                    builder.Append(indentText);
                }

                SerializeString(builder, strKey);
                builder.Append(CHAR_0);
                if (pretty)
                    builder.Append(CHAR_2);

                SerializeValue(builder, pretty, indentText, obj[key], indent + 1);

                first = false;
            }

            if (pretty)
            {
                builder.Append(SPECIAL_CHAR_5);
                builder.Append(indentLine);
            }
            builder.Append(CHAR_DICTINARY_CLOSE);
        }
        private static void SerializeArray(StringBuilder builder, bool pretty, string indentText, IList anArray, int indent)
        {
            if (pretty && anArray.Count == 0)
            {
                builder.Append(STRING_ARRAY_EMPTY);
                return;
            }

            bool first = true;
            string indentLine = null;

            builder.Append(CHAR_ARRAY_OPEN);
            if (pretty)
            {
                builder.Append(SPECIAL_CHAR_5);
                indentLine = string.Concat(Enumerable.Repeat(indentText, indent).ToArray());
            }

            foreach (object obj in anArray)
            {
                if (!first)
                {
                    builder.Append(CHAR_1);
                    if (pretty)
                        builder.Append(SPECIAL_CHAR_5);
                }

                if (pretty)
                {
                    builder.Append(indentLine);
                    builder.Append(indentText);
                }

                SerializeValue(builder, pretty, indentText, obj, indent + 1);

                first = false;
            }

            if (pretty)
            {
                builder.Append(SPECIAL_CHAR_5);
                builder.Append(indentLine);
            }
            builder.Append(CHAR_ARRAY_CLOSE);
        }
        private static void SerializeString(StringBuilder builder, string str)
        {
            builder.Append(SPECIAL_CHAR_0);
            char[] charArray = str.ToCharArray();
            foreach (var c in charArray)
            {
                switch (c)
                {
                    case SPECIAL_CHAR_1:
                        builder.Append(STRING_SPECIAL_CHAR_1);
                        break;
                    case SPECIAL_CHAR_2:
                        builder.Append(STRING_SPECIAL_CHAR_2);
                        break;
                    case SPECIAL_CHAR_3:
                        builder.Append(STRING_SPECIAL_CHAR_3);
                        break;
                    case SPECIAL_CHAR_4:
                        builder.Append(STRING_SPECIAL_CHAR_4);
                        break;
                    case SPECIAL_CHAR_5:
                        builder.Append(STRING_SPECIAL_CHAR_5);
                        break;
                    case SPECIAL_CHAR_6:
                        builder.Append(STRING_SPECIAL_CHAR_6);
                        break;
                    case SPECIAL_CHAR_7:
                        builder.Append(STRING_SPECIAL_CHAR_7);
                        break;
                    default:
                        int codepoint = Convert.ToInt32(c);
                        if ((codepoint >= 32) && (codepoint <= 126))
                        {
                            builder.Append(c);
                        }
                        else
                        {
                            builder.Append(STRING_SPECIAL_CHAR_8);
                            builder.Append(codepoint.ToString(STRING_X4));
                        }
                        break;
                }
            }

            builder.Append(SPECIAL_CHAR_0);
        }
        private static void SerializeOther(StringBuilder builder, bool pretty, string indentText, object value, int indent)
        {
            // NOTE: decimals lose precision during serialization.
            // They always have, I'm just letting you know.
            // Previously floats and doubles lost precision too.
            if (value is float asFloat)
            {
                builder.Append((asFloat.ToString(STRING_R, System.Globalization.CultureInfo.InvariantCulture)));
            }
            else if (value is int || value is uint || value is long || value is sbyte || value is byte || value is short || value is ushort || value is ulong)
            {
                builder.Append(value);
            }
            else if (value is double || value is decimal)
            {
                builder.Append(Convert.ToDouble(value).ToString(STRING_R, System.Globalization.CultureInfo.InvariantCulture));
            }
            else
            {
                Dictionary<string, object> map = new Dictionary<string, object>();
                List<FieldInfo> fields = new List<FieldInfo>(value.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public));
                foreach (FieldInfo field in fields)
                    map.Add(field.Name, field.GetValue(value));
                List<PropertyInfo> properties = new List<PropertyInfo>(value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public));
                foreach (PropertyInfo property in properties)
                    map.Add(property.Name, property.GetValue(value, null));
                SerializeObject(builder, pretty, indentText, map, indent);
            }
        }

        #endregion
    }
}
