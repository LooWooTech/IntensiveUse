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
    public class ScheduleAFour:ScheduleBase,ISchedule
    {
        public List<double> CitiesValue { get; set; }
        public int a { get; set; }
        public ScheduleAFour()
        {
            this.Start = 2;
            this.Begin = 2;
            this.SerialNumber = 6;
        }
        public IWorkbook Write(string FilePath, ManagerCore Core,int Year, string City,string Distict,int[] Indexs)
        {
            if (Indexs == null || Indexs.Count() != this.SerialNumber)
            {
                throw new ArgumentException("精度位数据为null或者空，无法进行数据填写！");
            }
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("打开模板失败,服务器缺失文件");
            }
            TempRow = sheet.GetRow(Start);
            this.Year = Year;
            this.a = Core.ExcelManager.GetSuperiorCID(City);
            this.CID = Core.ExcelManager.GetID(City);

            this.City = City;
            this.Disticts = Core.ExcelManager.GetDistrict(City);
            this.Disticts.Add(City);
            Message(Core);
            int Count = DictData.Count;
            IRow row = ExcelHelper.OpenRow(ref sheet,Start+1);
            ICell cell = null;
            int line = Begin;
            //填写上一级行政区 行数据
            int serial = 0;
            foreach (var item in CitiesValue)
            {
                cell = row.GetCell(line++, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(Math.Round(item, Indexs[++serial]));
            }
            //row = ExcelHelper.OpenRow(ref sheet, Start + 1);
            //Queue<double> queue = Core.ConstructionLandManager.TranslateOfLandUseChange(Landusechange);
            //line = Begin;
            ////填写行政辖区整体  行数据
            //serial = 0;
            //foreach (var item in queue)
            //{
            //    cell = row.GetCell(line++, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            //    cell.SetCellValue(Math.Round(item, Indexs[serial++]));
            //}
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
                serial = 0;
                foreach (var val in Values)
                {
                    cell = OpenCell(ref row, line++);
                    cell.SetCellValue(Math.Round(val, Indexs[serial++]));
                }
                cell = OpenCell(ref row, line++);
            }
            sheet.AddMergedRegion(new CellRangeAddress(Start + SerialNumber-1, Start + SerialNumber-1, 0, 1));
            row.GetCell(0).SetCellValue("行政辖区整体");
            return workbook;
        }

        public void Message(ManagerCore Core)
        {
            //浙江省数据
            Situation[] Cities = Core.EconmoyManager.Find(Year,a);
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
            Situation[] Dcities = Core.EconmoyManager.Find(Year, CID);
            foreach (var item in Disticts)
            {
                int ID = Core.ExcelManager.GetID(item);
                LandUseChange one;
                if (item == City)
                {
                    one = Core.EconmoyManager.Gain(Year, ID, Cities);
                }
                else
                {
                    one= Core.EconmoyManager.Gain(Year, ID, Dcities);
                }
                Queue<double> queue = Core.ConstructionLandManager.TranslateOfLandUseChange(one);
                if (!DictData.ContainsKey(item))
                {
                    DictData.Add(item, queue);
                }
            }
        }
    }
}