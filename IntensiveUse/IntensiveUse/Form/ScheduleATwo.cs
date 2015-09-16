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
    public class ScheduleATwo:ScheduleBase,ISchedule
    {
        public ScheduleATwo()
        {
            this.Start = 1;
            this.Begin = 5;
            this.SerialNumber = 32;
        }

        public IWorkbook Write(string FilePath,ManagerCore Core,int Year,string City,string Distict,int[] Indexs)
        {
            if (Indexs.Count() < this.SerialNumber)
            {
                throw new ArgumentException("精度位发生错误，不能小于32！");
            }
            if (string.IsNullOrEmpty(Distict))
            {
                this.CID = Core.ExcelManager.GetID(City);
            }
            else
            {
                this.CID = Core.ExcelManager.GetID(Distict);
            }
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            this.Year = Year;
            
            Message(Core);
            ISheet sheet = workbook.GetSheetAt(0);
            IRow row = sheet.GetRow(Start);
            ICell cell = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            if (cell == null)
            {
                cell = row.CreateCell(2);
            }
            cell.SetCellValue(City);
            if (!string.IsNullOrEmpty(Distict))
            {
                cell = row.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                if (cell != null)
                {
                    cell.SetCellValue(Distict);
                }
            }

            foreach (var item in DictData.Keys)
            {
                int line = Start + 1;
                row = sheet.GetRow(line++);
                cell = row.GetCell(Begin, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                if (cell == null)
                {
                    cell = row.CreateCell(Begin);
                }
                cell.SetCellValue(item+"年");
                
                var values = DictData[item];
                int Serial = 0;
                foreach (var entity in values)
                {
                    row = sheet.GetRow(line++);
                    cell = row.GetCell(Begin, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    cell.SetCellValue(Math.Round(entity,Indexs[Serial++]));
                }
                Begin++;
            }
            return workbook;
        }

        public void Message(ManagerCore Core)
        {
            for (var i = 4; i >= 0; i--)
            {
                int em = Year - i;
                Queue<double> Data = new Queue<double>();
                double[] peoples = Core.PeopleManager.Get(em, CID);
                foreach (var entity in peoples)
                {
                    Data.Enqueue(entity);
                }
                double[] economys = Core.EconmoyManager.Get(em, CID);
                foreach (var entity in economys)
                {
                    Data.Enqueue(entity);
                }
                List<double> landuse = Core.LandUseManager.Get(em, CID);
                foreach (var entity in landuse)
                {
                    Data.Enqueue(entity);
                }
                List<double> landsupply = Core.LandSupplyManager.Get(em, CID);
                foreach (var entity in landsupply)
                {
                    Data.Enqueue(entity);
                }
                DictData.Add(em.ToString(),Data);
            }
        }

    }
}