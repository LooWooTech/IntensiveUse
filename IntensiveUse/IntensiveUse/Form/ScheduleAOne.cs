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
            this.SerialNumber = 35;
        }
        public IWorkbook Write(string FilePath, ManagerCore Core, int Year, string City,string Distict,int[] Indexs)
        {
            if (Indexs.Count() != this.SerialNumber)
            {
                throw new ArgumentException("提取数据精度位失败！无法进行生成表格");
            }
            ScheduleATwo work = new ScheduleATwo();
            IWorkbook workbook = work.Write(FilePath, Core, Year, City,Distict,Indexs);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("未读取模板文件中的sheet");
            }
            if (string.IsNullOrEmpty(Distict))
            {
                this.CID = Core.ExcelManager.GetSuperiorCID(City);
            }
            else
            {
                this.CID = Core.ExcelManager.GetSuperiorDID(Distict);
            }
            
            this.Year = Year;
            Message(Core);
            IRow row = null;
            ICell cell = null;
            int m=Begin;
            foreach (var item in DictData.Keys)
            {
                int line = Start;
                int serial = 32;
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
                    cell.SetCellValue(Math.Round(val, Indexs[serial++]));
                }
                m++;
                
            }
            return workbook;
        }

        public IWorkbook AWrite(string filePath,ManagerCore core,int year,string province,string belongCity,string name,int[] indexs)
        {
            ScheduleATwo work = new ScheduleATwo();
            IWorkbook workbook = work.AWrite(filePath, core, year, province, belongCity, name, indexs);
            var sheet=workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("未读取Sheet");
            }
            this.CID = core.RegionManager.GetID(province, belongCity, name);
            this.Year = year;
            Message(core, true, !string.IsNullOrEmpty(name));
            IRow row = null;
            ICell cell = null;
            int m = Begin;
            foreach (var item in DictData.Keys)
            {
                int line = Start;
                int serial = 32;
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
                    cell.SetCellValue(Math.Round(val, indexs[serial++]));
                }
                m++;

            }
            return workbook;
            //return workbook;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Core"></param>
        /// <param name="flag">false: 区域评价  true：全国汇总</param>
        /// <param name="pbFlag">false: 省份  true:地级市</param>
        public void Message(ManagerCore Core,bool flag=false,bool pbFlag=false)
        {
            for (var i = 4; i >= 0; i--)
            {
                int em=(Year-i);
                Queue<double> queue = new Queue<double>();
                if (flag)
                {
                    var superior = Core.SuperiorManager.SearchForSuperior(em, CID);
                    queue.Enqueue(pbFlag?superior.CityCurrent:superior.ProvinceCurrent);
                    queue.Enqueue(pbFlag ? superior.CityCompare : superior.ProvinceCompare);
                    queue.Enqueue(pbFlag ? superior.CityConstruction : superior.ProvinceConstruction);
                }
                else
                {
                    Economy economy = Core.EconmoyManager.SearchForEconomy(em, CID);
                    ConstructionLand construction = Core.EconmoyManager.SearchForConstruction(em, CID);
                    queue.Enqueue(economy.Current);
                    queue.Enqueue(economy.Compare);
                    queue.Enqueue(construction.SubTotal);
                }
                
                if (!DictData.ContainsKey(em.ToString()))
                {
                    DictData.Add(em.ToString(), queue);
                }
            }
        }
        
    }
}