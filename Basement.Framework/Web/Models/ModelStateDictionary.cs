using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basement.Framework.Web.Models
{
    public class ModelStateDictionary
    {
        /// <summary>
        /// 验证是否合法
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (Errors == null)
                {
                    return true;
                }
                if (Errors.Count() > 0)
                {
                    return false;
                }
                return true;
            }
        }
        /// <summary>
        /// 错误消息
        /// </summary>
        public IEnumerable<ModelError> Errors { set; get; }

    }
}
