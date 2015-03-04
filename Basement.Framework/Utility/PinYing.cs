using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.International.Converters.PinYinConverter;

namespace Basement.Framework.Utility
{
    public class PinYing
    {
        #region  获取汉字首字母包含多音如 “奇” 返回 new List<string>{"Q","J"};
        /// <summary>
        /// 获取汉字首字母包含多音
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> GetPYInitialList(string str)
        {
            List<string> list = new List<string>();
            return GetPYInitialList(GetPYList(str));
        }

        /// <summary>
        /// 获取汉字首字母包含多音
        /// </summary>
        /// <param name="pyList">传入GetPY这个方法返回的对象</param>
        /// <returns></returns>
        public static List<string> GetPYInitialList(List<string> pyList)
        {
            List<string> list = new List<string>();
            foreach (string py in pyList)
            {
                string[] strs = py.Split('$');
                StringBuilder sb = new StringBuilder();
                foreach (string s in strs)
                {
                    if (s.Length > 0)
                    {
                        sb.Append(s.Substring(0, 1));
                    }
                }
                list.Add(sb.ToString());
            }
            return list;
        }
        #endregion

        #region 获取拼音包含多音,如"奇"返回new List<string>{"QI","JI"};
        /// <summary>
        /// 获取拼音包含多音
        /// </summary>
        /// <param name="pyList">传入GetPY这个方法返回的对象</param>
        /// <returns></returns>
        public static List<string> GetPYFullList(List<string> pyList)
        {
            List<string> list = new List<string>();
            foreach (string py in pyList)
            {
                list.Add(py.Replace("$", ""));
            }
            return list;
        }
        /// <summary>
        /// 获取拼音包含多音
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> GetPYFullList(string str)
        {
            List<string> list = new List<string>();
            return GetPYFullList(GetPYList(str));
        }
        #endregion

        #region 获取汉字的拼音包含多音字用"$"连接,如 "奇怪" 返回 new List<string>{"QI$GUAI","JI$GUAI"};
        /// <summary>
        /// 获取汉字的拼音包含多音字用"$"连接
        /// </summary>
        /// <param name="pyList"></param>
        /// <param name="str"></param>
        /// <param name="isFirst"></param>
        /// <returns></returns>
        public static List<string> GetPYList(string str, List<string> pyList = null, bool isFirst = true)
        {
            if (pyList == null)
            {
                pyList = new List<string>();
            }
            if (!string.IsNullOrWhiteSpace(str))
            {
                if (str.Length > 1)
                {
                    string tmp = str.Substring(0, 1);
                    string remain = str.Substring(1);
                    List<string> tmpList = GetPYALL(tmp);
                    if (isFirst)
                    {
                        pyList.AddRange(tmpList);
                        pyList = GetPYList(remain, pyList, false);
                    }
                    else
                    {
                        pyList = GetPYList(remain, CombinationList(pyList, tmpList), false);
                    }
                }
                else
                {
                    if (pyList.Count > 0)
                    {
                        pyList = CombinationList(pyList, GetPYALL(str));
                    }
                    else
                    {
                        pyList.AddRange(GetPYALL(str));
                    }
                }
            }
            return pyList;
        }
        #endregion

        #region 组合2个List，内部使用
        /// <summary>
        /// 组合2个List
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        private static List<string> CombinationList(List<string> list1, List<string> list2)
        {
            List<string> tmpPYList = new List<string>();
            foreach (string i in list1)
            {
                foreach (string j in list2)
                {
                    tmpPYList.Add(i + "$" + j);
                }
            }
            return tmpPYList;
        }
        #endregion

        #region 获取全拼,包含多音
        /// <summary>
        /// 获取全拼,包含多音
        /// </summary>
        /// <param name="str"></param>
        /// <returns>返回一个多音集合</returns>
        public static List<string> GetPYALL(string str)
        {
            List<string> soruceList = new List<string>();
            char[] chars = str.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (new Regex(@"^[\u4e00-\u9fa5]+$").Match(chars[i].ToString()).Success)
                {
                    ChineseChar chineseChar = new ChineseChar(chars[i]);
                    ReadOnlyCollection<string> pyColl = chineseChar.Pinyins;
                    //循环生成首字母的笛卡尔积,存储到临时拼音列表
                    foreach (var item in pyColl)
                    {
                        if (item != null)
                        {
                            string temp = item.Remove(item.Length - 1, 1);
                            if (!soruceList.Contains(temp))
                            {
                                soruceList.Add(temp);
                            }
                        }
                    }
                }
                else//不是汉字的
                {
                    soruceList.Add(chars[i].ToString().ToUpper());
                }
            }
            return soruceList;
        }
        #endregion
    }
}
