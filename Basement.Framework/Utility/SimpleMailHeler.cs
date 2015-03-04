using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Text;
using System.Linq;

namespace Basement.Framework.Utility
{
    public static class SimpleMailHelper
    {
        private static readonly string MailXMLPath = System.Configuration.ConfigurationManager.AppSettings["SystemErrorMail"];

        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        public static void SendMail(string subject, string body)
        {
            var client = new SmtpClient();
            //设置用于 SMTP 事务的主机的名称，填IP地址也可以了
            var MailConfig = GetMailConfig();
            if (MailConfig == null) return;

            client.Host = MailConfig.StmpServer;
            //设置用于 SMTP 事务的端口，默认的是 25
            client.Port = MailConfig.Port;
            client.UseDefaultCredentials = false;
            //邮件发送账户密码
            client.Credentials = new System.Net.NetworkCredential(MailConfig.UserCode, MailConfig.Password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //邮件发送
            var mail = InitMailInfo(subject, body, MailConfig);
            client.Send(mail);
        }

        /// <summary>
        /// 初始化邮件内容
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="MailConfig"></param>
        /// <returns></returns>
        private static MailMessage InitMailInfo(string subject, string body, EmailConfigInfo MailConfig)
        {
            var mail = new MailMessage();
            mail.IsBodyHtml = true;
            mail.BodyEncoding = Encoding.GetEncoding(MailConfig.EncodeType);
            mail.SubjectEncoding = Encoding.GetEncoding(MailConfig.EncodeType);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new MailAddress(MailConfig.MailFrom, MailConfig.MailFromName);


            MailConfig.MailTo.ToList().ForEach(item => mail.To.Add(new MailAddress(item.Key, item.Value)));
            MailConfig.MailCopy.ToList().ForEach(item => mail.CC.Add(new MailAddress(item.Key, item.Value)));

            return mail;
        }

        /// <summary>
        /// 获取邮件地址列表
        /// </summary>
        /// <returns></returns>
        public static EmailConfigInfo GetMailConfig()
        {
            try
            {
                var filePath = AppDomain.CurrentDomain.BaseDirectory + MailXMLPath;
                var ds = new DataSet();
                ds.ReadXml(filePath);

                var mailInfo = new EmailConfigInfo();

                if (ds.Tables["TO"] != null)
                {
                    foreach (DataRow dr in ds.Tables["TO"].Rows)
                    {
                        mailInfo.MailTo.Add(dr["mail"].ToString(), dr["name"].ToString());
                    }
                }

                if (ds.Tables["CC"] != null)
                {
                    foreach (DataRow dr in ds.Tables["CC"].Rows)
                    {
                        mailInfo.MailCopy.Add(dr["mail"].ToString(), dr["name"].ToString());
                    }
                }

                if (ds.Tables["SmtpServer"].Rows.Count > 0)
                {
                    var dr = ds.Tables["SmtpServer"].Rows[0];
                    mailInfo.StmpServer = dr["StmpServer"].ToString();
                    mailInfo.Port = int.Parse(dr["Port"].ToString());
                    mailInfo.MailFrom = dr["FromMail"].ToString();
                    mailInfo.MailFromName = dr["FromName"].ToString();
                    mailInfo.UserCode = dr["UserCode"].ToString();
                    mailInfo.Password = dr["Password"].ToString();
                    mailInfo.EncodeType = dr["EncodeType"].ToString();
                }
                return mailInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public class EmailConfigInfo
    {
        public EmailConfigInfo()
        {
            MailCopy = new Dictionary<string, string>();
            MailTo = new Dictionary<string, string>();
        }

        /// <summary>
        /// 邮件抄送列表
        /// </summary>
        public Dictionary<string, string> MailCopy { get; set; }

        /// <summary>
        /// 邮件接收列表
        /// </summary>
        public Dictionary<string, string> MailTo { get; set; }

        /// <summary>
        /// 发件人地址
        /// </summary>
        public string MailFrom { get; set; }

        /// <summary>
        /// 发件人姓名
        /// </summary>
        public string MailFromName { get; set; }

        /// <summary>
        /// 邮件服务器
        /// </summary>
        public string StmpServer { get; set; }

        /// <summary>
        ///邮箱服务器端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 邮箱账户
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 邮箱正文编码
        /// </summary>
        public string EncodeType { get; set; }
    }
}
