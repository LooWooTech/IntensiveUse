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
    public class ScheduleATen:ScheduleBase,ISchedule
    {
        public const int Start = 4;
        public const int Line = 11;
        public PEEI Sum { get; set; }
        public ScheduleATen()
        {
            if (Sum == null)
            {
                Sum = new PEEI()
                {
                    PEI = new PEI()
                    {
                        PEI1 = new IIBase()
                    },
                    EEI = new EEI()
                    {
                        EEI1 = new IIBase()
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
                throw new ArgumentException("服务器上的模板文件存在问题");
            }
            IRow row = sheet.GetRow(Start);
            if (row != null)
            {
                TempRow = row;
            }
            this.Year = Year;
            this.CID = Core.ExcelManager.GetID(City);
            Disticts = Core.ExcelManager.GetDistrict(City);
            Message(Core);
            this.Queue= Core.ConstructionLandManager.TranslateOfPEEI(Sum);
            WriteBase(ref sheet, Start);
                
            return workbook;
        }

        private void Ready()
        {
            for (var i = 2; i < Line; i++)
            {
                Columns.Add(i);
            }
        }

        public void Message(ManagerCore Core)
        {
            foreach (var item in Disticts)
            {
                int ID = Core.ExcelManager.GetID(item);
                PEEI peei = Core.ConstructionLandManager.AcquireOfPEEI(Year, ID, CID);
                if (!DictData.ContainsKey(item))
                {
                    DictData.Add(item, Core.ConstructionLandManager.TranslateOfPEEI(peei));
                    Sum += peei;
                }
            }
            if (DictData.Count > 0)
            {
                Sum = Sum / DictData.Count;
            }
        }
    }
}