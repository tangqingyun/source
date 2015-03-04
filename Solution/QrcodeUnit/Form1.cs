using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QrcodeUnit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string fileName = @"C:\a.jpg";
        Bitmap bitmap = null;
        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();   //创建一个对话框对象
            ofd.Title = "请选择上传的图片";  //为对话框设置标题
            ofd.Filter = "图片格式|*.jpg";  //设置筛选的图片格式
            ofd.Multiselect = false;        //设置是否允许多选
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filePath = ofd.FileName;//获得文件的完整路径（包括名字后后缀）
                txtUploadFile.Text = filePath;//将文件路径显示在文本框中
                int position = filePath.LastIndexOf("\\");
                string fileName = filePath.Substring(position + 1);
                using (Stream stream = ofd.OpenFile())
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.Create))
                    {
                        stream.CopyTo(fs);
                        fs.Flush();
                    }
                    this.pictureBox1.ImageLocation = fileName;
                }
            }

        }

        private void btnCreateCode_Click(object sender, EventArgs e)
        {
            string text = tbText.Text.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("输入文字不能为空!");
                return;
            }
            bitmap = Qrcode.Qrcode.GenByZXingNet(tbText.Text.Trim(), 230);
            bitmap.Save(fileName);
            this.pictureBox1.ImageLocation = fileName;
        }

        private void btnDecodeQrCode_Click(object sender, EventArgs e)
        {
            string str = Qrcode.Qrcode.DecodeQrCode(bitmap);
            MessageBox.Show(str);
        }

    }
}
