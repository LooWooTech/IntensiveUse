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
    public class ScheduleAEleven:ScheduleBase,ISchedule
    {
        public const int Start = 4;
        public const int Line = 10;
        public APIUL sum { get; set; }
        public ScheduleAEleven()
        {
            if (sum == null)
            {
                sum = new APIUL()
                {
                    ULAPI = new ULAPI()
                    {
                        ULAPI1 = new IIBase(),
                        ULAPI2 = new IIBase()
                    }
                };
            }
        }
        public IWorkbook Write(string FilePath, ManagerCore Core, int Year, string City, string Distict)
        {
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("打开服务器上的模板文件失败");
            }
            TempRow = sheet.GetRow(Start);
            Disticts = Core.ExcelManager.GetDistrict(City);
            CID = Core.ExcelManager.GetID(City);
            this.Year = Year;
            Message(Core);
            Queue<double> queue = Core.ConstructionLandManager.TranslateOfAPIUL(sum);
            WriteBase(ref sheet, Line, Start, queue);
            return workbook;
        }

        public void Message(ManagerCore Core)
        {
            foreach (var item in Disticts)
            {
                int ID = Core.ExcelManager.GetID(item);
                APIUL apiul = Core.ConstructionLandManager.AcquireOfAPIUL(Year, ID, CID);
                if (!DictData.ContainsKey(item))
                {
                    DictData.Add(item, Core.ConstructionLandManager.TranslateOfAPIUL(apiul));
                    sum += apiul;
                }
            }
            if (DictData.Count > 0)
            {
                sum = sum / DictData.Count;
            }
        }
    }
}