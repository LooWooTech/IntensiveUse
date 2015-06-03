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
    public class ScheduleAOne:ScheduleBase,ISchedule
    {
        public ScheduleAOne()
        {
            this.Start = 35;
            this.Begin = 5;
        }
        public IWorkbook Write(string FilePath, ManagerCore Core, int Year, string City,string Distict)
        {
            //if (string.IsNullOrEmpty(Distict))
            //{
            //    throw new ArgumentException("请选择地级市下面的所辖区");
            //}
            ScheduleATwo work = new ScheduleATwo();
            IWorkbook workbook = work.Write(FilePath, Core, Year, City,Distict);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("未读取模板文件中的sheet");
            }
            this.CID=Core.ExcelManager.GetID(City);
            this.Year = Year;
            Message(Core);
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


        public void Message(ManagerCore Core)
        {
            for (var i = 4; i >= 0; i--)
            {
                int em=(Year-i);
                Economy economy = Core.EconmoyManager.SearchForEconomy(em, CID);
                ConstructionLand construction = Core.EconmoyManager.SearchForConstruction(em, CID);
                Queue<double> queue=new Queue<double>();
                queue.Enqueue(economy.Current);
                queue.Enqueue(economy.Compare);
                queue.Enqueue(construction.SubTotal);
                if (!DictData.ContainsKey(em.ToString()))
                {
                    DictData.Add(em.ToString(), queue);
                }
            }
        }
    }
}