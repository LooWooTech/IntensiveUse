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
        public PEGCI PEGCI { get; set; }
        public ScheduleANine()
        {
            if (DictData == null)
            {
                DictData = new Dictionary<string, Queue<double>>();
            }
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

        public IWorkbook Write(string FilePath, ManagerCore Core, int Year,string City, string Distict)
        {
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
            Queue<double> queue = Core.ConstructionLandManager.TranslateOfPEGCI(PEGCI);
            IRow row = sheet.GetRow(5);
            ICell cell = null;
            for (var i = 2; i < 17; i++)
            {
                cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(Math.Round(queue.Dequeue(), 2));
            }
            sheet.ShiftRows(5, sheet.LastRowNum, DictData.Count - 1);
            int Serial = 0;
            foreach (var item in DictData.Keys)
            {
                row = sheet.GetRow(Serial + Start);
                if (row == null)
                {
                    row = sheet.CreateRow(Serial + Start);
                    if (TempRow.RowStyle != null)
                    {
                        row.RowStyle = TempRow.RowStyle;
                    }
                }
                cell = OpneCell(ref row, 0);
                cell.SetCellValue(++Serial);
                cell = OpneCell(ref row, 1);
                cell.SetCellValue(item);
                for (var i = 2; i < 17; i++)
                {
                    cell = OpneCell(ref row, i);
                    cell.SetCellValue(Math.Round(DictData[item].Dequeue(), 2));
                }

            }
                
            return workbook;
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