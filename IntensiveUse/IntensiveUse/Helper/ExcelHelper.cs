using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                        return cell.NumericCellValue.ToString().Trim();
                        
                    }
                    catch
                    {
                        double data;
                        double.TryParse(cell.StringCellValue.Trim(), out data);
                        return data.ToString();
                        //return null;
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


    public class LandUseChange
    {
        public Situation ESituation { get; set; }
        public Situation CSituation { get; set; }
        public double EEI1 { get; set; }
        public double ECI1 { get; set; }

        public static LandUseChange operator +(LandUseChange c1, LandUseChange c2)
        {
            return new LandUseChange()
            {
                ESituation = new Situation()
                {
                    Increment = c1.ESituation.Increment + c2.ESituation.Increment,
                    Extent = c1.ESituation.Extent + c2.ESituation.Extent
                },
                CSituation=new Situation(){
                    Increment=c1.CSituation.Increment+c2.CSituation.Increment,
                    Extent=c1.CSituation.Extent+c2.CSituation.Extent
                },
                EEI1 = c1.EEI1 + c2.EEI1,
                ECI1 = c1.ECI1 + c2.ECI1
            };
        }

        public static LandUseChange operator /(LandUseChange c1, int c2)
        {
            return new LandUseChange()
            {
                ESituation = new Situation()
                {
                    Increment = c1.ESituation.Increment / c2,
                    Extent = c1.ESituation.Extent / c2
                },
                CSituation = new Situation()
                {
                    Increment=c1.CSituation.Increment/c2,
                    Extent=c1.CSituation.Extent/c2
                },
                EEI1 = c1.EEI1 / c2,
                ECI1 = c1.ECI1 / c2
            };
        }
    }

    public enum Division
    {
        [Description("总人口发展与城乡建设用地增长匹配情况")]
        Total=0,
        [Description("城镇人口与城镇工矿用地增长匹配情况")]
        Town=1,
        [Description("农村人口与村庄建设用地增长匹配情况")]
        County=2
    }

}