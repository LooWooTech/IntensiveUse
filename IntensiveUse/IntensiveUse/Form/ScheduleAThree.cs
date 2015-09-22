using IntensiveUse.Helper;
using IntensiveUse.Manager;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Form
{
    public class ScheduleAThree:ScheduleBase,ISchedule
    {
        public Dictionary<string, List<string>> DictValue { get; set; }
        private ICell TemplateCell { get; set; }
         
        public ScheduleAThree()
        {
            this.Start = 1;
            this.Begin = 2;
            this.SerialNumber = 18;
            if (DictValue == null)
            {
                DictValue = new Dictionary<string, List<string>>();
            }
        }
        public IWorkbook Write(string FilePath, ManagerCore Core,int Year, string City,string Distict,int[] Indexs)
        {
            if (Indexs.Count() != this.SerialNumber)
            {
                throw new ArgumentException("提取数据精度位失败！无法进行生成表格");
            }
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("打开模板失败,服务器缺失文件");
            }
            if (!string.IsNullOrEmpty(Distict))
            {
                Disticts.Add(Distict);
            }
            else
            {
                Disticts = Core.ExcelManager.GetDistrict(City);
                Disticts.Add(City);
            }
            this.Year = Year;
            this.City = City;
            Message(Core);
            int StartRow = 0;
            int StartLine=Begin;
            int SerialNumber = 0;

            ICell cell = null;
            foreach (var key in DictValue.Keys)
            {
                StartRow = Start;
                cell = GetCell(sheet, StartRow++, StartLine);
                if (string.IsNullOrEmpty(Distict))
                {
                    cell.SetCellValue(++SerialNumber);
                    cell = GetCell(sheet, StartRow++, StartLine);
                }
                cell.SetCellValue(key);
                var values = DictValue[key];
                foreach (var entry in values)
                {
                    cell = GetCell(sheet, StartRow++, StartLine);
                    cell.SetCellValue(entry);
                }
                StartLine++;
            }
            if (string.IsNullOrEmpty(Distict))
            {
                StartLine--;
                sheet.AddMergedRegion(new CellRangeAddress(this.Start, this.Start + 1, StartLine, StartLine));
                cell = GetCell(sheet, this.Start, StartLine);
                cell.SetCellValue("城市行政辖区整体");
            }

            #region
            /*
            foreach (var item in DictValue.Keys)
            {
                StartRow = Start;
                IRow row = ExcelHelper.OpenRow(ref sheet, StartRow);
                StartRow++;
                TemplateCell = row.GetCell(this.Begin);
                ICell cell = ExcelHelper.OpenCell(ref row, StartLine,TemplateCell);
                if (string.IsNullOrEmpty(Distict))
                {
                    cell.SetCellValue(++SerialNumber);
                    row = ExcelHelper.OpenRow(ref sheet, StartRow++);
                    TemplateCell = row.GetCell(this.Begin);
                    cell = ExcelHelper.OpenCell(ref row, StartLine,TemplateCell);
                }
                
                cell.SetCellValue(item);
                var values = DictValue[item];
                foreach (var entity in values)
                {
                    row = ExcelHelper.OpenRow(ref sheet, StartRow++);
                    TemplateCell = row.GetCell(this.Begin);
                    cell = ExcelHelper.OpenCell(ref row, StartLine,TemplateCell);
                    cell.SetCellValue(entity);
                }
                StartLine++;
            }*/
            #endregion
            return workbook;
        }
        private ICell GetCell(ISheet sheet, int IndexRow, int IndexLine)
        {
            IRow row = sheet.GetRow(IndexRow);
            TemplateCell = row.GetCell(this.Begin);
            ICell cell = row.GetCell(IndexLine);
            if (cell == null)
            {
                cell = row.CreateCell(IndexLine, TemplateCell.CellType);
                cell.CellStyle = TemplateCell.CellStyle;
            }
            return cell;
        }

        public void Message(ManagerCore Core)
        {
            foreach (var item in Disticts)
            {
                int ID = Core.ExcelManager.GetID(item);
                List<string> values = Core.PeopleManager.Statistics(ID,Year);
                if (!DictValue.ContainsKey(item))
                {
                    DictValue.Add(item, values);
                }
            }
        }
    }
}