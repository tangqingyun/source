using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace Basement.Framework.Drawing
{
    /// <summary>
    /// ͼƬ���������
    /// </summary>
    public static class ImageExtension
    {
        #region Thumbnail Image

        /// <summary>
        /// ���ع̶���ȵ�����ͼ
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static Image GetThumbnailImageWithWidth(Image image, int width)
        {
            if (image == null)
                return null;

            int height = image.Height * width / image.Width;
            return GetThumbnailImage(image, width, height, true);
        }

        /// <summary>
        /// ���ع̶��߶ȵ�����ͼ
        /// </summary>
        /// <param name="image"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Image GetThumbnailImageWithHeight(Image image, int height)
        {
            if (image == null)
                return null;

            int width = image.Width * height / image.Height;
            return GetThumbnailImage(image, width, height, true);
        }

        /// <summary>
        /// ���ع̶����ȺͿ�ȵ�����ͼ���������ֲü�
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Image GetThumbnailImageWithCutOut(Image image, int width, int height)
        {
            if (image == null)
                return null;

            double imageHeightRatio = (double)image.Height / image.Width;
            double heightRatio = (double)height / width;

            Image newImage;
            Rectangle srcRectangle;
            if (heightRatio == imageHeightRatio) // ����һ��������Ҫ��
            {
                newImage = GetThumbnailImageWithHeight(image, height);
                return newImage;
            }
            else if (heightRatio > imageHeightRatio) // ������ 
            {
                newImage = GetThumbnailImageWithHeight(image, height);
                srcRectangle = new Rectangle((newImage.Width - width) / 2, 0, width, height);
            }
            else
            { // ��β
                newImage = GetThumbnailImageWithWidth(image, width);
                srcRectangle = new Rectangle(0, 0, width, height);
            }
            Image timg = GetThumbnailImageEx(newImage, width, height, srcRectangle);
            newImage.Dispose();
            return timg;

        }

        public static Image GetThumbnailImage(Image image, int size)
        {
            return GetThumbnailImage(image, size);
        }

        public static Image GetThumbnailImage(Image image, int width, int height)
        {
            if (width == height)
                return GetThumbnailImage(image, width);

            Size size = GetThumbnailImageSize(image.Width, image.Height, width, height);
            return GetThumbnailImage(image, size.Width, size.Height);
        }

        public static Image GetThumbnailImage(Image image, int width, int height, bool keepRatio)
        {
            if (width == height)
                return GetThumbnailImage(image, width);
            return GetThumbnailImage(image, width, height, keepRatio);
        }

        /// <summary>
        /// ��ȡ����ͼ�ߴ�
        /// ע������Width >= Height
        /// �������ͼ�ߴ��ԭͼ����ô����ԭͼ��С
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Size GetThumbnailImageSize(int imageWidth, int imageHeight, int width, int height)
        {
            if ((imageWidth == width && imageHeight == height) || (imageWidth == height && imageHeight == width))
                return new Size(imageWidth, imageHeight);

            int desiredWidth, desiredHeight;
            double heightRatio = (double)imageHeight / imageWidth;
            double widthRatio = (double)imageWidth / imageHeight;

            // ����������ͼƬ
            //
            if (widthRatio < 1)
            {
                desiredHeight = width;
                desiredWidth = height;
            }
            else
            {
                desiredHeight = height;
                desiredWidth = width;
            }

            if (imageWidth < desiredWidth && imageHeight < desiredHeight)
                return new Size(imageWidth, imageHeight);


            height = desiredHeight;

            if (widthRatio > 0)
                width = Convert.ToInt32(height * widthRatio);

            if (width > desiredWidth)
            {
                width = desiredWidth;
                height = Convert.ToInt32(width * heightRatio);
            }

            return new Size(width, height);
        }

        #endregion

        #region Thumbnail Image Ex
        /// <summary>
        /// ��ȡ�����ε�����ͼ
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static Image GetThumbnailImageEx(Image image, int width)
        {
            if (image == null || width < 1)
                return null;
            Rectangle srcRectangle;
            int x, y, w;

            if (image.Width > image.Height)
            {
                x = (image.Width - image.Height) / 2;
                y = 0;
                w = image.Height;
            }
            else
            {
                x = 0;
                y = (image.Height - image.Width) / 2;
                w = image.Width;
            }
            srcRectangle = new Rectangle(x, y, w, w);

            return GetThumbnailImageEx(image, width, width, srcRectangle);
        }

        /// <summary>
        /// ��ȡ����ͼ(����ͼƬ����)
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Image GetThumbnailImageEx(Image image, int width, int height)
        {
            return GetThumbnailImageEx(image, width, height, true);
        }

        /// <summary>
        /// ����ͼ
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="maintainRatio">�Ƿ񱣳�ͼƬ����</param>
        /// <returns></returns>
        public static Image GetThumbnailImageEx(Image image, int width, int height, bool maintainRatio)
        {
            if (image == null || width < 1 || height < 1)
                return null;

            // ���ֱ���
            //
            if (maintainRatio)
            {
                double heightRatio = (double)image.Height / image.Width;
                double widthRatio = (double)image.Width / image.Height;

                int desiredHeight = height;
                int desiredWidth = width;


                height = desiredHeight;
                if (widthRatio > 0)
                    width = Convert.ToInt32(height * widthRatio);

                if (width > desiredWidth)
                {
                    width = desiredWidth;
                    height = Convert.ToInt32(width * heightRatio);
                }
            }

            Rectangle srcRectangle = new Rectangle(0, 0, image.Width, image.Height);

            return GetThumbnailImageEx(image, width, height, srcRectangle);
        }

        private static Image GetThumbnailImageEx(Image image, int width, int height, Rectangle srcRectangle)
        {
            if (image == null || width < 1 || height < 1)
                return null;

            // �½�һ��bmpͼƬ
            //
            Image bitmap = new System.Drawing.Bitmap(width, height, PixelFormat.Format24bppRgb);

            // �½�һ������
            //
            using (Graphics g = System.Drawing.Graphics.FromImage(bitmap))
            {

                // ���ø�������ֵ��
                //
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // ���ø�����,���ٶȳ���ƽ���̶�
                //
                g.SmoothingMode = SmoothingMode.HighQuality; // SmoothingMode.AntiAlias;

                // �����������ٶȸ���
                //
                g.CompositingQuality = CompositingQuality.HighQuality;

                g.PixelOffsetMode = PixelOffsetMode.HighQuality;



                // ��ջ�������͸������ɫ���
                //
                g.Clear(Color.Transparent);

                // ��ָ��λ�ò��Ұ�ָ����С����ԭͼƬ��ָ������
                //
                g.DrawImage(image, new Rectangle(0, 0, width, height), srcRectangle, GraphicsUnit.Pixel);

                return bitmap;
            }
        }


        /// <summary>
        /// ��ȡͼ���������������������Ϣ
        /// </summary>
        /// <param name="mimeType">��������������Ķ���;�����ʼ�����Э�� (MIME) ���͵��ַ���</param>
        /// <returns>����ͼ���������������������Ϣ</returns>
        public static ImageCodecInfo GetImageCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }

        public static ImageCodecInfo GetImageCodecInfo(ImageFormat format)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo icf in encoders)
            {
                if (icf.FormatID == format.Guid)
                {
                    return icf;
                }
            }

            return null;
        }

        public static void SaveImage(Image image, string savePath)
        {
            SaveImage(image, savePath, GetImageCodecInfo(ImageFormat.Jpeg));
        }

        /// <summary>
        /// ����������ͼƬ
        /// </summary>
        /// <param name="image"></param>
        /// <param name="savePath"></param>
        /// <param name="ici"></param>
        public static void SaveImage(Image image, string savePath, ImageCodecInfo ici)
        {
            // ���� ԭͼƬ ����� EncoderParameters ����
            //
            EncoderParameters parms = new EncoderParameters(1);
            EncoderParameter parm = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ((long)95));
            parms.Param[0] = parm;

            string dir = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            image.Save(savePath, ici, parms);
            parms.Dispose();
        }
        #endregion

        #region WaterMark

        /// <summary>
        /// ���ˮӡ���ֵ�ͼƬ��
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="watermarkText"></param>
        /// <param name="position"></param>
        public static void AddWatermarkText(ref Graphics picture, int width, int height, string watermarkText, WatermarkPosition position)
        {
            int alpha = 250;
            AddWatermarkText(ref picture, width, height, watermarkText, position, alpha);
        }

        /// <summary>
        /// ���ˮӡ���ֵ�ͼƬ��
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="watermarkText"></param>
        /// <param name="position"></param>
        /// <param name="alpha"></param>
        public static void AddWatermarkText(ref Graphics picture, int width, int height, string watermarkText, WatermarkPosition position, int alpha)
        {
            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };
            Font crFont = null;
            SizeF crSize = new SizeF();
            for (int i = 0; i < 7; i++)
            {
                crFont = new Font("Verdana", sizes[i], FontStyle.Bold);
                crSize = picture.MeasureString(watermarkText, crFont);

                if ((ushort)crSize.Width < (ushort)width)
                    break;
            }

            float xpos = 0;
            float ypos = 0;

            switch (position)
            {
                case WatermarkPosition.TOP_LEFT:
                    xpos = ((float)width * (float).01) + (crSize.Width / 2);
                    ypos = (float)height * (float).01;
                    break;
                case WatermarkPosition.TOP_CENTER:
                    xpos = ((float)width - crSize.Width) / 2;
                    ypos = (float)height * (float).01;
                    break;
                case WatermarkPosition.TOP_RIGHT:
                    xpos = ((float)width * (float).99) - (crSize.Width / 2);
                    ypos = (float)height * (float).01;
                    break;
                case WatermarkPosition.CENTER_LEFT:
                    xpos = ((float)width * (float).01) + (crSize.Width / 2);
                    ypos = ((float)height - crSize.Height) / 2;
                    break;
                case WatermarkPosition.CENTER_CENTER:
                    xpos = ((float)width - crSize.Width) / 2;
                    ypos = ((float)height - crSize.Height) / 2;
                    break;
                case WatermarkPosition.CENTER_RIGHT:
                    xpos = ((float)width * (float).99) - (crSize.Width / 2);
                    ypos = ((float)height - crSize.Height) / 2;
                    break;
                case WatermarkPosition.BOTTOM_RIGHT:
                    xpos = ((float)width - crSize.Width) / 2;
                    ypos = ((float)height * (float).99) - crSize.Height;
                    break;
                case WatermarkPosition.BOTTOM_CENTER:
                    xpos = ((float)width * (float).99) - (crSize.Width / 2);
                    ypos = ((float)height * (float).99) - crSize.Height;
                    break;
                case WatermarkPosition.BOTTOM_LEFT:
                    xpos = ((float)width * (float).01) + (crSize.Width / 2);
                    ypos = ((float)height * (float).99) - crSize.Height;
                    break;
            }

            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(alpha, 0, 0, 0));
            picture.DrawString(watermarkText, crFont, semiTransBrush2, xpos + 1, ypos + 1, StrFormat);

            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(alpha, 255, 255, 255));
            picture.DrawString(watermarkText, crFont, semiTransBrush, xpos, ypos, StrFormat);


            semiTransBrush2.Dispose();
            semiTransBrush.Dispose();
        }

        /// <summary>
        /// ���ˮӡͼƬ��ͼƬ��
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="watermark"></param>
        /// <param name="position"></param>
        public static void AddWatermarkImage(ref Graphics picture, int width, int height, Image watermark, WatermarkPosition position)
        {

            using (ImageAttributes imageAttributes = new ImageAttributes())
            {
                ColorMap colorMap = new ColorMap();

                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                ColorMap[] remapTable = { colorMap };

                imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                float[][] colorMatrixElements = {
												new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
												new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
												new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
												new float[] {0.0f,  0.0f,  0.0f,  1.0f, 0.0f},
												new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
											};

                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                int xpos = 0;
                int ypos = 0;

                switch (position)
                {
                    case WatermarkPosition.TOP_LEFT:
                        xpos = 10;
                        ypos = 10;
                        break;
                    case WatermarkPosition.TOP_CENTER:
                        xpos = (width - watermark.Width) / 2;
                        ypos = 10;
                        break;
                    case WatermarkPosition.TOP_RIGHT:
                        xpos = ((width - watermark.Width) - 10);
                        ypos = 10;
                        break;
                    case WatermarkPosition.CENTER_RIGHT:
                        xpos = ((width - watermark.Width) - 10);
                        ypos = (height - watermark.Height) / 2;
                        break;
                    case WatermarkPosition.CENTER_CENTER:
                        xpos = (width - watermark.Width) / 2;
                        ypos = (height - watermark.Height) / 2;
                        break;
                    case WatermarkPosition.CENTER_LEFT:
                        xpos = 10;
                        ypos = (height - watermark.Height) / 2;
                        break;
                    case WatermarkPosition.BOTTOM_RIGHT:
                        xpos = ((width - watermark.Width) - 10);
                        ypos = height - watermark.Height - 10;
                        break;
                    case WatermarkPosition.BOTTOM_CENTER:
                        xpos = (width - watermark.Width) / 2;
                        ypos = height - watermark.Height - 10;
                        break;
                    case WatermarkPosition.BOTTOM_LEFT:
                        xpos = 10;
                        ypos = height - watermark.Height - 10;
                        break;
                }

                picture.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);


                watermark.Dispose();
                imageAttributes.Dispose();
            }
        }

        #endregion

    }

    /// <summary>
    /// ˮӡλ��
    /// </summary>
    public enum WatermarkPosition
    {
        /// <summary>
        /// ����
        /// </summary>
        TOP_LEFT,

        /// <summary>
        /// ����
        /// </summary>
        TOP_CENTER,

        /// <summary>
        /// ����
        /// </summary>
        TOP_RIGHT,

        /// <summary>
        /// ����
        /// </summary>
        CENTER_RIGHT,

        /// <summary>
        /// ����
        /// </summary>
        CENTER_CENTER,

        /// <summary>
        /// ����
        /// </summary>
        CENTER_LEFT,

        /// <summary>
        /// ����
        /// </summary>
        BOTTOM_LEFT,

        /// <summary>
        /// ����
        /// </summary>
        BOTTOM_CENTER,

        /// <summary>
        /// ����
        /// </summary>
        BOTTOM_RIGHT
    }
}
