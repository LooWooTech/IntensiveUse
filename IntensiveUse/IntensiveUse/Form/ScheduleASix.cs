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
    public class ScheduleASix:ScheduleBase,ISchedule,IRead
    {
        public Queue<string> Qfoundation { get; set; }
        public ScheduleASix()
        {
            this.Start = 2;
            this.Begin = 4;
            this.SerialNumber = 2;
            if (Qfoundation == null)
            {
                Qfoundation = new Queue<string>();
            }
        }

        public void Read(string FilePath,ManagerCore Core,string City,int Year)
        {
            
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("未获取上传文件的sheet");
            }
            Exponent exponent = Core.IndexManager.GainExponent(sheet, Begin + 1, Start);
            if (exponent == null)
            {
                throw new ArgumentException("未获取到相关理想数据");
            }
            Foundation foundation = Core.IndexManager.GainFoundation(sheet, Begin + 2, Start);
            if (foundation == null)
            {
                throw new ArgumentException("未获取到相关理想值依据");
            }
            this.Year = Year;
            this.CID = Core.ExcelManager.GetID(City);
            exponent.Year = Year;
            exponent.RID = CID;
            exponent.Type = IdealType.Value;
            foundation.RID = CID;
            foundation.Year = Year;

            Core.ExponentManager.Save(exponent);
            Core.FoundationManager.Save(foundation);
            Core.CommonManager.UpDate(
                new Dictionary<int, Exponent>(){
                    {Year,exponent}
                },
                exponent.RID
                );
        }
        public IWorkbook Write(string FilePath, ManagerCore Core, int Year,string City,string Distict,int[] Indexs)
        {
            if (Indexs == null || Indexs.Count() != this.SerialNumber)
            {
                throw new ArgumentException("未读取到服务器上的模板文件，请联系相关人员");
            }
            IWorkbook workbook = ExcelHelper.OpenWorkbook(FilePath);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("服务器缺失相关模板");
            }
            this.Year = Year;
            if (string.IsNullOrEmpty(Distict))
            {
                this.CID = Core.ExcelManager.GetID(City);
            }
            else
            {
                this.CID = Core.ExcelManager.GetID(Distict);
            }
            
            Message(Core);
            int line = 0;
            int Serial = 0;
            foreach (var item in DictData.Keys)
            {
                line = Start;
                var values = DictData[item];
                int a = 0;
                if (int.TryParse(item, out a))
                {
                    foreach (var entity in values)
                    {
                        IRow row = ExcelHelper.OpenRow(ref sheet, line++);
                        ICell cell = ExcelHelper.OpenCell(ref row, a);
                        cell.SetCellValue(Math.Round(entity, Indexs[Serial]));
                    }
                }
                Serial++;
            }
            line=Start;
            foreach (var item in Qfoundation)
            {
                IRow row = ExcelHelper.OpenRow(ref sheet, line++);
                ICell cell = ExcelHelper.OpenCell(ref row, Begin + 2);
                cell.SetCellValue(item);
            }
            return workbook;
        }

        public void Message(ManagerCore Core)
        {
            Exponent exponent =Core.ExponentManager.GetTurthExponent(Year,CID);
            Queue<double> queue = new Queue<double>();
            Core.ExcelManager.Gain(exponent, ref queue);
            DictData.Add(Begin.ToString(), queue);


            Queue<double> queue2 = new Queue<double>();
            Exponent idealExponent = Core.ExponentManager.SearchForExponent(Year, CID, IdealType.Value);
            Core.ExcelManager.Gain(idealExponent, ref queue2);
            DictData.Add((Begin + 1).ToString(), queue2);


            Foundation foundation = Core.FoundationManager.SearchForFoundation(Year, CID);
            System.Reflection.PropertyInfo[] propList = typeof(Foundation).GetProperties();
            foreach (var item in propList)
            {
                if (item.PropertyType.Equals(typeof(string)))
                {
                    string str="";
                    if(item.GetValue(foundation,null)!=null){
                        str=item.GetValue(foundation,null).ToString();
                    }
                    Qfoundation.Enqueue(str);
                }
            }
        }


    }
}