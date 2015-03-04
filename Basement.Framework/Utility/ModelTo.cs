using EmitMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basement.Framework.Utility
{
    /// <summary>
    /// EmitMapper开源实体映射框架
    /// </summary>
    /// <typeparam name="FromModel"></typeparam>
    /// <typeparam name="ToModel"></typeparam>
    public static class ModelTo<FromModel, ToModel> where FromModel : class, new()
    {
        static ObjectsMapper<FromModel, ToModel> mapper = EmitMapper.ObjectMapperManager.DefaultInstance.GetMapper<FromModel, ToModel>();

        /// <summary>
        /// 属性赋值(单个)
        /// </summary>
        /// <param name="formModel"></param>
        /// <returns></returns>
        public static ToModel Single(FromModel formModel)
        {
            ToModel toModel = mapper.Map(formModel);
            return toModel;
        }

        /// <summary>
        /// 属性赋值(集合)
        /// </summary>
        /// <param name="fromModelList">集合</param>
        /// <returns></returns>
        public static IList<ToModel> List(IEnumerable<FromModel> fromModelList)
        {
            IList<ToModel> toModelList = mapper.MapEnum(fromModelList).ToList();
            return toModelList;
        }
    }

}
