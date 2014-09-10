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
    /// 获取汉字首字母类
    /// </summary>
    public static class GetStringSpell
    {
        /// <summary>
        /// 提取汉字首字母
        /// </summary>
        /// <param name="strText">需要转换的字</param>
        /// <returns>转换结果</returns>
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
        /// 把提取的字母变成大写
        /// </summary>
        /// <param name="text">需要转换的字符串</param>
        /// <returns>转换结果</returns>
        public static string GetLowerChineseSpell(this string text)
        {
            return GetChineseSpell(text).ToLower();
        }

        /// <summary>
        /// 把提取的字母变成大写
        /// </summary>
        /// <param name="text">需要转换的字符串</param>
        /// <returns>转换结果</returns>
        public static string GetUpperChineseSpell(this string text)
        {
            return GetChineseSpell(text).ToUpper();
        }

        /// <summary>
        /// 获取单个汉字的首拼音
        /// </summary>
        /// <param name="myChar">需要转换的字符</param>
        /// <returns>转换结果</returns>
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
