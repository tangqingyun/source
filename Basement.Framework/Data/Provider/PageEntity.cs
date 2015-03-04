using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basement.Framework.Data
{
    /// <summary>
    /// 分页实体
    /// </summary>
    public class PageEntity
    {
        private int pageSize;
        private int currentPageIndex;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pageSize">每一个的数量</param>
        /// <param name="currentPageIndex">当前页码</param>
        public PageEntity(int pageSize, int currentPageIndex)
        {

            if (pageSize <= 0)
            {
                throw new ArgumentException("参数无效，必须为大于0的正整数", "pageSize");
            }
            if (currentPageIndex < 0)
            {
                throw new ArgumentException("参数无效，必须为大于0的正整数", "currentPageIndex");
            }
            this.pageSize = pageSize;
            this.currentPageIndex = currentPageIndex;
        }
        /// <summary>
        /// 根据页数据容量 和当前页计算的开始偏移量
        /// </summary>
        public int SetStart
        {
            get
            {
                //3 
                return currentPageIndex == 1 ? 1 : (currentPageIndex - 1) * (pageSize) + 1;
            }
        }
        /// <summary>
        /// 根据页数据容量 和当前页计算的结束偏移量
        /// </summary>
        public int SetEnd
        {
            get
            {
                return currentPageIndex * pageSize;
            }
        }

    }

}
