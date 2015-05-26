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
    public class ScheduleASix:ISchedule
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

        public Exponent Read(string FilePath)
        {
            Queue<double> queue = new Queue<double>();
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("表格中没有sheet，请核对表格");
            }
            IRow row = null;
            ICell cell = null;
            string value =string.Empty;
            int Count = sheet.LastRowNum;
            for (var i = Start; i <=Count; i++)
            {
                row = sheet.GetRow(i);
                if (row == null)
                {
                    break;
                }
                cell = row.GetCell(Begin + 1, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                double m = 0.0;
                value = cell.ToString().Replace("%", "").Trim();
                double.TryParse(value, out m);
                queue.Enqueue(m);
            }
            Exponent exponent = new Exponent();
            System.Reflection.PropertyInfo[] propList = typeof(Exponent).GetProperties();
            foreach (var item in propList)
            {
                if (item.PropertyType.Equals(typeof(double)))
                {
                    item.SetValue(exponent, queue.Dequeue(), null);
                }
            }

            return exponent;
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


    }
}