using IntensiveUse.Helper;
using IntensiveUse.Manager;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Form
{
    public class ScheduleANine:ISchedule
    {
        public const int Start = 4;
        public List<string> Disticts { get; set; }
        public Dictionary<string, Queue<double>> DictData { get; set; }
        public ScheduleANine()
        {
            if (DictData == null)
            {
                DictData = new Dictionary<string, Queue<double>>();
            }
        }

        public IWorkbook Write(string FilePath, ManagerCore Core, int Year,string City, string Distict)
        {
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            Disticts = Core.ExcelManager.GetDistrict(City);
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