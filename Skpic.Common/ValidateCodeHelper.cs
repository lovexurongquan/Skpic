/*
 * Added by laoxu 2014-09-10 11:00:00
 * ---------------------------------------------------------------
 * for：validate picture.
 * ---------------------------------------------------------------
 * version:1.0
 * mail:lovexurongquan@163.com
 */

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;

namespace Skpic.Common
{
    /// <summary>
    /// Verification code helper class
    /// </summary>
    public class ValidateCodeHelper
    {
        /// <summary>
        /// Generated image verification code
        /// </summary>
        /// <param name="heitht">Image Height</param>
        /// <param name="length">Length verification code</param>
        /// <param name="strKey">Output parameters , validate the content code</param>
        /// <returns>Picture byte stream</returns>
        public static byte[] GenerateVerifyImage(int heitht, int length, ref string strKey)
        {
            if (strKey == null) throw new ArgumentNullException("strKey");
            var nBmpWidth = 13 * length + 5;
            var nBmpHeight = heitht;
            var bmp = new Bitmap(nBmpWidth, nBmpHeight);

            // 1. Generates a random background color
            var rd = new Random(Guid.NewGuid().GetHashCode());
            const int nRed = 218;
            const int nGreen = 229;
            const int nBlue = 234;

            // 2. Fill the bitmap background
            Graphics graph = Graphics.FromImage(bmp);
            graph.FillRectangle(new SolidBrush(Color.FromArgb(nRed, nGreen, nBlue)), 0, 0, nBmpWidth, nBmpHeight);

            // 3. Draw interference lines, using some slightly darker than the background color
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

            // Use of character sets , you can then expand , and you can control the probability of characters appear
            const string strCode = "ABCDEFGHJKLMNPQRSTUVWXYZ";

            // 4. Cycle made ​​characters and plot
            string strResult = "";
            for (var i = 0; i < length; i++)
            {
                var x = (i * 13 + rd.Next(3));
                var y = rd.Next(4) + 1;

                // Determine the font
                var font = new Font("Courier New",
                 12 + rd.Next() % 4,
                 FontStyle.Bold);
                var c = strCode[rd.Next(strCode.Length)];  // Get random characters
                strResult += c.ToString(CultureInfo.InvariantCulture);

                // Drawing characters
                graph.DrawString(c.ToString(CultureInfo.InvariantCulture),
                 font,
                    //new SolidBrush(Color.FromArgb(nRed - 60 + y * 3, nGreen - 60 + y * 3, nBlue - 40 + y * 3)),
                 new SolidBrush(Color.FromArgb(225, 123, 8)),
                 x,
                 y);
            }

            // 5. Output byte stream
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