namespace Project.Extensions.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Xóa bổ tiền tố chuỗi
        /// </summary>
        /// <param name="str"></param>
        /// <param name="preFixes"></param>
        /// <returns></returns>
        public static string RemovePreFix(this string str, params string[] preFixes)
        {
            return str.RemovePreFix(StringComparison.Ordinal, preFixes);
        }

        /// <summary>
        /// Xóa bổ tiền tố chuỗi với StringComparison
        /// </summary>
        /// <param name="str"></param>
        /// <param name="comparisonType"></param>
        /// <param name="preFixes"></param>
        /// <returns></returns>
        public static string RemovePreFix(this string str, StringComparison comparisonType, params string[] preFixes)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }

            if (preFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var preFix in preFixes)
            {
                if (str.StartsWith(preFix, comparisonType))
                {
                    return str.Right(str.Length - preFix.Length);
                }
            }

            return str;
        }

        /// <summary>
        /// Gets a substring of a string from end of the string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
        public static string Right(this string str, int len)
        {
            if(str.IsNullOrEmpty()) return string.Empty;

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(str.Length - len, len);
        }

    }
}
