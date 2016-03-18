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
    public class ScheduleAEleven:ScheduleBase,ISchedule
    {
        public const int Start = 4;
        public const int Line = 10;
        public APIUL sum { get; set; }
        public ScheduleAEleven()
        {
            this.SerialNumber = 8;
            if (sum == null)
            {
                sum = new APIUL()
                {
                    ULAPI = new ULAPI()
                    {
                        ULAPI1 = new IIBase(),
                        ULAPI2 = new IIBase()
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
                throw new ArgumentException("打开服务器上的模板文件失败");
            }
            TempRow = sheet.GetRow(Start);
            Message(core);
            Ready();
            this.Queue = core.ConstructionLandManager.TranslateOfAPIUL(sum);
            WriteBase(ref sheet, Start, indexs);
            return workbook;
        }
        /// <summary>
        /// 适用于 区域评价
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
            //IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            //ISheet sheet = workbook.GetSheetAt(0);
            //if (sheet == null)
            //{
            //    throw new ArgumentException("打开服务器上的模板文件失败");
            //}
            //TempRow = sheet.GetRow(Start);
            Disticts = Core.ExcelManager.GetDistrict(City);
            CID = Core.ExcelManager.GetID(City);
            this.Year = Year;
            return WriteBase(FilePath, Core, Indexs);

            //Message(Core);
            //Ready();
            //this.Queue= Core.ConstructionLandManager.TranslateOfAPIUL(sum);
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
                APIUL apiul = Core.ConstructionLandManager.AcquireOfAPIUL(Year, ID, CID);
                if (!DictData.ContainsKey(item))
                {
                    DictData.Add(item, Core.ConstructionLandManager.TranslateOfAPIUL(apiul));
                    sum += apiul;
                }
            }
            if (DictData.Count > 0)
            {
                sum = sum / DictData.Count;
            }
        }
    }
}