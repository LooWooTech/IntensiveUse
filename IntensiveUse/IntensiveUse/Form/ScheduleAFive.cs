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
            Gain(workbook, Core, Year, ID);
        }

        public IWorkbook WriteBase(string filePath,ManagerCore core,int year,int cid,int[] indexs)
        {
            if (indexs == null)
            {
                throw new ArgumentException("Indexs为null");
            }
            IWorkbook workbook = ExcelHelper.OpenWorkbook(filePath);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("服务器模板文件存在问题，请告知相关人员");
            }
            core.IndexManager.WriteIndexWeight(ref sheet, Index[0], year, cid, indexs[0]);
            core.IndexManager.WriteSubIndex(ref sheet, Index[1], year, cid, indexs[0]);
            core.IndexManager.WriteExponent(ref sheet, Index[2], Start, year, cid, indexs[0]);
            return workbook;
        }

        public IWorkbook Write(string FilePath, ManagerCore Core,int Year, string City,string Distict,int[] Indexs)
        {
            //if (Indexs == null)
            //{
            //    throw new ArgumentException("Indexs为null");
            //}
            //IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            //ISheet sheet = workbook.GetSheetAt(0);
            //if (sheet == null)
            //{
            //    throw new ArgumentException("服务器模板文件存在问题，请告知相关人员");
            //}
            string Name=string.Empty;
            if(string.IsNullOrEmpty(Distict)){
                Name=City;
            }else{
                Name=Distict;
            }
            int CID=Core.ExcelManager.GetID(Name);
            return WriteBase(FilePath, Core, Year, CID, Indexs);
            //Core.IndexManager.WriteIndexWeight(ref sheet, Index[0], Year, CID,Indexs[0]);
            //Core.IndexManager.WriteSubIndex(ref sheet, Index[1], Year, CID,Indexs[0]);
            //Core.IndexManager.WriteExponent(ref sheet, Index[2], Start, Year, CID,Indexs[0]);
            //return workbook;
        }

        public IWorkbook AWrite(string filePath, ManagerCore core, int year, string province, string belongCity, string name, int[] indexs)
        {
            return null;
        }

        //public IWorkbook AWrite


        public void Gain(IWorkbook workbook,ManagerCore Core,int Year,int ID)
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
                Core.CommonManager.UpDate(
                    new Dictionary<int, IndexWeight>(){
                        {Year,IndexWeight}
                    },
                    ID
                    );
                Core.IndexManager.Save(subIndex);
                Core.CommonManager.UpDate(
                    new Dictionary<int, SubIndex>(){
                        {Year,subIndex}
                    },
                    ID
                    );
                Core.ExponentManager.Save(exponent);
                Core.CommonManager.UpDate(
                    new Dictionary<int, Exponent>(){
                        {Year,exponent}
                    },
                    ID
                    );
            }
            catch (Exception ex)
            {
                throw new ArgumentException("保存更新数据库的时候，发生错误，错误原因："+ex.ToString());
            }
        }
    }
}