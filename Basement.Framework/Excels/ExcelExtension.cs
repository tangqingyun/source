using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel.NPOI;
using System.Data;
using Basement.Framework.Utility;
using System.IO;

namespace Basement.Framework.Excels
{
    public class ExcelExtension
    {
        private ExcelNPOI excel = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="absolutePath">文件绝对路径</param>
        /// <param name="sheetIndex">第几个表</param>
        public ExcelExtension(string absolutePath, int sheetIndex = 0)
        {
            excel = new ExcelNPOI(absolutePath, sheetIndex);
        }

        /// <summary>
        /// 把Excel读入DataTable中
        /// </summary>
        /// <param name="isHead">是否把第一行当数据 true是 false否</param>
        /// <returns></returns>
        public DataTable ReadExcelAsDataTable(bool isHead = false)
        {
            return excel.ReadExcelToDataTable(isHead);
        }

        /// <summary>
        /// 把Excel读入List集合中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isHead"></param>
        /// <returns></returns>
        public IList<T> ReadExcelAsList<T>(bool isHead = false)
        {
            DataTable dt = ReadExcelAsDataTable(isHead);
            return ReflectHandle.DataTableAsList<T>(dt);
        }

        /// <summary>
        /// 将DataTable生成Excel文件
        /// </summary>
        /// <param name="dt">传入DataTable</param>
        /// <param name="abPath">保存路径</param>
        /// <param name="fileName">导出时保存的文件名</param>
        /// <param name="tableHead">设置导出Excel表头信息 如果为null 不设置</param>
        public FileStream DataTableToExcelFile(DataTable dt, string savePath, string[] tableHead = null)
        {
            return excel.DataTableToExcelFile(dt, savePath, tableHead);
        }

    }
}
