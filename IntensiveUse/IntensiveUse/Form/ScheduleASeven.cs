using IntensiveUse.Helper;
using IntensiveUse.Manager;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Form
{
    public class ScheduleASeven:ISchedule
    {
        public IWorkbook Write(string FilePath, ManagerCore Core, int Year,string City,string Distict)
        {
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            return workbook;
        }
    }
}