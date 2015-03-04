using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web;
using Basement.Framework.Utility;


namespace Basement.Framework.Drawing
{

    /// <summary>
    /// 图片验证码
    /// </summary>
    public class ImageVerification
    {

        #region Member variables
        private static byte[] randb = new byte[4];
        private static RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();

        private static Font[] fonts = {
                                        new Font(new FontFamily("Times New Roman"), 20 + Next(4), FontStyle.Bold),
                                        new Font(new FontFamily("Georgia"), 20 + Next(4), FontStyle.Bold),
                                        new Font(new FontFamily("Arial"), 20 + Next(4), FontStyle.Bold),
                                        new Font(new FontFamily("Comic Sans MS"), 20 + Next(4), FontStyle.Bold)
                                     };

        #endregion

        #region Constructor
        /// <summary>
        /// Create Validate
        /// </summary>
        public string CheckCode = CharTool.GenerateCheckCode(5);
        public ImageVerification()
        {
            this.CreateCheckCodeImage(CheckCode);
        }


        /// <summary>
        /// Create input value to image
        /// </summary>
        /// <param name="checkCode">input value</param>
        public ImageVerification(string checkCode)
        {
            this.CreateCheckCodeImage(checkCode);
        }

        /// <summary>
        ///  Create input value to image
        /// </summary>
        /// <param name="checkCode">input value</param>
        /// <param name="fettle">fettle value(1:no style)</param>
        public ImageVerification(string checkCode, int fettle)
        {
            this.CreateCheckCodeImage(checkCode, 1);
        }

        public ImageVerification(string checkCode, int fettle = 1, int textcolor = 0, int width = 80, int height = 30, int fontSize = 12)
        {
            this.CreateCheckCodeImage(checkCode, fettle, textcolor, width, height, fontSize);
        }
        #endregion

        #region Create Check Code Image
        private void CreateCheckCodeImage(string checkCode)
        {
            this.CreateCheckCodeImage(checkCode, 0);
        }


        public static System.IO.MemoryStream GetImgMemoryStream(string checkCode)
        {
            return GetImgMemoryStream(checkCode, 0);
        }

        public static System.IO.MemoryStream GetImgMemoryStream(string checkCode, int fettle)
        {
            return GetImgMemoryStream(checkCode, fettle, 1);
        }

        public static System.IO.MemoryStream GetImgMemoryStream(string checkCode, int fettle, int textcolor)
        {
            return GetImgMemoryStream(checkCode, fettle, textcolor, 80, 30);
        }

        //历史遗留问题,该方法需要重构
        public static System.IO.MemoryStream GetImgMemoryStream(string checkCode, int fettle, int textcolor, int width, int height)
        {

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                Rectangle rect = new Rectangle(0, 0, width, height);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                //清空图片背景色
                g.Clear(Color.White);

                Font font;
                System.Drawing.Drawing2D.LinearGradientBrush brush;

                switch (fettle)
                {
                    case 0:
                        #region 0
                        //画图片的背景噪音线
                        for (int i = 0; i < 25; i++)
                        {
                            int x1 = random.Next(image.Width);
                            int x2 = random.Next(image.Width);
                            int y1 = random.Next(image.Height);
                            int y2 = random.Next(image.Height);

                            g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                        }
                        font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                        brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);

                        g.DrawString(checkCode, font, brush, 2, 2);

                        //画图片的前景噪音点
                        for (int i = 0; i < 100; i++)
                        {
                            int x = random.Next(image.Width);
                            int y = random.Next(image.Height);

                            image.SetPixel(x, y, Color.FromArgb(random.Next()));
                        }

                        //画图片的边框线
                        g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                        #endregion
                        break;
                    case 1:
                        #region 1
                        font = new System.Drawing.Font("宋体", 12);

                        brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Black, Color.Black, 1.2f, true);

                        g.DrawString(checkCode, font, brush, 2, 2);
                        #endregion
                        break;
                    case 2:
                        #region 2
                        int fixedNumber = textcolor == 2 ? 60 : 0;

                        SolidBrush drawBrush = new SolidBrush(Color.FromArgb(Next(100), Next(100), Next(100)));
                        for (int x = 0; x < 3; x++)
                        {
                            Pen linePen = new Pen(Color.FromArgb(Next(150) + fixedNumber, Next(150) + fixedNumber, Next(150) + fixedNumber), 1);
                            g.DrawLine(linePen, new PointF(0.0F + Next(20), 0.0F + Next(height)), new PointF(0.0F + Next(width), 0.0F + Next(height)));
                        }

                        Matrix m = new Matrix();
                        for (int x = 0; x < checkCode.Length; x++)
                        {
                            m.Reset();
                            m.RotateAt(Next(30) - 15, new PointF(Convert.ToInt64(width * (0.10 * x)), Convert.ToInt64(height * 0.5)));
                            g.Transform = m;
                            drawBrush.Color = Color.FromArgb(Next(150) + fixedNumber + 20, Next(150) + fixedNumber + 20, Next(150) + fixedNumber + 20);
                            PointF drawPoint = new PointF(0.0F + Next(4) + x * 20, 3.0F + Next(3));
                            g.DrawString(Next(1) == 1 ? checkCode[x].ToString() : checkCode[x].ToString().ToUpper(), fonts[Next(fonts.Length - 1)], drawBrush, drawPoint);
                            g.ResetTransform();
                        }



                        double distort = Next(5, 10) * (Next(10) == 1 ? 1 : -1);

                        using (Bitmap copy = (Bitmap)image.Clone())
                        {
                            for (int y = 0; y < height; y++)
                            {
                                for (int x = 0; x < width; x++)
                                {
                                    int newX = (int)(x + (distort * Math.Sin(Math.PI * y / 84.5)));
                                    int newY = (int)(y + (distort * Math.Cos(Math.PI * x / 54.5)));
                                    if (newX < 0 || newX >= width)
                                        newX = 0;
                                    if (newY < 0 || newY >= height)
                                        newY = 0;
                                    //image.SetPixel(x, y, copy.GetPixel(newX, newY));
                                }
                            }
                        }
                        #endregion
                        break;
                    default:
                        break;
                }

                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
            return ms;
        }

        private void CreateCheckCodeImage(string checkCode, int fettle = 1)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;

            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "image/Gif";
            HttpContext.Current.Response.BinaryWrite(GetImgMemoryStream(checkCode, fettle).ToArray());
        }

        private void CreateCheckCodeImage(string checkCode, int fettle = 1, int textcolor =0, int width = 80, int height = 30, int fontSize = 12)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "image/Gif";
            HttpContext.Current.Response.BinaryWrite(GetImgMemoryStream(checkCode, fettle, textcolor, width, height).ToArray());
        }

        #endregion

        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        private static int Next(int max)
        {
            rand.GetBytes(randb);
            int value = BitConverter.ToInt32(randb, 0);
            value = value % (max + 1);
            if (value < 0)
                value = -value;
            return value;
        }

        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        private static int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }
    }
}
