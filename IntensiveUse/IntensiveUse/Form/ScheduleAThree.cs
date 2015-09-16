using IntensiveUse.Helper;
using IntensiveUse.Manager;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Form
{
    public class ScheduleAThree:ScheduleBase,ISchedule
    {
        public Dictionary<string, List<string>> DictValue { get; set; }
         
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
            }
            this.Year = Year;
            this.City = City;
            Message(Core);
            int StartRow = 0;
            int StartLine=Begin;
            int SerialNumber = 0;
            foreach (var item in DictValue.Keys)
            {
                StartRow = Start;
                IRow row = ExcelHelper.OpenRow(ref sheet, StartRow);
                StartRow++;
                ICell cell = ExcelHelper.OpenCell(ref row, StartLine);
                if (string.IsNullOrEmpty(Distict))
                {
                    cell.SetCellValue(++SerialNumber);
                    row = ExcelHelper.OpenRow(ref sheet, StartRow++);
                    cell = ExcelHelper.OpenCell(ref row, StartLine);
                }
                
                cell.SetCellValue(item);
                var values = DictValue[item];
                foreach (var entity in values)
                {
                    row = ExcelHelper.OpenRow(ref sheet, StartRow++);
                    cell = ExcelHelper.OpenCell(ref row, StartLine);
                    cell.SetCellValue(entity);
                }
                StartLine++;
            }
            return workbook;
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