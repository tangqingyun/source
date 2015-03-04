using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using System.Collections;
using System.IO;

namespace Excel.NPOI
{
    public interface IWorkBook
    {
        /// <summary>
        /// 获得excel表中所有工作表
        /// </summary>
        /// <param name="path">传入excel路径</param>
        /// <returns></returns>
        List<ISheet> GetWorkSheets(string path);

        IList<ISheet> GetWorkSheets(Stream fs);

    }
}
