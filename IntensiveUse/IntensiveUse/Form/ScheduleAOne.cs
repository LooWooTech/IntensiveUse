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
    public class ScheduleAOne:ISchedule
    {
        public const int Start = 35;
        public const int Begin = 5;
        public Dictionary<string, List<double>> DictData { get; set; }
        public ScheduleAOne()
        {
            if (DictData == null)
            {
                DictData = new Dictionary<string, List<double>>();
            }
        }
        public IWorkbook Write(string FilePath, ManagerCore Core, int Year, string City,string Distict)
        {
            if (string.IsNullOrEmpty(Distict))
            {
                throw new ArgumentException("请选择地级市下面的所辖区");
            }
            ScheduleATwo work = new ScheduleATwo();
            IWorkbook workbook = work.Write(FilePath, Core, Year, Distict,Distict);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("未读取模板文件中的sheet");
            }
            int RID=Core.ExcelManager.GetID(City);
            Message(Core,RID,Year);
            IRow row = null;
            ICell cell = null;
            int m=Begin;
            foreach (var item in DictData.Keys)
            {
                int line = Start;
                foreach (var val in DictData[item])
                {
                    row = sheet.GetRow(line);
                    if (row == null)
                    {
                        row = sheet.CreateRow(line);
                    }
                    line++;
                    cell = row.GetCell(m, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    if (cell == null)
                    {
                        cell = row.CreateCell(m);
                    }
                    cell.SetCellValue(Math.Round(val, 2));
                }
                m++;
                
            }
            return workbook;
        }


        public void Message(ManagerCore Core,int ID,int Year)
        {
            for (var i = 4; i >= 0; i--)
            {
                string em=(Year-i).ToString();
                Economy economy = Core.EconmoyManager.SearchForEconomy(em, ID);
                ConstructionLand construction = Core.EconmoyManager.SearchForConstruction(em, ID);
                if (!DictData.ContainsKey(em))
                {
                    DictData.Add(em, new List<double>(){
                        economy.Current,
                        economy.Compare,
                        construction.SubTotal
                    });
                }
            }
        }
    }
}