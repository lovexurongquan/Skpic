using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;

namespace Skpic.Common
{
    /// <summary>
    /// 验证码帮助类
    /// </summary>
    public class ValidateCodeHelper
    {
        /// <summary>
        /// 生成图片验证码
        /// </summary>
        /// <param name="heitht">图片高度</param>
        /// <param name="length">验证码的长度</param>
        /// <param name="strKey">输出参数，验证码的内容</param>
        /// <returns>图片字节流</returns>
        public static byte[] GenerateVerifyImage(int heitht, int length, ref string strKey)
        {
            if (strKey == null) throw new ArgumentNullException("strKey");
            var nBmpWidth = 13 * length + 5;
            var nBmpHeight = heitht;
            var bmp = new Bitmap(nBmpWidth, nBmpHeight);

            // 1. 生成随机背景颜色
            var rd = new Random(Guid.NewGuid().GetHashCode());
            const int nRed = 218;
            const int nGreen = 229;
            const int nBlue = 234;

            // 2. 填充位图背景
            Graphics graph = Graphics.FromImage(bmp);
            graph.FillRectangle(new SolidBrush(Color.FromArgb(nRed, nGreen, nBlue)), 0, 0, nBmpWidth, nBmpHeight);


            // 3. 绘制干扰线条，采用比背景略深一些的颜色
            const int nLines = 3;
            var sb = new SolidBrush(Color.FromArgb(nRed - 17, nGreen - 17, nBlue - 17));
            var pen = new Pen(sb, 3);
            for (var a = 0; a < nLines; a++)
            {
                var x1 = rd.Next() % nBmpWidth;
                var y1 = rd.Next() % nBmpHeight;
                var x2 = rd.Next() % nBmpWidth;
                var y2 = rd.Next() % nBmpHeight;
                graph.DrawLine(pen, x1, y1, x2, y2);
            }

            // 采用的字符集，可以随即拓展，并可以控制字符出现的几率
            const string strCode = "ABCDEFGHJKLMNPQRSTUVWXYZ";

            // 4. 循环取得字符，并绘制
            string strResult = "";
            for (var i = 0; i < length; i++)
            {
                var x = (i * 13 + rd.Next(3));
                var y = rd.Next(4) + 1;

                // 确定字体
                var font = new Font("Courier New",
                 12 + rd.Next() % 4,
                 FontStyle.Bold);
                var c = strCode[rd.Next(strCode.Length)];  // 随机获取字符
                strResult += c.ToString(CultureInfo.InvariantCulture);

                // 绘制字符
                graph.DrawString(c.ToString(CultureInfo.InvariantCulture),
                 font,
                    //new SolidBrush(Color.FromArgb(nRed - 60 + y * 3, nGreen - 60 + y * 3, nBlue - 40 + y * 3)),
                 new SolidBrush(Color.FromArgb(225, 123, 8)),
                 x,
                 y);
            }

            // 5. 输出字节流
            var bstream = new System.IO.MemoryStream();
            bmp.Save(bstream, ImageFormat.Png);
            bmp.Dispose();
            graph.Dispose();

            strKey = strResult;
            byte[] byteReturn = bstream.ToArray();
            bstream.Close();

            return byteReturn;
        }
 
    }
}