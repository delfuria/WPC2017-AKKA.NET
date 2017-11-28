using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AKKA.Library.Demo
{
    public static class StringExtensions
    {
        public static string Repeat(this char chatToRepeat, int repeat)
        {
            return new string(chatToRepeat, repeat);
        }

        public static string Repeat(this string stringToRepeat, int repeat)
        {
            var builder = new StringBuilder(repeat * stringToRepeat.Length);
            for (int i = 0; i < repeat; i++)
            {
                builder.Append(stringToRepeat);
            }
            return builder.ToString();
        }

        public static string Right(this string value, int length)
        {
            return value.Substring(value.Length - length);
        }

        public static string Left(this string value, int length)
        {
            return value.Substring(0, length);
        }

        public static string Chr(this int value)
        {
            return value.ToStr();
        }

        public static string Chr(this char value)
        {
            return value.ToStr();
        }

        public static IEnumerable<string> Split(this string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        public static string ToStr(this int num)
        {
            return ((char)num).ToStr();
        }

        public static string ToStr(this char ch)
        {
            return ch.ToString();
        }

        public static bool In(this char ch, string str)
        {
            return str.Contains(ch.ToStr());
        }

        public static string CapitalizeWords(this string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (value.Length == 0)
                return value;

            StringBuilder result = new StringBuilder(value);
            result[0] = char.ToUpper(result[0]);
            for (int i = 1; i < result.Length; ++i)
            {
                if (char.IsWhiteSpace(result[i - 1]))
                    result[i] = char.ToUpper(result[i]);
                else
                    result[i] = char.ToLower(result[i]);
            }
            return result.ToString();
        }

        public static string CamelCaseStringSplitBySpace(this string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (value.Length == 0)
                return value;
            string res = value[0].ToString(CultureInfo.InvariantCulture);
            foreach (char c in value.Skip(1))
            {
                if (char.IsUpper(c))
                {
                    res += " ";
                    res += c;
                }
                else
                    res += c;
            }

            return res;
        }

        public static string TrimBlank(this string value)
        {
            string ret = null;
            if (!string.IsNullOrEmpty(value))
                ret = value.Trim();
            return ret;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static byte[] ToAsciiEncoding(this string value)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] newMsg = encoding.GetBytes(value);
            return newMsg;
        }

        public static string FromAsciiEncoding(this byte[] value)
        {
            return Encoding.ASCII.GetString(value);
        }

        public static string FromUTF8Encoding(this byte[] value)
        {
            return Encoding.UTF8.GetString(value);
        }

        public static byte[] ToUTF8Encoding(this string value)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] newMsg = encoding.GetBytes(value);
            return newMsg;
        }

        public static string EncodeToBase64(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string DecodeFromBase64(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string TakeAndPad(this string str, int padTo)
        {
            string ret = str;
            ret = !String.IsNullOrEmpty(str) ? String.Join("", str.Take(padTo)).PadRight(padTo, ' ') : "".PadRight(padTo, ' ');
            return ret;
        }
    }

    //public static class BlockingCollectionEx<WCFMessageBase>
    //{
    //    public static IEnumerable<WCFMessageBase> GetConsumingEnumerable(this BlockingCollection<WCFMessageBase> coll)
    //    {
    //        return coll.GetConsumingEnumerable(CancellationToken.None);
    //    }

    //    [__DynamicallyInvokable]
    //    public IEnumerable<T> GetConsumingEnumerable(CancellationToken cancellationToken)
    //    {
    //        CancellationTokenSource combinedTokenSource = null;
    //        combinedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken,
    //                                                                              this.m_ConsumersCancellationTokenSource
    //                                                                                  .Token);
    //        while (!this.IsCompleted)
    //        {
    //            T iteratorVariable1;
    //            if (this.TryTakeWithNoTimeValidation(out iteratorVariable1, -1, cancellationToken, combinedTokenSource))
    //            {
    //                yield return iteratorVariable1;
    //            }
    //        }
    //    }
    //}
}