using IntensiveUse.Helper;
using IntensiveUse.Manager;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Form
{
    public class ScheduleAEight:ISchedule
    {
        public const int Start = 4;
        public IRow TempRow { get; set; }
        public int Year { get; set; }
        public int ID { get; set; }
        public List<string> Disticts { get; set; }
        public Dictionary<string, Queue<double>> DictData { get; set; }
        public ScheduleAEight()
        {
            if (DictData == null)
            {
                DictData = new Dictionary<string, Queue<double>>();
            }
        }

        public IWorkbook Write(string FilePath, ManagerCore Core, int Year, string City, string Distict)
        {
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("未读取到服务器上的模板文件，请联系相关人员");
            }
            IRow row = sheet.GetRow(Start);
            if (row != null)
            {
                TempRow = row;
            }
            Disticts = Core.ExcelManager.GetDistrict(City);
            this.Year = Year;
            this.ID = Core.ExcelManager.GetID(City);
            return workbook;
        }

        public void Message(ManagerCore Core)
        {
            foreach (var item in Disticts)
            {

            }
        }
    }
}