using System;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Basement.Framework.Security
{

	public class ValidateUtil
	{
		public static Regex RegNumber = new Regex("^[0-9]+$");//�޷�������������ǰ���ж���0��
        public static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");//�з�������������ǰ���ж���0��
        public static Regex RegDecimal = new Regex("^[0-9]+(([.][0-9]+)|[0-9]?)$");//�޷���������С��������ǰ���ж���0��
        public static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+(([.][0-9]+)|[0-9]?)$"); //�з���������С��������ǰ���ж���0��
       
        public static Regex RegEmail = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");//Email
        //����yyyy-MM-dd��yyyy:0001-9999 �Ѿ�����ƽ������꣩
        public static Regex RegDate = new Regex("^(([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})-(((0[13578]|1[02])-(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)-(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8]))))|((([0-9]{2})(0[48]|[2468][048]|[13579][26])|((0[48]|[2468][048]|[3579][26])00))-02-29)$");
        //����MM/dd/yyyy��yyyy:0001-9999 �Ѿ�����ƽ������꣩
        public static Regex RegDate2 = new Regex("^((((0[13578]|1[02]))/(0[1-9]|[12][0-9]|3[01])|((0[469]|11)/(0[1-9]|[12][0-9]|30))|(02)/(0[1-9]|[1][0-9]|2[0-8]))/([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3}))|(02/29/(([0-9]{2})(0[48]|[2468][048]|[13579][26])|((0[48]|[2468][048]|[3579][26])00)))$");
        
        public static Regex RegLegalUsername = new Regex(@"^[a-zA-Z]\w*$");//�Ϸ��û�����⣨ֻ�ܰ�����ĸ�����֡��»��ߣ�������ĸ��ͷ������>=3,<=20��
        public static Regex RegLegalPassword = new Regex(@"^\w*$");//�Ϸ������루ֻ�ܰ�����ĸ�����֡��»���,����>=6,<=20��
        public static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");//�����ַ����
        public static Regex RegLegalUsernameOrEmail = new Regex(@"(^[a-zA-Z]\w{2,19}$)|(^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$)");//�Ϸ����û�����Email

		#region �����ַ������
		/// <summary>
		/// �Ƿ������ַ���
		/// </summary>
		/// <param name="inputData">�����ַ���</param>
		/// <returns></returns>
		public static bool IsNumber(string inputData)
		{
			Match m = RegNumber.Match(inputData);
			return m.Success;
		}		
		/// <summary>
		/// �Ƿ������ַ��� �ɴ�������
		/// </summary>
		/// <param name="inputData">�����ַ���</param>
		/// <returns></returns>
		public static bool IsNumberSign(string inputData)
		{
			Match m = RegNumberSign.Match(inputData);
			return m.Success;
		}		
		/// <summary>
		/// �Ƿ��Ǹ�����
		/// </summary>
		/// <param name="inputData">�����ַ���</param>
		/// <returns></returns>
		public static bool IsDecimal(string inputData)
		{
			Match m = RegDecimal.Match(inputData);
			return m.Success;
		}		
		/// <summary>
		/// �Ƿ��Ǹ����� �ɴ�������
		/// </summary>
		/// <param name="inputData">�����ַ���</param>
		/// <returns></returns>
		public static bool IsDecimalSign(string inputData)
		{
			Match m = RegDecimalSign.Match(inputData);
			return m.Success;
		}

        /// <summary>
        /// ��֤�ֻ���
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsMobile(string str)
        {
            return Regex.IsMatch(str, "^1[0-9]{10}$", RegexOptions.IgnoreCase);
        }


		#endregion
        #region �ʼ���ַ
        /// <summary>
        /// �Ƿ��Ǹ����� �ɴ�������
        /// </summary>
        /// <param name="inputData">�����У����ַ�����Ϣ</param>
        /// <returns></returns>
        public static bool IsEmail(string inputData)
        {
            Match m = RegEmail.Match(inputData);
            return m.Success;
        }
        #endregion
        #region �����ַ������
        /// <summary>
        /// ��������ַ���
        /// </summary>
        /// <param name="inputData">�����У����ַ�����Ϣ</param>
        /// <returns></returns>
        public static bool IsDate(string inputData)
        {
            Match m = RegDate.Match(inputData);
            Match m2 = RegDate2.Match(inputData);
            return m.Success || m2.Success;
        }
        #endregion
        #region �Ϸ��û�������
        /// <summary>
        /// ����Ƿ�Ϊ�Ϸ��û���
        /// </summary>
        /// <param name="inputData">�����У����ַ�����Ϣ</param>
        /// <returns></returns>
        public static bool IsLegalUsername(string inputData)
        {
            Match m = RegLegalUsername.Match(inputData);
            return m.Success;
        }

        public static bool IsLegalUsernameOrEmail(string inputData)
        {
            Match m = RegLegalUsernameOrEmail.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// ����Ƿ�Ϊ�Ϸ�����
        /// </summary>
        /// <param name="inputData">�����У����ַ�����Ϣ</param>
        /// <returns></returns>
        public static bool IsLegalPassword(string inputData)
        {
            Match m = RegLegalPassword.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// У���ֻ������Ƿ���ϱ�׼��
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool ValidateMobile(string mobile)
        {
            if (string.IsNullOrEmpty(mobile))
                return false;

            return Regex.IsMatch(mobile, @"^(13|14|15|16|18|19)\d{9}$");
        }

        #endregion
        #region ���ļ��
        /// <summary>
		/// ����Ƿ��������ַ�
		/// </summary>
        /// <param name="inputData">�����У����ַ�����Ϣ</param>
		/// <returns></returns>
		public static bool IsHasCHZN(string inputData)
		{
			Match m = RegCHZN.Match(inputData);
			return m.Success;
		}	
		#endregion
        #region ���ϱȽ�
        public static bool IsCollectionEqual<T>(ICollection<T> a, ICollection<T> b)
        {
            foreach (T ta in a)
            {
                bool find = false;
                foreach (T tb in b)
                {
                    if (tb.Equals(ta))
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                    return false;
            }
            foreach (T tb in b)
            {
                bool find = false;
                foreach (T ta in a)
                {
                    if (ta.Equals(tb))
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                    return false;
            }
            if (b.Count != a.Count)
                return false;
            return true;
        }

        public static bool IsChildCollection<T>(ICollection<T> a, ICollection<T> b)
        {
            foreach (T at in a)
            {
                if (!b.Contains(at))
                    return false;
            }
            return true;
        }

        public static bool IsSet<T>(ICollection<T> a)
        {
            Collection<T> temp = new Collection<T>();
            foreach (T at in a)
            {
                if (temp.Contains(at))
                    return false;
                temp.Add(at);
            }
            return true;
        }
        #endregion
    }
}
