using IntensiveUse.Helper;
using IntensiveUse.Manager;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Form
{
    public class ScheduleAThree:ISchedule
    {
        public const int Start = 1;
        public const int Begin = 2;
        public Dictionary<string, List<string>> DictData { get; set; }
        public ScheduleAThree()
        {
            if (DictData == null)
            {
                DictData = new Dictionary<string, List<string>>();
            }
        }
        public IWorkbook Write(string FilePath, ManagerCore Core,int Year, string City)
        {
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("打开模板失败,服务器缺失文件");
            }
            Message(Core,Year, City);
            int StartRow = 0;
            int StartLine=Begin;
            int SerialNumber = 0;
            foreach (var item in DictData.Keys)
            {
                StartRow = Start;
                IRow row = ExcelHelper.OpenRow(ref sheet, StartRow);
                StartRow++;
                ICell cell = ExcelHelper.OpenCell(ref row, StartLine);
                cell.SetCellValue(++SerialNumber);
                row = ExcelHelper.OpenRow(ref sheet, StartRow++);
                cell = ExcelHelper.OpenCell(ref row, StartLine);
                cell.SetCellValue(item);
                List<string> values = DictData[item];
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

        public void Message(ManagerCore Core,int Year,string City)
        {
            List<string> Division = Core.ExcelManager.GetDistrict(City);
            foreach (var item in Division)
            {
                int ID = Core.ExcelManager.GetID(item);
                List<string> values = Core.PeopleManager.Statistics(ID,Year);
                if (!DictData.ContainsKey(item))
                {
                    DictData.Add(item, values);
                }
            }
        }
    }
}