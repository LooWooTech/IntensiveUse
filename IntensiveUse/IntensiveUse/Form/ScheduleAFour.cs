using IntensiveUse.Helper;
using IntensiveUse.Manager;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Form
{
    public class ScheduleAFour:ISchedule
    {
        public const int Start = 3;
        public const int Begin = 2;
        public Dictionary<string, List<double>> DictData { get; set; }
        public List<double> CitiesValue { get; set; }
        public IRow TempRow { get; set; }
        public ScheduleAFour()
        {
            if (DictData == null)
            {
                DictData = new Dictionary<string, List<double>>();
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
            TempRow = sheet.GetRow(Start);
            Message(Core,Year, City);
            int Count = DictData.Count;
            IRow row = ExcelHelper.OpenRow(ref sheet,Start+2);
            ICell cell = null;
            int line = Begin;
            foreach (var item in CitiesValue)
            {
                cell = row.GetCell(line++, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(Math.Round(item, 2));
            }
            sheet.ShiftRows(Start + 1, sheet.LastRowNum, DictData.Count-1);
            int SerialNumber = 0;
            foreach (var item in DictData.Keys)
            {
                line = 0;
                row = OpenRow(ref sheet, Start + SerialNumber);
                cell = OpenCell(ref row, line++);
                cell.SetCellValue(++SerialNumber);
                cell = OpenCell(ref row, line++);
                cell.SetCellValue(item);
                List<double> Values = DictData[item];
                foreach (var val in Values)
                {
                    cell = OpenCell(ref row, line++);
                    cell.SetCellValue(Math.Round(val, 2));
                }
                cell = OpenCell(ref row, line++);

            }
            return workbook;
        }

        private IRow OpenRow(ref ISheet Sheet, int ID)
        {
            IRow row = Sheet.GetRow(ID);
            if (row == null)
            {
                row = Sheet.CreateRow(ID);
                if (TempRow.RowStyle != null)
                {
                    row.RowStyle = TempRow.RowStyle;
                }
                
            }
            return row;
        }

        private ICell OpenCell(ref IRow Row, int ID)
        {
            ICell cell = Row.GetCell(ID, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            if (cell == null)
            {
                cell = Row.CreateCell(ID, TempRow.GetCell(ID).CellType);
            }
            if (TempRow.GetCell(ID).CellStyle != null)
            {
                cell.CellStyle = TempRow.GetCell(ID).CellStyle;
            }
            
            
            return cell;
        }

        public void Message(ManagerCore Core,int Year,string City)
        {
            List<string> Division = Core.ExcelManager.GetDistrict(City);
            int CID=Core.ExcelManager.GetID(City);
            Situation[] Cities = Core.EconmoyManager.Find(Year, CID);
            if (Cities == null || Cities.Count() != 2)
            {
                throw new ArgumentException("未读取到上一级数据");
            }
            double m=0.0;
            if(Math.Abs(Cities[1].Extent-0)>0.001){
                m=Cities[0].Extent/Cities[1].Extent;
            }
            CitiesValue = new List<double>(){
                Cities[0].Increment,
                Cities[0].Extent,
                Cities[1].Increment,
                Cities[1].Extent,
                m
            };
            foreach (var item in Division)
            {
                int ID = Core.ExcelManager.GetID(item);
                List<double> values = Core.EconmoyManager.Gain(Year, ID, Cities);
                if (!DictData.ContainsKey(item))
                {
                    DictData.Add(item, values);
                }
            }
        }
    }
}