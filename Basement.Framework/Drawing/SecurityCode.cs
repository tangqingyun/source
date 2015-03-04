using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;

namespace Basement.Framework.Drawing
{
    public class SecurityCode
    {
        public static string GenerateSecurityCode()
        {
            string code = string.Empty;
            Random rd = new Random();
            for (int i = 0; i < 4; i++)
            {
                code += rd.Next(10);
            }
            return code;
        }

        public static string GenerateEmailVerifyCode()
        {
            string code = string.Empty;
            Random rd = new Random();
            for (int i = 0; i < 6; i++)
            {
                code += rd.Next(10);
            }
            return code;
        }

        /// <summary>
        /// 生成验证码图片，MIME类型为"image/gif"
        /// </summary>
        /// <param name="securityCode"></param>
        /// <returns></returns>
        public static byte[] CreateSecurityCodeImage(string securityCode)
        {
            //创建一个位图对象
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(100, 45); //new System.Drawing.Bitmap((int)Math.Ceiling((securityCode.Length * 12.5)), 22);
            //创建Graphics对象
            Graphics g = Graphics.FromImage(image);

            try
            {
                //生成随机生成器
                Random random = new Random();

                //清空图片背景色
                g.Clear(Color.White);

                //画图片的背景噪音线
                for (int i = 0; i < 2; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);

                    g.DrawLine(new Pen(Color.Black), x1, y1, x2, y2);
                }

                Font font = new System.Drawing.Font("Arial", 22, (System.Drawing.FontStyle.Bold));// 12
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                //g.DrawString(securityCode, font, brush, 2, 2);
                g.DrawString(securityCode, font, brush, 12, 8);
                //画图片的前景噪音点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                //返回图片内容
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        public static void CreateImageCode(string securityCode){
            byte[] imgByte = CreateSecurityCodeImage(securityCode);
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "image/Gif";
            HttpContext.Current.Response.BinaryWrite(imgByte);
        }

        public static byte[] CreateSecurityCodeImage()
        {
            return CreateSecurityCodeImage(GenerateSecurityCode());
        }

    }
}
