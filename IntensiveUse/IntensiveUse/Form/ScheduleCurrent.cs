using IntensiveUse.Helper;
using IntensiveUse.Manager;
using IntensiveUse.Models;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Form
{
    public static class ScheduleCurrent
    {
        private static int MIndex=6;
        public static IWorkbook Wirte(string FilePath, ManagerCore Core,CurrentSituation Current)
        {
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            ISheet sheet = workbook.GetSheetAt(0);
            IRow TemplateRow = sheet.GetRow(MIndex);
            var dict = Message(Core, Current);
            IRow row = null;
            ICell cell = null;
            int Index = MIndex;
            int Serial = 0;
            foreach (var key in dict.Keys)
            {
                row = sheet.GetRow(Index);
                if (row == null)
                {
                    row = sheet.CreateRow(Index);
                    //row.RowStyle = TemplateRow.RowStyle;
                }
                Serial = 0;
                cell = row.GetCell(Serial);
                if (cell == null)
                {
                    cell = row.CreateCell(Serial,TemplateRow.GetCell(Serial).CellType);
                    cell.CellStyle = TemplateRow.GetCell(Serial).CellStyle;
                }
                cell.SetCellValue(key);
                Serial++;
                foreach (var values in dict[key])
                {
                    cell = row.GetCell(Serial);
                    if (cell == null)
                    {
                        cell = row.CreateCell(Serial, TemplateRow.GetCell(Serial).CellType);
                        cell.CellStyle = TemplateRow.GetCell(Serial).CellStyle;
                    }
                    cell.SetCellValue(Math.Round(values, 6));
                    Serial++;
                }
                Index++;
            }
            return workbook;
        }

        public static Dictionary<string,Queue<Double>> Message(ManagerCore Core, CurrentSituation Current)
        {
            var dict = new Dictionary<string, Queue<double>>();
            foreach (var item in Current.Regions)
            {
                var CID = Core.ExcelManager.GetID(item);
                var exponent = Core.ExponentManager.GetTurthExponent(Current.Year, CID);
                if (!dict.ContainsKey(item))
                {
                    var queue = new Queue<double>();
                    Core.ExcelManager.Gain(exponent, ref queue);
                    dict.Add(item, queue);
                }
            }
            return dict;
        }
    }
}