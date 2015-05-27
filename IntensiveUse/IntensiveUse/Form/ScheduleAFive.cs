using IntensiveUse.Helper;
using IntensiveUse.Manager;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Form
{
    public class ScheduleAFive:ISchedule,IRead
    {
        public void Read(string FilePath,ManagerCore Core,string City,int Year)
        {
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);

        }

        public IWorkbook Write(string FilePath, ManagerCore Core,int Year, string City)
        {
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            return workbook;
        }


        public void Gain(IWorkbook workbook)
        {

        }
    }
}