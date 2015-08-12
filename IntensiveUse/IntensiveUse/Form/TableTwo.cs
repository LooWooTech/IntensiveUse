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
    public class TableTwo:TableBase,IForm
    {
        private const int Start = 4;
        private const int Begin = 1;
        private int[] AgSerial = { 
                                     4, 8, 12, 17, 18, 29, 37, 40, 44, 45, 
                                     21, 22, 24, 23, 27, 28, 30, 31, 32, 36, 41, 25, 
                                     19, 34, 35, 38, 39,42, 46, 47,48,49
                                 };
        public string Code { get; set; }
        public Dictionary<int, AgricultureLand> DictAgriculture { get; set; }
        public Dictionary<int, ConstructionLand> DictConstruction { get; set; }
        public TableTwo(string Name)
        {
            this.Name = Name;
            if (DictAgriculture == null)
            {
                DictAgriculture = new Dictionary<int, AgricultureLand>();
            }
            if (DictConstruction == null)
            {
                DictConstruction = new Dictionary<int, ConstructionLand>();
            }
        }
       
        public void Gain(string FilePath)
        {
            ISheet sheet = ExcelHelper.Open(FilePath);
            IRow row = RowGet(sheet, Start);
            GetMessage(row);
            bool Flag = true;
            ICell cell;
            string value = string.Empty;
            int Line = Start;
            while (Flag)
            {
                row = sheet.GetRow(Line);
                if (row == null)
                {
                    break;
                }
                cell = row.GetCell(Begin, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                value = ExcelHelper.GetValue(cell).Replace("年","");
                int m = 0;
                int.TryParse(value, out m);
                if (string.IsNullOrEmpty(value))
                {
                    break;
                }
                if (!VerificationYear(value))
                {
                    Flag = false;
                }
                else
                {
                    GainForValue(row, m);
                    Line++;
                }
            }

        }
        public void Save(ManagerCore Core)
        {
            int ID = Core.ExcelManager.GetID(this.Name);
            Update(ID);
            try
            {
                Core.ExcelManager.Save(DictAgriculture, ID);
                Core.CommonManager.UpDate(DictAgriculture, ID);
                Core.ExcelManager.Save(DictConstruction, ID);
                Core.CommonManager.UpDate(DictConstruction, ID);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("在保存更新表2数据时发生错误："+ex.ToString());
            }

        }

        public void GainForValue(IRow row, int Year)
        {
            int Number = AgSerial.Count();
            double[] Data = new double[Number];
            ICell cell;
            int k=0;
            foreach (var i in AgSerial)
            {
                cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                double.TryParse(ExcelHelper.GetValue(cell), out Data[k]);
                k++;
            }
            
            double sub = 0.0;
            double sum = 0.0;
            for (var i = 0; i < 10; i++)
            {
                sub += Data[i];
            }
            if (!DictAgriculture.ContainsKey(Year))
            {
                DictAgriculture.Add(Year, new AgricultureLand
                {
                    Subtotal=sub,
                    Arable = Data[0],
                    Garden = Data[1],
                    Forest = Data[2],
                    Meadow = Data[3]+Data[4],
                    Other = Data[5] + Data[6] + Data[7] + Data[8] + Data[9],
                    Year = Year
                });
            }
            sum += sub;
            sub = 0;
            for (var i = 10; i < 22; i++)
            {
                sub+=Data[i];//统计建设用地小计
            }
            double other = 0.0;
            for (var i = 22; i < 32; i++)
            {
                other+=Data[i];//统计其他土地
            }
            sum += sub;
            sum += other;
            if (!DictConstruction.ContainsKey(Year))
            {
                DictConstruction.Add(Year, new ConstructionLand
                {
                    SubTotal = sub,
                    Town = Data[10] + Data[11],
                    MiningLease = Data[12],
                    County = Data[13],
                    Traffic = Data[14] + Data[15] + Data[16] + Data[17] + Data[18] + Data[19] + Data[20],
                    OtherConstruction = Data[21],
                    Other = other,
                    Sum=sum,
                    Year = Year
                });
            }
               
               

        }

        public void GetMessage(IRow row)
        {
            ICell cell = row.GetCell(Begin + 1, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            if (cell == null)
            {
                throw new ArgumentException("当检索表2的时候，初次读取行政区划名称数据失败");
            }
            //this.Name = ExcelHelper.GetValue(cell);
            cell = row.GetCell(Begin + 2, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            if (cell == null)
            {
                throw new ArgumentException("当检索表2的时候，初次读取行政区划代码数据失败");
            }
            this.Code = ExcelHelper.GetValue(cell);
        }

        public void Update(int ID)
        {
            foreach (var item in DictAgriculture.Values)
            {
                item.RID = ID;
            }
            foreach (var item in DictConstruction.Values)
            {
                item.RID = ID;
            }
        }

        
    }
}