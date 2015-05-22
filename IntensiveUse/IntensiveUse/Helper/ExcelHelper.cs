using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace IntensiveUse.Helper
{
    public  static class ExcelHelper
    {
        public static ISheet Open(string FilePath)
        {
            IWorkbook workbook = OpenWorkbook(FilePath);
            var sheet = workbook.GetSheetAt(0);
            return sheet;
        }

        public static IWorkbook OpenWorkbook(string FilePath)
        {
            IWorkbook workbook = null;
            try
            {
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    workbook = WorkbookFactory.Create(fs);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("打开文件："+FilePath+"失败，错误原因如下："+ex.ToString());
            }
            if (workbook == null)
            {
                throw new ArgumentException("打开Excel表格失败！");
            }
            if (workbook.NumberOfSheets == 0)
            {
                throw new ArgumentException("Excel文件中没有表格。");
            }
            return workbook;
        }


        public static string GetAbsolutePath(this string File)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, File);
        }

        
        
    }
}