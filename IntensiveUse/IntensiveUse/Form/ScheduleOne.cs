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
    public class ScheduleOne
    {
        public Dictionary<string, List<double>> DictData { get; set; }
        public int Start = 1;
        public int Begin = 5;
        public ScheduleOne()
        {
            if (DictData == null)
            {
                DictData = new Dictionary<string, List<double>>();
            }
        }

        public IWorkbook Write(string FilePath,ManagerCore Core,List<string> Years,string City)
        {
            int ID = Core.ExcelManager.GetID(City);
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            foreach (var item in Years)
            {
                List<double> Data = new List<double>();
                double[] peoples = Core.PeopleManager.Get(item, ID);
                foreach (var entity in peoples)
                {
                    Data.Add(entity);
                }
                double[] economys = Core.EconmoyManager.Get(item,ID);
                foreach (var entity in economys)
                {
                    Data.Add(entity);
                }
                List<double> landuse = Core.LandUseManager.Get(item,ID);
                foreach (var entity in landuse)
                {
                    Data.Add(entity);
                }
                List<double> landsupply = Core.LandSupplyManager.Get(item,ID);
                foreach (var entity in landsupply)
                {
                    Data.Add(entity);
                }
                DictData.Add(item, Data);
            }
            ISheet sheet = workbook.GetSheetAt(0);
            IRow row = sheet.GetRow(Start);
            ICell cell = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            if (cell == null)
            {
                cell = row.CreateCell(2);
            }
            cell.SetCellValue(City);
            return workbook;
        }

        public void Message(ManagerCore Core)
        {

        }

    }
}