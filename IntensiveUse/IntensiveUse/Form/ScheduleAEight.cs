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
    public class ScheduleAEight:ScheduleBase,ISchedule
    {
        public const int Start = 4;
        public const int Begin = 2;
        public const int Line = 14;
        public PEUII Sum { get; set; }
        public ScheduleAEight()
        {
            this.SerialNumber = 12;
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
        public IWorkbook WriteBase(string filePath,ManagerCore core,int[] indexs)
        {
            if (indexs == null || indexs.Count() != this.SerialNumber)
            {
                throw new ArgumentException("精度位数据为null或者空，无法进行数据填写");
            }

            IWorkbook workbook = ExcelHelper.OpenWorkbook(filePath);
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

            Ready();
            Message(core);
            this.Queue = core.ConstructionLandManager.TranslateOfPEUII(Sum);
            WriteBase(ref sheet, Start, indexs);
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
        public IWorkbook Write(string FilePath, ManagerCore Core, int Year, string City, string Distict,int[] Indexs)
        {
            //if (Indexs == null || Indexs.Count() != this.SerialNumber)
            //{
            //    throw new ArgumentException("精度位数据为null或者空，无法进行数据填写");
            //}

            Disticts = Core.ExcelManager.GetDistrict(City);
            this.Year = Year;
            this.CID = Core.ExcelManager.GetID(City);

            return WriteBase(FilePath, Core, Indexs);

            //IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            //ISheet sheet = workbook.GetSheetAt(0);
            //if (sheet == null)
            //{
            //    throw new ArgumentException("未读取到服务器上的模板文件，请联系相关人员");
            //}
            //IRow row = sheet.GetRow(Start);
            //if (row != null)
            //{
            //    TempRow = row;
            //}
            
            //Ready();
            //Message(Core);
            //this.Queue= Core.ConstructionLandManager.TranslateOfPEUII(Sum);
            //WriteBase(ref sheet,Start,Indexs);
            //return workbook;
        }
        /// <summary>
        /// 适用于 全国汇总
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="core"></param>
        /// <param name="year"></param>
        /// <param name="province"></param>
        /// <param name="belongCity"></param>
        /// <param name="distict"></param>
        /// <param name="indexs"></param>
        /// <returns></returns>
        public IWorkbook AWrite(string filePath,ManagerCore core,int year,string province,string belongCity,string distict,int[] indexs)
        {
            Disticts = core.RegionManager.GetCounty(province, belongCity);
            this.Year = year;
            this.CID = core.RegionManager.GetID(province, belongCity);

            return WriteBase(filePath, core, indexs);
        }
        private void Ready()
        {
            for (var i = 2; i < Line; i++)
            {
                Columns.Add(i);
            }
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