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
    public class ScheduleASix:ISchedule,IRead
    {
        public const int Start = 2;
        public const int Begin = 4;
        public List<double> Data { get; set; }
        public ScheduleASix()
        {
            if (Data == null)
            {
                Data = new List<double>();
            }
        }

        public void Read(string FilePath,ManagerCore Core,string City,int Year)
        {
            
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            Exponent exponent = Gain(workbook,Core);
            if (exponent == null)
            {
                throw new ArgumentException("未获取到相关理想数据");
            }
            exponent.Year = Year.ToString();
            exponent.RID = Core.ExcelManager.GetID(City);
            exponent.Type = IdealType.Value;
            Core.ExponentManager.Save(exponent);
        }
        public IWorkbook Write(string FilePath, ManagerCore Core, int Year,string City)
        {
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("服务器缺失相关模板");
            }
            int ID=Core.ExcelManager.GetID(City);
            Message(Core,Year, ID);
            int line = Start;
            foreach (var item in Data)
            {
                IRow row = ExcelHelper.OpenRow(ref sheet,line++);
                ICell cell = ExcelHelper.OpenCell(ref row, Begin);
                cell.SetCellValue(Math.Round(item, 2));
            }
            return workbook;
        }

        public void Message(ManagerCore Core,int Year,int ID)
        {
            double[] UII= Core.LandUseManager.GetUII(Year, ID);
            foreach (var item in UII)
            {
                Data.Add(item);
            }
            double[] GCI=Core.LandUseManager.GetGCI(Year,ID);
            foreach (var item in GCI)
            {
                Data.Add(item);
            }
            double[] EI = Core.LandUseManager.GetEI(Year, ID);
            foreach (var item in EI)
            {
                Data.Add(item);
            }
            double[] API = Core.LandUseManager.GetULAPI(Year, ID);
            foreach (var item in API)
            {
                Data.Add(item);
            }
        }


        public Exponent Gain(IWorkbook workbook,ManagerCore Core)
        {
            Queue<double> queue = new Queue<double>();
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("表格中没有sheet，请核对表格");
            }
            return Core.IndexManager.GainExponent(sheet,Begin+1,Start);
        }


    }
}