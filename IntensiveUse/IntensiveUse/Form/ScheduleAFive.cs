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
    public class ScheduleAFive:ISchedule,IRead
    {
        public int[] Index = { 1, 3, 5 };
        public const int Start = 2;
        public void Read(string FilePath,ManagerCore Core,string City,int Year)
        {
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            int ID = Core.ExcelManager.GetID(City);
            Gain(workbook, Core, Year.ToString(), ID);
        }

        public IWorkbook Write(string FilePath, ManagerCore Core,int Year, string City,string Distict)
        {
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            
            return workbook;
        }


        public void Gain(IWorkbook workbook,ManagerCore Core,string Year,int ID)
        {
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("未获取到相关的sheet表格");
            }
            IndexWeight IndexWeight = Core.IndexManager.GainIndexWeight(sheet,Index[0]);
            IndexWeight.Year = Year;
            IndexWeight.RID = ID;
            SubIndex subIndex = Core.IndexManager.GainSubIndex(sheet, Index[1]);
            subIndex.Year = Year;
            subIndex.RID = ID;
            Exponent exponent = Core.IndexManager.GainExponent(sheet,Index[2],Start);
            exponent.Year = Year;
            exponent.RID = ID;
            exponent.Type = IdealType.Weight;
            try
            {
                Core.IndexManager.Save(IndexWeight);
                Core.IndexManager.Save(subIndex);
                Core.ExponentManager.Save(exponent);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("保存更新数据库的时候，发生错误，错误原因："+ex.ToString());
            }
        }
    }
}