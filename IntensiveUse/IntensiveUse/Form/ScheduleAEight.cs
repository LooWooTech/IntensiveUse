﻿using IntensiveUse.Helper;
using IntensiveUse.Manager;
using IntensiveUse.Models;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Form
{
    public class ScheduleAEight:ScheduleBase,ISchedule
    {
        public const int Start = 4;
        public const int Begin = 2;
        public PEUII Sum { get; set; }
        public ScheduleAEight()
        {
            if (DictData == null)
            {
                DictData = new Dictionary<string, Queue<double>>();
            }
            if (Sum == null)
            {
                Sum = new PEUII()
                {
                    PUII = new PUII()
                    {
                        II = new IIBase()
                    },
                    EUII = new EUII()
                    {
                        EUII1 = new IIBase(),
                        EUII2 = new IIBase()
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
                throw new ArgumentException("未读取到服务器上的模板文件，请联系相关人员");
            }
            IRow row = sheet.GetRow(Start);
            if (row != null)
            {
                TempRow = row;
            }
            Disticts = Core.ExcelManager.GetDistrict(City);
            this.Year = Year;
            this.CID = Core.ExcelManager.GetID(City);
            Message(Core);

            Queue<double> all = Core.ConstructionLandManager.TranslateOfPEUII(Sum);
            row = sheet.GetRow(Start + 1);
            ICell cell = null;
            for (var i = Begin; i < 14; i++)
            {
                cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                cell.SetCellValue(Math.Round(all.Dequeue(),2));
            }
            sheet.ShiftRows(5, sheet.LastRowNum, DictData.Count-1);
            int Serial = 0;
            foreach (var item in DictData.Keys)
            {
                row = sheet.GetRow(Serial+Start);
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
                for (var i = 2; i < 14; i++)
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
                PEUII peuii = Core.ConstructionLandManager.AcquireOfPEUII(Year, ID,CID);
                if (!DictData.ContainsKey(item))
                {
                    DictData.Add(item, Core.ConstructionLandManager.TranslateOfPEUII(peuii));
                    Sum += peuii;
                }
            }
            if (DictData.Count > 0)
            {
                Sum = Sum / (DictData.Count);
            }
            
        }
    }
}