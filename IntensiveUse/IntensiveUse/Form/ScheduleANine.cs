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
    public class ScheduleANine:ScheduleBase,ISchedule
    {
        public const int Start = 4;
        public const int Line = 17;
        public PEGCI PEGCI { get; set; }
        public ScheduleANine()
        {
            this.SerialNumber = 15;
            if (PEGCI == null)
            {
                PEGCI = new PEGCI()
                {
                    PGCI = new PGCI()
                    {
                        PGCI1 = new IIBase()
                    },
                    EGCI = new EGCI()
                    {
                        EGCI1 = new IIBase(),
                        EGCI2 = new IIBase(),
                        EGCI3 = new IIBase()
                    }
                };
            }
        }

        public IWorkbook Write(string FilePath, ManagerCore Core, int Year,string City, string Distict,int[] Indexs)
        {
            if (Indexs == null || Indexs.Count() != this.SerialNumber)
            {
                throw new ArgumentException("精度位数据为null或者空，无法进行数据填写");
            }
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("服务器模板文件发生错误，未读取到附表1A9表格");
            }
            TempRow = sheet.GetRow(Start);
            Disticts = Core.ExcelManager.GetDistrict(City);
            this.CID = Core.ExcelManager.GetID(City);
            this.Year = Year;
            Message(Core);
            Ready();
            this.Queue= Core.ConstructionLandManager.TranslateOfPEGCI(PEGCI);
            WriteBase(ref sheet,Start,Indexs);
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
                PEGCI pegci = Core.ConstructionLandManager.AcquireOfPEGCI(Year, ID, CID);
                if (!DictData.ContainsKey(item))
                {
                    DictData.Add(item, Core.ConstructionLandManager.TranslateOfPEGCI(pegci));
                    pegci += PEGCI;
                }
            }
            if (DictData.Count > 0)
            {
                PEGCI = PEGCI / DictData.Count;
            }
        }
    }
}