using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Basement.Framework.Utility
{
    public delegate string StringProcessEvaluator(string text);

    public class HtmlTool
    {
        #region = Html =
        private string _html = string.Empty;
        public string Html
        {
            get { return _html; }
            set { _html = value; }
        }
        #endregion

        #region = Instance =
        /// <summary>
        /// Instances the specified HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static HtmlTool Instance(string html)
        {
            return new HtmlTool(html);
        }
        #endregion

        #region = Constructor =
        public HtmlTool(string html)
        {
            this.Html = html;
        }
        #endregion

        #region = ReplaceText =
        /// <summary>
        /// �滻Html�е��ı�,������html��ǩ
        /// </summary>
        /// <remarks>
        /// ��html��ǩװ�����
        /// ���ÿͻ��˵�ί��
        /// ������д��
        /// </remarks>
        /// <param name="evaluator">The evaluator.</param>
        /// <returns></returns>
        public static string ReplaceText(string html, StringProcessEvaluator evaluator)
        {
            //�滻html��ǩΪ��ʶ
            html = html.Replace("@", "&#64;");
            const string strre = @"\<.+?\>";

            MatchCollection mc = Regex.Matches(html, strre);

            StringBuilder strHtml = new StringBuilder();

            int start = 0; //�ı���ͷ
            int end = 0; //�ı���β

            Queue<string> HtmlTag = new Queue<string>(); //html��ǩ
            foreach (Match m in mc)
            {
                end = m.Index - start;
                strHtml.Append(html.Substring(start, end)); //�����ı�

                HtmlTag.Enqueue(m.Value); //���������

                strHtml.Append(CharTool.Replicate("@", m.Length));

                start = m.Index + m.Length;
            }

            strHtml.Append(html.Substring(start));

            string replacehtml = strHtml.ToString();

            if (evaluator != null)
            {
                replacehtml = evaluator(replacehtml);
            }

            //�ֽ�
            mc = Regex.Matches(replacehtml, "@{2,}");
            start = 0; //html��ǩ��ͷ
            end = 0; //html��ǩ��β

            strHtml = new StringBuilder();
            foreach (Match m in mc)
            {
                end = m.Index - start;

                strHtml.Append(replacehtml.Substring(start, end)); //�����ı�

                string s = HtmlTag.Dequeue();
                while (s.Length < m.Length) //������ǩ��һ��ʱ,�ۼ�
                {
                    s += HtmlTag.Dequeue();
                }
                strHtml.Append(s);

                start = m.Index + m.Length;
            }

            strHtml.Append(replacehtml.Substring(start));

            return strHtml.ToString();
        }
        public string ReplaceText(StringProcessEvaluator evaluator)
        {
            return ReplaceText(this.Html, evaluator);
        }

        public static string ReplaceHtmlCode(string strText)
        {
            strText = Regex.Replace(strText, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            strText = Regex.Replace(strText, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            strText = Regex.Replace(strText, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            strText = Regex.Replace(strText, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            strText = Regex.Replace(strText, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            return strText;
        }
        #endregion

        #region = RemoveTag =
        /// <summary>
        /// Removes the tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        public static string RemoveTag(string html, string tag)
        {
            string returnStr;
            string regexstr1 = string.Format(@"<{0}([^>])*>", tag);
            string regexstr2 = string.Format(@"</{0}([^>])*>", tag);
            returnStr = Regex.Replace(html, regexstr1, string.Empty, RegexOptions.IgnoreCase);
            returnStr = Regex.Replace(returnStr, regexstr2, string.Empty, RegexOptions.IgnoreCase);
            return returnStr;
        }

        public string RemoveTag(string tag)
        {
            return RemoveTag(this.Html, tag);
        }

        public static string RemoveTag(string html, string[] tags)
        {
            string content = html;
            string regexstr1, regexstr2;
            foreach (string tag in tags)
            {
                if (string.IsNullOrEmpty(tag) == false)
                {
                    regexstr1 = string.Format(@"<{0}([^>])*>", tag);
                    regexstr2 = string.Format(@"</{0}([^>])*>", tag);
                    content = Regex.Replace(content, regexstr1, string.Empty, RegexOptions.IgnoreCase);
                    content = Regex.Replace(content, regexstr2, string.Empty, RegexOptions.IgnoreCase);
                }
            }
            return content;
        }

        public string RemoveTag(string[] tags)
        {
            return RemoveTag(this.Html, tags);
        }
        #endregion

        #region = Encode =
        /// <summary>
        /// ��������Ԫת�� HTML ��ʽ��
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  HtmlEncode(string)
        /// ����ʽ��������Ԫת�� HTML ���ִ���ʽ ( &....; )����õ��ĳ��Ͽ��ܾ��Ǵ���ͻ����Ե����԰��ˡ�
        /// & (��) ת�� &amp; 
        /// " (˫����) ת�� &quot; 
        /// < (С��) ת�� &lt; 
        /// > (����) ת�� &gt; 
        /// �˺�ʽֻת�������������Ԫ��������ȫ��ת���� HTML ������ ASCII ת���� 
        /// </remarks>
        /// <param name="str">�ַ������ʽ������ string ����Ԫ��ת�� HTML �������ּ��ִ������ string �����а��� Null���򷵻� ""</param>
        /// <returns>�ִ�</returns>
        public static string Encode(string html)
        {
            if (html == null)
            {
                return null;
            }
            if (html.Length == 0)
            {
                return string.Empty;
            }
            StringBuilder builder1 = new StringBuilder("", html.Length * 2);
            foreach (char ch1 in html)
            {
                if ((((ch1 > '`') && (ch1 < '{')) || ((ch1 > '@') && (ch1 < '['))) || (((ch1 == ' ') || ((ch1 > '/') && (ch1 < ':'))) || (((ch1 == '.') || (ch1 == ',')) || ((ch1 == '-') || (ch1 == '_')))))
                {
                    builder1.Append(ch1);
                }
                else
                {
                    int i = (int)ch1;
                    builder1.Append("&#" + i.ToString() + ";");
                }
            }
            return builder1.ToString();

            //if (string.IsNullOrEmpty(html) == true)
            //{
            //    return string.Empty;
            //}

            //StringBuilder str = new StringBuilder();
            //foreach (char c in html)
            //{
            //    int i = (int)c;
            //    switch (i)
            //    {
            //        case 13:
            //            str.Append("<br/>");
            //            break;
            //        case 10:
            //            break;
            //        case 34:
            //            str.Append("&quot;");
            //            break;
            //        case 32:
            //            str.Append("&nbsp;");
            //            break;
            //        default:
            //            str.AppendFormat("&#{0};", i);
            //            break;
            //    }

            //}

            //return str.ToString();
        }

        public string Encode()
        {
            return Encode(this.Html);
        }
        #endregion

        #region = Decode =
        public static string Decode(string html)
        {
            return Regex.Replace(html, @"&#\d+;", Decode);
        }

        private static string Decode(Match m)
        {
            string s = Regex.Replace(m.Value, @"^&#(\d+);$", "$1");

            int i;

            if (int.TryParse(s, out i) == true)
            {
                return ((char)i).ToString();
            }

            return string.Empty;
        }

        #endregion

        #region = EncodeAttribute =
        public static string EncodeAttribute(string strInput)
        {
            if (strInput == null)
            {
                return null;
            }
            if (strInput.Length == 0)
            {
                return string.Empty;
            }
            StringBuilder builder1 = new StringBuilder("", strInput.Length * 2);
            foreach (char ch1 in strInput)
            {
                if ((((ch1 > '`') && (ch1 < '{')) || ((ch1 > '@') && (ch1 < '['))) || (((ch1 > '/') && (ch1 < ':')) || (((ch1 == '.') || (ch1 == ',')) || ((ch1 == '-') || (ch1 == '_')))))
                {
                    builder1.Append(ch1);
                }
                else
                {
                    builder1.Append("&#" + ch1.ToString() + ";");
                }
            }
            return builder1.ToString();
        }
        #endregion

        #region = Compress =
        /// <summary>
        /// ѹ��html����
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string Compress(string text)
        {
            text = Regex.Replace(text, @"<!--\S*?-->", "");

            text = Regex.Replace(text, @"", "");

            text = Regex.Replace(text, " {2,}", " ");
            text = Regex.Replace(text, @"\s{2,}", "\n");
            text = Regex.Replace(text, @">\s+?<", "><");
            text = Regex.Replace(text, @"^\s+?", string.Empty);

            return text;
        }

        public string Compress()
        {
            return Compress(this.Html);
        }
        #endregion

        #region = ��ȡHTML�ؼ���ָ������ =
        /// <summary>
        /// Pickups the HTML control attribute.
        /// </summary>
        /// <param name="HTML">The HTML.</param>
        /// <param name="TagName">Name of the tag.</param>
        /// <param name="AttributeName">Name of the attribute.</param>
        /// <param name="RemoteUri">The remote URI.</param>
        /// <returns></returns>
        public static string[] PickupAttribute(string html, string TagName, string AttributeName, Uri RemoteUri, string ExtensionName)
        {
            Regex re;
            if (string.IsNullOrEmpty(ExtensionName) == true)
            {
                re = new Regex("<" + TagName + " .*?" + AttributeName + "=\"(?<" + AttributeName + ">.+?)\".*?/>", RegexOptions.IgnoreCase);
            }
            else
            {
                re = new Regex("<" + TagName + " .*?" + AttributeName + "=\"(?<" + AttributeName + ">.+?\\." + ExtensionName + ")\".*?/>", RegexOptions.IgnoreCase);
            }


            MatchCollection matches = re.Matches(html);

            string[] string_array = new string[matches.Count];

            int i = 0;
            foreach (Match match in matches)
            {
                string word = match.Groups[AttributeName].Value.Replace("\"", "");

                if (word.StartsWith("http") == true)
                {

                }
                else if (word.StartsWith("/") == true) //root ����
                {
                    if (RemoteUri != null)
                    {
                        word = RemoteUri.ToString().Replace(RemoteUri.AbsolutePath, string.Empty) + word;
                    }
                }
                else
                {
                }

                string_array[i] = word;
                i++;
            }

            return string_array;
        }

        public static string[] PickupAttribute(string html, string TagName, string AttributeName)
        {
            return PickupAttribute(html, TagName, AttributeName, null, null);
        }

        public string[] PickupAttribute(string TagName, string AttributeName)
        {
            return PickupAttribute(TagName, AttributeName, null, null);
        }

        public string[] PickupAttribute(string TagName, string AttributeName, Uri RemoteUri, string ExtensionName)
        {
            return PickupAttribute(this.Html, TagName, AttributeName, RemoteUri, ExtensionName);
        }
        #endregion

        #region = ConvertToText =
        /// <summary>
        /// ת��Ϊ�ı���ʽ
        /// </summary>
        /// <returns></returns>
        public string ConvertToText()
        {
            return ConvertToText(this.Html);
        }

        public static string ConvertToText(string strHTML)
        {
            strHTML = RemoveTagContent(strHTML, "head");
            strHTML = RemoveTagContent(strHTML, "script");
            strHTML = Regex.Replace(strHTML, @"<br.+>", "\n");
            strHTML = Regex.Replace(strHTML, @"&\s+;", string.Empty);
            //����HTML
            strHTML = RemoveHtml(strHTML);

            strHTML = Decode(strHTML);

            return strHTML;
        }
        #endregion

        #region = RemoveTagContent =
        public static string RemoveTagContent(string html, string tagname)
        {
            string regexstr = string.Format(@"(?i)<{0}([^>])*>(\w|\W)*?</{0}([^>])*>", tagname);//@"<script.*</script>";
            return Regex.Replace(html, regexstr, " ", RegexOptions.IgnoreCase);
        }
        #endregion

        #region = RemoveHtml =
        /// <summary>
        /// Removes the HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static string RemoveHtml(string html)
        {
            html = Regex.Replace(html, "<br>", " ");
            html = Regex.Replace(html, @"<(?:\.|\s)*?>", string.Empty);
            html = Regex.Replace(html, @"<[\s|\S]+?>", string.Empty);
            html = Regex.Replace(html, "&nbsp;", " ");
            html = Regex.Replace(html, " {2,}", " ");
            html = Regex.Replace(html, @"[\f\n\r\t\v]", string.Empty);
            return html;
        }

        public string RemoveHtml()
        {
            return RemoveHtml(this.Html);
        }
        #endregion

        /// <summary>  
        /// ȡ��HTML������ͼƬ�� URL��  
        /// </summary>  
        /// <param name="sHtmlText">HTML����</param>  
        /// <returns>ͼƬ��URL�б�</returns>  
        public static List<string> GetHtmlImageUrlList(string sHtmlText)
        {
            // ����������ʽ����ƥ�� img ��ǩ  
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            MatchCollection matches = regImg.Matches(sHtmlText);  // ����ƥ����ַ���  
            int i = 0;
            List<string> sUrlList = new List<string>();
            foreach (Match match in matches)  // ȡ��ƥ�����б�  
            {
                sUrlList.Add(match.Groups["imgUrl"].Value);
            }
            return sUrlList;
        }

    }
}
