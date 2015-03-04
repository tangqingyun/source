
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Basement.Framework.Data;
using Basement.Framework.Data.Attribute;

namespace Basement.Framework.Unit
{
    [Serializable]
    [Table("CompanyGroup_Company")]
    public class Company : IEntity
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        [Column("CompanyID", DbType.Int64, true, true)]
        public long CompanyID { set; get; }
        /// <summary>
        /// 公司名
        /// </summary>
        [Column("CompanyName", DbType.String, false, false)]
        public string CompanyName { set; get; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Column("LinkMan", DbType.String, false, false)]
        public string LinkMan { set; get; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Column("Mobile", DbType.String, false, false)]
        public string Mobile { set; get; }
        /// <summary>
        /// 审核状态 -1拒审  0未审核  1已审核 
        /// </summary>
        [Column("Status", DbType.Int32, false, false)]
        public int Status { set; get; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CreateBy", DbType.Guid, false, false)]
        public Guid CreateBy { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CreateTime", DbType.DateTime, false, false)]
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [Column("CheckTime", DbType.DateTime, false, false)]
        public DateTime? CheckTime { set; get; }
        /// <summary>
        /// 审核人
        /// </summary>
        [Column("CheckBy", DbType.Guid, false, false)]
        public Guid? CheckBy { set; get; }
    }


    [Serializable]
    [Table("CompanyGroup_Company")]
    public class CompanyUni : IEntity
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        [Column("CompanyID", DbType.Int64, true, true)]
        public long CompanyID { set; get; }
        /// <summary>
        /// 公司名
        /// </summary>
        [Column("CompanyName", DbType.String, false, false)]
        public string CompanyName { set; get; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Column("LinkMan", DbType.String, false, false)]
        public string LinkMan { set; get; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Column("Mobile", DbType.String, false, false)]
        public string Mobile { set; get; }
        /// <summary>
        /// 审核状态 -1拒审  0未审核  1已审核 
        /// </summary>
        [Column("Status", DbType.Int32, false, false)]
        public int Status { set; get; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CreateBy", DbType.Guid, false, false)]
        public Guid CreateBy { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CreateTime", DbType.DateTime, false, false)]
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [Column("CheckTime", DbType.DateTime, false, false)]
        public DateTime? CheckTime { set; get; }
        /// <summary>
        /// 审核人
        /// </summary>
        [Column("CheckBy", DbType.Guid, false, false)]
        public Guid? CheckBy { set; get; }
    }

}
