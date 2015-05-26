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
        public const string Name = "附表1A-6地级以上城市（县级市）区域用地状况定量评价指标理想值汇总表";
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
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("表格中没有sheet，请核对表格");
            }
            IRow row = sheet.GetRow(0);
            ICell cell = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            if (cell == null)
            {
                throw new ArgumentException("未读取到"+Name+"相关数据");
            }
            string value = cell.ToString();
            if (value != Name)
            {
                throw new ArgumentException("请核对上传的表格");
            }
            int Count = sheet.LastRowNum;
            for (var i = Start; i < Count; i++)
            {
                row = sheet.GetRow(i);
                if (row == null)
                {
                    break;
                }
                cell = row.GetCell(Start + 1, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                double m = 0.0;
                value = cell.ToString().Replace("%", "").Trim();
                double.TryParse(value, out m);
                Data.Add(m);
            }

            return new Exponent();
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