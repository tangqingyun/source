using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;

namespace Excel.NPOI
{
    public class Factory
    {
        public static List<ISheet> GetSheets(string path,string ext)
        {
            List<ISheet> sheet = null;
            switch (ext)
            {
                case ".xls":
                    sheet = new ExcelXLS().GetWorkSheets(path); 
                    break;
                case ".xlsx":
                    sheet = new ExcelXLSX().GetWorkSheets(path);
                    break;
                default:
                    sheet = new ExcelXLS().GetWorkSheets(path); 
                    break;
            }
            return sheet;
        }
    }
}
