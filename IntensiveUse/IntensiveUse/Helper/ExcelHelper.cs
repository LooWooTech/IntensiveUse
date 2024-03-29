﻿using NPOI.SS.UserModel;
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
        /// <summary>
        /// 获取cell值
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static string GetValue(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }
            switch (cell.CellType)
            {
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString().Trim();
                case CellType.Numeric:
                    return cell.NumericCellValue.ToString().Trim();
                case CellType.String:
                    return cell.StringCellValue.Trim();
                case CellType.Formula:
                    try
                    {
                        double data;
                        double.TryParse(cell.StringCellValue.Trim(), out data);
                        return data.ToString();
                    }
                    catch
                    {
                        return null;
                    }
                default:
                    try
                    {
                        return cell.StringCellValue.Trim();
                    }
                    catch
                    {
                        return null;
                    }
            }
        }

        public static string GetAbsolutePath(this string File)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, File);
        }

        public static IRow OpenRow(ref ISheet sheet, int ID)
        {
            IRow row = sheet.GetRow(ID);
            if (row == null)
            {
                row = sheet.CreateRow(ID);
            }
            return row;
        }

        public static ICell OpenCell(ref IRow row ,int ID){
            ICell cell=row.GetCell(ID,MissingCellPolicy.CREATE_NULL_AS_BLANK);
            if(cell==null){
                cell=row.CreateCell(ID);
            }
            return cell;
        }

    }


    public class Situation
    {
        /// <summary>
        /// 增长量
        /// </summary>
        public double Increment { get; set; }
        /// <summary>
        /// 增长幅度
        /// </summary>
        public double Extent { get; set; }
    }

    public enum Division
    {
        Total=0,
        Town=1,
        County=2
    }
}