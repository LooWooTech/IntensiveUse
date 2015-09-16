using IntensiveUse.Manager;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntensiveUse.Form
{
    interface ISchedule
    {
        IWorkbook Write(string FilePath, ManagerCore Core,int Year, string City,string Distict,int[] Indexs);
    }
}
