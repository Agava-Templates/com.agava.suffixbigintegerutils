using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Agava.SuffixBigIntegerUtils
{
    public static class BigIntegerExtention
    {
        private static readonly char[] _simpleSuffixes = new char[] { 'k', 'M', 'B', 'T', 'Q' };
        private static readonly char[] _complexSuffixes;

        static BigIntegerExtention()
        {
            var complexSuffixes = new List<char>(25);
            for (char symbol = 'a'; symbol <= 'z'; symbol++)
                if (symbol != 'k')
                    complexSuffixes.Add(symbol);

            _complexSuffixes = complexSuffixes.ToArray();
        }

        public static string FormatWithSuffix(BigInteger integer)
        {
            var value = integer.ToString();
            if (value.Length <= 4)
                return value;

            var wholePartCount = value.Length % 3;
            if (wholePartCount == 0)
                wholePartCount = 3;

            var result = new StringBuilder();
            for (int i = 0; i < wholePartCount; i++)
                result.Append(value[i]);

            if (value[wholePartCount] != '0' || value[wholePartCount + 1] != '0' || value[wholePartCount + 2] != '0')
            {
                result.Append('.');
                for (int i = wholePartCount; i < wholePartCount + 3; i++)
                    result.Append(value[i]);
            }

            result.Append(GetSuffix(value.Length));
            return result.ToString();
        }

        public static BigInteger ToBigIntegerFromSuffixFormat(this string suffixFormat)
        {
            var index = suffixFormat.IndexOfAny(_simpleSuffixes);
            if (index >= 0)
                return FromSimpleSuffix(suffixFormat.Substring(0, index), suffixFormat.Substring(index));

            index = suffixFormat.IndexOfAny(_complexSuffixes);
            if (index >= 0)
                return FromComplexSuffix(suffixFormat.Substring(0, index), suffixFormat.Substring(index));

            return BigInteger.Parse(suffixFormat);
        }

        private static BigInteger FromSimpleSuffix(string number, string suffix)
        {
            if (suffix.Length > 1)
                throw new FormatException(nameof(suffix) + " has wrong format");

            var additionalExponent = GetExponentAferDot(number);
            number = number.Replace(".", string.Empty);
            BigInteger value = BigInteger.Parse(number);
            var symbol = suffix[0];
            value *= BigInteger.Pow(10, 3 * (Array.IndexOf(_simpleSuffixes, symbol) + 1) - additionalExponent);

            return value;
        }

        private static BigInteger FromComplexSuffix(string number, string suffix)
        {
            var numberBase = _complexSuffixes.Length;
            int power = suffix.Length - 1;
            int result = 0;
            for (int i = 0; i < suffix.Length - 1; i++)
            {
                var suffixNumber = Array.IndexOf(_complexSuffixes, suffix[i]) + 1;
                if (suffixNumber <= 0)
                    throw new FormatException("Wrong suffix format");

                result += suffixNumber * IntPow(numberBase, power - i);
            }

            var lastSymbolNumber = suffix[suffix.Length - 1];
            var lastSymbolSuffixNumber = Array.IndexOf(_complexSuffixes, lastSymbolNumber);
            if (lastSymbolSuffixNumber < 0)
                throw new FormatException("Wrong suffix format");

            result += lastSymbolSuffixNumber;

            var additionalExponent = GetExponentAferDot(number);
            var numberOfZeros = result * 3 + (_simpleSuffixes.Length + 1) * 3 - additionalExponent;

            number = number.Replace(".", string.Empty);
            return BigInteger.Parse(number) * BigInteger.Pow(10, numberOfZeros);
        }

        private static string GetSuffix(int numberLength)
        {
            var zeroCount = numberLength - 1;
            var orderCount = zeroCount / 3;
            if (orderCount <= _simpleSuffixes.Length)
                return _simpleSuffixes[orderCount - 1].ToString();

            var result = new StringBuilder();
            orderCount -= _simpleSuffixes.Length;
            while (orderCount > 0)
            {
                if (orderCount == 1)
                {
                    result.Append(_complexSuffixes[0]);
                    break;
                }

                orderCount--;
                var modulo = orderCount % _complexSuffixes.Length;
                result.Append(_complexSuffixes[modulo]);
                orderCount = orderCount / _complexSuffixes.Length;
            }

            return ReverseStringBuilder(result.ToString());
        }

        private static int IntPow(int number, int pow)
        {
            if (pow < 0)
                throw new InvalidOperationException("Negative numbers not support by this method");
            if (pow == 0)
                return 1;

            int result = 1;
            for (int i = 0; i < pow; i++)
                result *= number;

            return result;
        }

        private static int GetExponentAferDot(string number)
        {
            int dotIndex = -1;
            for (int i = 0; i < number.Length; i++)
            {
                if (number[i] == '.')
                {
                    if (dotIndex >= 0)
                        throw new FormatException("Number has multiply dots");

                    dotIndex = i;
                }
            }

            if (dotIndex == -1)
                return 0;

            return number.Length - dotIndex - 1;
        }

        private static string ReverseStringBuilder(string str)
        {
            StringBuilder sb = new StringBuilder(str.Length);
            for (int i = str.Length - 1; i >= 0; i--)
                sb.Append(str[i]);

            return sb.ToString();
        }
    }
}