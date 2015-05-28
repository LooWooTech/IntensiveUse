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
        public PEEI Sum { get; set; }
        public ScheduleATen()
        {
            if (DictData == null)
            {
                DictData = new Dictionary<string, Queue<double>>();
            }
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
            Queue<double> queue = Core.ConstructionLandManager.TranslateOfPEEI(Sum);
            row = sheet.GetRow(Start + 1);
            ICell cell = null;
            for (var i = 2; i < 11; i++)
            {
                cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(Math.Round(queue.Dequeue(), 2));
            }
            sheet.ShiftRows(Start + 1, sheet.LastRowNum, DictData.Count - 1);
            int Serial = 0;
            foreach (var item in DictData.Keys)
            {
                row = OpenRow(ref sheet, Serial + Start);
                cell = OpneCell(ref row, 0);
                cell.SetCellValue(++Serial);
                cell = OpneCell(ref row, 1);
                cell.SetCellValue(item);
                for (var i = 2; i < 11; i++)
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