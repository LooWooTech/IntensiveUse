using IntensiveUse.Helper;
using IntensiveUse.Manager;
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
        public IWorkbook Write(string FilePath, ManagerCore Core, string City)
        {
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("服务器缺失相关模板");
            }
            int ID=Core.ExcelManager.GetID(City);
            Message(Core, ID);
            int line = Start;
            foreach (var item in Data)
            {
                IRow row = ExcelHelper.OpenRow(ref sheet,line++);
                ICell cell = ExcelHelper.OpenCell(ref row, Begin);
                cell.SetCellValue(Math.Round(item, 2));
            }
            return workbook;
        }

        public void Message(ManagerCore Core,int ID)
        {
            double[] UII= Core.LandUseManager.GetUII(DateTime.Now.Year, ID);
            foreach (var item in UII)
            {
                Data.Add(item);
            }
            double[] GCI=Core.LandUseManager.GetGCI(DateTime.Now.Year,ID);
            foreach (var item in GCI)
            {
                Data.Add(item);
            }
            double[] EI = Core.LandUseManager.GetEI(DateTime.Now.Year, ID);
            foreach (var item in EI)
            {
                Data.Add(item);
            }
            double[] API = Core.LandUseManager.GetULAPI(DateTime.Now.Year, ID);
            foreach (var item in API)
            {
                Data.Add(item);
            }
        }


    }
}