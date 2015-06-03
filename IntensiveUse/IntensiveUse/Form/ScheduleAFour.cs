using IntensiveUse.Helper;
using IntensiveUse.Manager;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Form
{
    public class ScheduleAFour:ScheduleBase,ISchedule
    {
        public List<double> CitiesValue { get; set; }
        public LandUseChange Landusechange { get; set; }
        public ScheduleAFour()
        {
            this.Start = 2;
            this.Begin = 2;
            if (Landusechange == null)
            {
                Landusechange = new LandUseChange()
                {
                    ESituation = new Situation(),
                    CSituation = new Situation(),
                };
            }
        }
        public IWorkbook Write(string FilePath, ManagerCore Core,int Year, string City,string Distict)
        {
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("打开模板失败,服务器缺失文件");
            }
            TempRow = sheet.GetRow(Start);
            this.Year = Year;
            this.CID = Core.ExcelManager.GetID(City);
            this.City = City;
            this.Disticts = Core.ExcelManager.GetDistrict(City);
            Message(Core);
            int Count = DictData.Count;
            IRow row = ExcelHelper.OpenRow(ref sheet,Start+2);
            ICell cell = null;
            int line = Begin;
            foreach (var item in CitiesValue)
            {
                cell = row.GetCell(line++, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(Math.Round(item, 2));
            }
            row = ExcelHelper.OpenRow(ref sheet, Start + 1);
            Queue<double> queue = Core.ConstructionLandManager.TranslateOfLandUseChange(Landusechange);
            line = Begin;
            foreach (var item in queue)
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
                var Values = DictData[item];
                foreach (var val in Values)
                {
                    cell = OpenCell(ref row, line++);
                    cell.SetCellValue(Math.Round(val, 2));
                }
                cell = OpenCell(ref row, line++);

            }
            return workbook;
        }

        public void Message(ManagerCore Core)
        {
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
            foreach (var item in Disticts)
            {
                int ID = Core.ExcelManager.GetID(item);
                var one= Core.EconmoyManager.Gain(Year, ID, Cities);
                Landusechange += one;
                Queue<double> queue = Core.ConstructionLandManager.TranslateOfLandUseChange(one);
                if (!DictData.ContainsKey(item))
                {
                    DictData.Add(item, queue);
                }
            }

            if (DictData.Count > 0)
            {
                Landusechange = Landusechange / DictData.Count;
            }
        }
    }
}