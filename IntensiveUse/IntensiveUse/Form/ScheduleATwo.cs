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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">模板文件</param>
        /// <param name="core"></param>
        /// <param name="year">年份</param>
        /// <param name="indexs">精度数组</param>
        /// <param name="belongCity">地级市  市名</param>
        /// <param name="name">地级市以下 所辖区 </param>
        /// <returns></returns>
        public IWorkbook WriteBase(string filePath,ManagerCore core,int[] indexs,string belongCity,string name)
        {
            if (indexs.Count() < this.SerialNumber)
            {
                throw new ArgumentException("精度位发生错误，不能小于32！");
            }
            IWorkbook workbook = ExcelHelper.OpenWorkbook(filePath);
            Message(core);
            ISheet sheet = workbook.GetSheetAt(0);
            IRow row = sheet.GetRow(Start);
            ICell cell = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            if (cell == null)
            {
                cell = row.CreateCell(2);
            }
            cell.SetCellValue(belongCity);//设置城市名
            if (!string.IsNullOrEmpty(name))//地级市下载附表1A-2的时候，是细分下面每个区县
            {
                cell = row.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                if (cell != null)
                {
                    cell.SetCellValue(name);
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
                cell.SetCellValue(item + "年");

                var values = DictData[item];
                int Serial = 0;
                foreach (var entity in values)
                {
                    row = sheet.GetRow(line++);
                    cell = row.GetCell(Begin, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                    cell.SetCellValue(Math.Round(entity, indexs[Serial++]));
                }
                Begin++;
            }
            return workbook;
        }

        /// <summary>
        /// 适用于区域评价
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="Core"></param>
        /// <param name="Year"></param>
        /// <param name="City"></param>
        /// <param name="Distict"></param>
        /// <param name="Indexs"></param>
        /// <returns></returns>
        public IWorkbook Write(string FilePath,ManagerCore Core,int Year,string City,string Distict,int[] Indexs)
        {
            if (string.IsNullOrEmpty(Distict))
            {
                this.CID = Core.ExcelManager.GetID(City);
            }
            else
            {
                this.CID = Core.ExcelManager.GetID(Distict);
            }
            this.Year = Year;
            #region 2016.3.18  集成全国注释
            //if (Indexs.Count() < this.SerialNumber)
            //{
            //    throw new ArgumentException("精度位发生错误，不能小于32！");
            //}

            //IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            //Message(Core);
            //ISheet sheet = workbook.GetSheetAt(0);
            //IRow row = sheet.GetRow(Start);
            //ICell cell = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            //if (cell == null)
            //{
            //    cell = row.CreateCell(2);
            //}
            //cell.SetCellValue(City);//设置城市名
            //if (!string.IsNullOrEmpty(Distict))//地级市下载附表1A-2的时候，是细分下面每个区县
            //{
            //    cell = row.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            //    if (cell != null)
            //    {
            //        cell.SetCellValue(Distict);
            //    }
            //}

            //foreach (var item in DictData.Keys)
            //{
            //    int line = Start + 1;
            //    row = sheet.GetRow(line++);
            //    cell = row.GetCell(Begin, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            //    if (cell == null)
            //    {
            //        cell = row.CreateCell(Begin);
            //    }
            //    cell.SetCellValue(item+"年");

            //    var values = DictData[item];
            //    int Serial = 0;
            //    foreach (var entity in values)
            //    {
            //        row = sheet.GetRow(line++);
            //        cell = row.GetCell(Begin, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            //        cell.SetCellValue(Math.Round(entity,Indexs[Serial++]));
            //    }
            //    Begin++;
            //}
            //return workbook;
            #endregion
            return WriteBase(FilePath, Core, Indexs, City, Distict);
        }

        /// <summary>
        /// 适用于全国汇总
        /// </summary>
        /// <param name="filePath">模板文件</param>
        /// <param name="core"></param>
        /// <param name="year">年份</param>
        /// <param name="province">省份</param>
        /// <param name="belongCity">所属城市</param>
        /// <param name="name">区域名称</param>
        /// <param name="indexs">精度位</param>
        /// <returns></returns>
        public IWorkbook AWrite(string filePath,ManagerCore core,int year,string province,string belongCity,string name,int[] indexs)
        {
            this.CID = core.RegionManager.GetID(province, belongCity, name);
            this.Year = year;
            return WriteBase(filePath, core, indexs, belongCity, name);
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