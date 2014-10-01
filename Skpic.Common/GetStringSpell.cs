/*
 * Added by laoxu 2014-09-10 11:00:00
 * ---------------------------------------------------------------
 * for：get chinese spell by chinese string.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

using System.Text;

namespace Skpic.Common
{
    /// <summary>
    /// Get characters initials class
    /// </summary>
    public static class GetStringSpell
    {
        /// <summary>
        /// Extract characters initials
        /// </summary>
        /// <param name="strText">Need to convert word</param>
        /// <returns>Conversion results</returns>
        public static string GetChineseSpell(this string strText)
        {
            var len = strText.Length;
            var str = "";
            for (var i = 0; i < len; i++)
            {
                str += GetSpell(strText.Substring(i, 1));
            }
            return str;
        }

        /// <summary>
        /// The extraction of the letters to lowercase
        /// </summary>
        /// <param name="text">Need to convert characters</param>
        /// <returns>Conversion results</returns>
        public static string GetLowerChineseSpell(this string text)
        {
            return GetChineseSpell(text).ToLower();
        }

        /// <summary>
        /// The extracted into uppercase letters
        /// </summary>
        /// <param name="text">Need to convert characters</param>
        /// <returns>Conversion results</returns>
        public static string GetUpperChineseSpell(this string text)
        {
            return GetChineseSpell(text).ToUpper();
        }

        /// <summary>
        /// Get the first single phonetic characters
        /// </summary>
        /// <param name="myChar">Need to convert characters</param>
        /// <returns>Conversion results</returns>
        private static string GetSpell(string myChar)
        {
            var arrCn = Encoding.Default.GetBytes(myChar);
            if (arrCn.Length <= 1) return myChar;
            int area = arrCn[0];
            int pos = arrCn[1];
            var code = (area << 8) + pos;
            int[] areacode =
                {
                    45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324,
                    49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481
                };
            for (var i = 0; i < 26; i++)
            {
                var max = 55290;
                if (i != 25) max = areacode[i + 1];
                if (areacode[i] <= code && code < max)
                {
                    return Encoding.Default.GetString(new[] { (byte)(65 + i) });
                }
            }
            return "_";
        }
    }
}
