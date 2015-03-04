using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basement.Framework.Utility
{
    /// <summary>
    /// 表头类
    /// </summary>
    public class Header
    {
        private string headerTitle = string.Empty;
        /// <summary>
        /// 文字说明
        /// </summary>
        public string HeaderTitle
        {
            get { return headerTitle; }
            set { headerTitle = value; }
        }

        private string from = string.Empty;
        /// <summary>
        /// 开始单元格
        /// </summary>
        public string From
        {
            get { return from; }
            set { from = value; }
        }

        private string to = string.Empty;
        /// <summary>
        /// 结束单元格
        /// </summary>
        public string To
        {
            get { return to; }
            set { to = value; }
        }

        private bool isSingle = false;
        /// <summary>
        /// 是否为单个单元格
        /// </summary>
        public bool IsSingle
        {
            get { return isSingle; }
            set { isSingle = value; }
        }

        private bool isEnter = false;
        /// <summary>
        /// 是否换行
        /// </summary>
        public bool IsEnter
        {
            get { return isEnter; }
            set { isEnter = value; }
        }


    }
}
