using IntensiveUse.Helper;
using IntensiveUse.Manager;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Form
{
    public class ScheduleBTwo:ISchedule
    {

        public IWorkbook Write(string FilePath, ManagerCore Core,int Year, string City,string Distict)
        {
            int ID = Core.ExcelManager.GetID(City);
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);

            return workbook;
        }

        public void Message(ManagerCore Core, int ID)
        {
            int year = DateTime.Now.Year;
            for (var i = 4; i >= 0; i--)
            {
                string em = (year - i).ToString();
            }
        }
    }

    
}