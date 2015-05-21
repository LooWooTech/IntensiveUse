using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace IntensiveUse.Form
{
    public class TableBase
    {
        public string Name { get; set; }
        

        private  Regex _yearRe = new Regex(@"^20[0-9]{2}", RegexOptions.Compiled);
        public  bool VerificationYear(string value)
        {
            return _yearRe.IsMatch(value);
        }

        /// <summary>
        /// 获取cell值
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public string GetValue(ICell cell)
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

        public IRow RowGet(ISheet sheet,int ID)
        {
            IRow row = sheet.GetRow(ID);
            if (row == null)
            {
                throw new ArgumentException("未获取相关EXCEL表格行，请核对表格数据，如有冲突请联系相关人员");
            }
            return row;
        }
    }
}