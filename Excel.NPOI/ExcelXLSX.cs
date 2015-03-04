using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;

namespace Excel.NPOI
{
    public class ExcelXLSX:IWorkBook
    {
        public List<ISheet> GetWorkSheets(string path)
        {
            List<ISheet> list = new List<ISheet>();
            FileStream fs = new FileStream(path, FileMode.Open);
            IWorkbook wb = new XSSFWorkbook(fs);
            for (int i = 0; i < wb.NumberOfSheets; i++)
            {
                list.Add(wb.GetSheetAt(i));
            }
            fs.Close();
            return list;
        }


        public IList<ISheet> GetWorkSheets(Stream fs)
        {
            List<ISheet> list = new List<ISheet>();
            IWorkbook wb = new XSSFWorkbook(fs);
            for (int i = 0; i < wb.NumberOfSheets; i++)
            {
                list.Add(wb.GetSheetAt(i));
            }
            fs.Close();
            return list;
        }


    }
}
