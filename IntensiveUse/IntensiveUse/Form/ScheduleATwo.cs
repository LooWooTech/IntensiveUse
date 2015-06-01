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
    public class ScheduleATwo:ISchedule
    {
        public Dictionary<int, List<double>> DictData { get; set; }
        public int Start = 1;
        public int Begin = 5;
        public ScheduleATwo()
        {
            if (DictData == null)
            {
                DictData = new Dictionary<int, List<double>>();
            }
        }

        public IWorkbook Write(string FilePath,ManagerCore Core,int Year,string City,string Distict)
        {
            
            int ID = Core.ExcelManager.GetID(City);
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            Message(Core, Year,ID);
            ISheet sheet = workbook.GetSheetAt(0);
            IRow row = sheet.GetRow(Start);
            ICell cell = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            if (cell == null)
            {
                cell = row.CreateCell(2);
            }
            cell.SetCellValue(City);
            if (string.IsNullOrEmpty(Distict))
            {
                cell = row.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                if (cell != null)
                {
                    cell.SetCellValue(Core.ExcelManager.GetStrDistict(City));
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
                
                List<double> values = DictData[item];
                foreach (var entity in values)
                {
                    row = sheet.GetRow(line++);
                    cell = row.GetCell(Begin, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    cell.SetCellValue(Math.Round(entity,2));
                }
                Begin++;
            }
            return workbook;
        }

        public void Message(ManagerCore Core,int Year,int ID)
        {
            for (var i = 4; i >= 0; i--)
            {
                int em = Year - i;
                List<double> Data = new List<double>();
                double[] peoples = Core.PeopleManager.Get(em, ID);
                foreach (var entity in peoples)
                {
                    Data.Add(entity);
                }
                double[] economys = Core.EconmoyManager.Get(em, ID);
                foreach (var entity in economys)
                {
                    Data.Add(entity);
                }
                List<double> landuse = Core.LandUseManager.Get(em, ID);
                foreach (var entity in landuse)
                {
                    Data.Add(entity);
                }
                List<double> landsupply = Core.LandSupplyManager.Get(em, ID);
                foreach (var entity in landsupply)
                {
                    Data.Add(entity);
                }
                DictData.Add(em,Data);
            }
        }

    }
}