using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ZXing;

namespace Qrcode
{
    public class Qrcode
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="msg">二维码信息</param>
        /// <param name="imgsize">图片长宽</param>
        /// <param name="border">图片边框</param>
        /// <returns>图片</returns>
        public static Bitmap GenByZXingNet(string msg, int imgsize=250,int border=0)
        {
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");//编码问题
            writer.Options.Hints.Add(
                EncodeHintType.ERROR_CORRECTION,
                ZXing.QrCode.Internal.ErrorCorrectionLevel.H
            );
            int codeSizeInPixels = imgsize;   //设置图片长宽
            writer.Options.Height = writer.Options.Width = codeSizeInPixels;
            writer.Options.Margin = border;//设置边框
            ZXing.Common.BitMatrix bm = writer.Encode(msg);
            Bitmap img = writer.Write(bm);
            return img;
        }
        /// <summary>
        /// 解密二维码
        /// </summary>
        /// <param name="barcodeBitmap">图片</param>
        /// <returns>二维码信息</returns>
        public static string DecodeQrCode(Bitmap barcodeBitmap)
        {
            BarcodeReader reader = new BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            var result = reader.Decode(barcodeBitmap);
            return (result == null) ? null : result.Text;
        }


    }
}
