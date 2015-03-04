using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Configuration;
using System.Web.Configuration;
using System.Net.Mail;
using System.Configuration;
using Basement.Framework.Utility;


namespace Basement.Framework.Net
{
    #region Mail
    /// 
    /// SystemMail ��ժҪ˵����
    /// 
    public static class Mail
    {

        public static void SendMail(string to, string subject, string message, bool ishtml, Encoding encode)
        {
            SendMail(to,subject,message,ishtml,encode,MailSendSource.Web);
        }

        /// <summary>
        /// ���ʼ�
        /// </summary>
        /// <param name="to">�ռ���(�Էֺŷָ��ĵ����ʼ���ַ�б�)</param>
        /// <param name="subject">����</param>
        /// <param name="message">�ʼ���</param>
        /// <param name="ishtml">�Ƿ�ʹ��HTML����</param>
        /// <example>SendMail("test@yahoo.com.cn", DateTime.Now.ToString(), DateTime.Now.ToString());</example>
        public static void SendMail(string to, string subject, string message, bool ishtml, Encoding encode, MailSendSource Source)
        {
            if (ishtml == false)
            {
                message = HtmlTool.ConvertToText(message);
            }
            SmtpClient smtpClient = new SmtpClient();
            System.Configuration.Configuration config = null;
            if (Source == MailSendSource.Web)
            {
                config = WebConfigurationManager.OpenWebConfiguration("/");
            }
            else
            {
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            MailSettingsSectionGroup netSmtpMailSection = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");

            using (MailMessage msg = new MailMessage(netSmtpMailSection.Smtp.From, to, subject, message))
            {
                msg.BodyEncoding = encode;
                msg.IsBodyHtml = ishtml;
                smtpClient.Send(msg);
            }
        }

        public static void SendMail(string from, string to, string subject, string message, bool ishtml, Encoding encode)
        {
            SmtpClient smtpClient = new SmtpClient();
            using (MailMessage msg = new MailMessage(from, to, subject, message))
            {
                msg.BodyEncoding = encode;
                msg.IsBodyHtml = ishtml;
                smtpClient.Send(msg);
            }
        }

        public static void SendMail(string to, string subject, string message, bool ishtml)
        {
            SendMail(to, subject, message, ishtml, Encoding.GetEncoding("GB2312"));
        }

        public static void SendMail(string to, string subject, string message)
        {
            SendMail(to, subject, message, true);
        }
    }
    #endregion

    public enum MailSendSource
    {
        Web = 0,
        App
    }
}
