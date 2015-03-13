using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Excel.NPOI.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string template_excel_file = @"C:\Users\dev\Desktop\ExcelTemplet\国航旅客名单导入模板.xlsx";//模板文件
            string new_excel_file = @"C:\Users\dev\Desktop\ExcelTemplet\abc.xlsx";//新excel文件
            ExcelNPOI execel = new ExcelNPOI(template_excel_file);
            DataTable dt = CreateTable();
            execel.DataTableToExcelByTemplate(dt, new_excel_file);
        }

        static DataTable CreateTable()
        {
            DataTable guohangdt = new DataTable();
            DataColumn dc = null;
            dc = guohangdt.Columns.Add("序号");
            dc = guohangdt.Columns.Add("姓名");
            dc = guohangdt.Columns.Add("旅客类型");
            dc = guohangdt.Columns.Add("证件类型");
            dc = guohangdt.Columns.Add("Column01");
            dc = guohangdt.Columns.Add("证件号");
            dc = guohangdt.Columns.Add("Column02");
            dc = guohangdt.Columns.Add("Column03");
            dc = guohangdt.Columns.Add("儿童出生年月日");
            dc = guohangdt.Columns.Add("Column04");
            dc = guohangdt.Columns.Add("联系电话");
            dc = guohangdt.Columns.Add("Column05");
            DataRow rows1;
            rows1 = guohangdt.NewRow();
            rows1["序号"] = "1";
            rows1["姓名"] = "姓名";
            rows1["旅客类型"] = "旅客类型";
            rows1["证件类型"] = "证件类型";
            rows1["Column01"] = "Column01";
            rows1["证件号"] = "证件号";
            rows1["Column02"] = "Column02";
            rows1["Column03"] = "Column03";
            rows1["儿童出生年月日"] = "儿童出生年月日";
            rows1["Column04"] = "Column04";
            rows1["联系电话"] = "联系电话";
            rows1["Column05"] = "Column05";
            guohangdt.Rows.Add(rows1);

            DataRow rows2;
            rows2 = guohangdt.NewRow();
            rows2["序号"] = "2";
            rows2["姓名"] = "姓名";
            rows2["旅客类型"] = "旅客类型";
            rows2["证件类型"] = "证件类型";
            rows2["Column01"] = "Column01";
            rows2["证件号"] = "证件号";
            rows2["Column02"] = "Column02";
            rows2["Column03"] = "Column03";
            rows2["儿童出生年月日"] = "儿童出生年月日";
            rows2["Column04"] = "Column04";
            rows2["联系电话"] = "联系电话";
            rows2["Column05"] = "Column05";
            guohangdt.Rows.Add(rows2);
            return guohangdt;
        }
    }
}
