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
    public class ScheduleATwelve:ScheduleBase,ISchedule
    {
        private const int Line = 12;
        private const int Start = 3;
        private UGEAA UGEAA { get; set; }
        public ScheduleATwelve()
        {
            this.SerialNumber = 5;
            if (UGEAA == null)
            {
                UGEAA = new UGEAA();
            }
        }
        public IWorkbook Write(string FilePath, ManagerCore Core, int Year, string City, string Distict,int[] Indexs)
        {
            if (Indexs == null || Indexs.Count() != this.SerialNumber)
            {
                throw new ArgumentException("精度位数据为null或者空，无法进行数据填写");
            }
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("服务器上的模板文件存在问题，缺失相关sheet");
            }
            this.TempRow = sheet.GetRow(Start);
            this.Year = Year;
            this.CID = Core.ExcelManager.GetID(City);
            this.Disticts = Core.ExcelManager.GetDistrict(City);
            Message(Core);
            Ready();
            this.Queue = Core.ConstructionLandManager.TranslateOfUGEAA(UGEAA);
            WriteBase(ref sheet, Start,Indexs);
            return workbook;
        }
        public IWorkbook AWrite(string filePath, ManagerCore core, int year, string province, string belongCity, string name, int[] indexs)
        {
            return null;
        }

        public void Ready()
        {
            for (var i = 2; i < Line; i++)
            {
                if (i % 2 == 0)
                {
                    Columns.Add(i);
                }
            }
        }

        public void Message(ManagerCore Core)
        {
            foreach (var item in Disticts)
            {

                int ID = Core.ExcelManager.GetID(item);
                UGEAA ugeaa = Core.ConstructionLandManager.AcquireOfUGEAA(Year, ID, CID);
                if (!DictData.ContainsKey(item))
                {
                    DictData.Add(item, Core.ConstructionLandManager.TranslateOfUGEAA(ugeaa));
                    UGEAA += ugeaa;
                }
            }
            if (DictData.Count > 0)
            {
                UGEAA = UGEAA / DictData.Count;
            }
        }
    }
}