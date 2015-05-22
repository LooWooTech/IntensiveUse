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
        private int[] AgSerial = { 4, 5, 6, 7, 17, 32, 25, 28, 33, 9, 10, 12, 11, 15, 16, 18, 19, 20, 24, 29, 13, 22, 23, 26, 27, 30, 34,35, 36, 37 };
        public string Code { get; set; }
        public Dictionary<string, AgricultureLand> DictAgriculture { get; set; }
        public Dictionary<string, ConstructionLand> DictConstruction { get; set; }
       
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
                value = GetValue(cell).Replace("年","");
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
                    GainForValue(row, value);
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
                Core.ExcelManager.Save(DictConstruction, ID);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("在保存更新表2数据时发生错误："+ex.ToString());
            }

        }

        public void GainForValue(IRow row, string Year)
        {
            int Number = AgSerial.Count();
            double[] Data = new double[Number];
            ICell cell;
            int k=0;
            foreach (var i in AgSerial)
            {
                cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                double.TryParse(GetValue(cell), out Data[k]);
                k++;
            }
            if (DictAgriculture == null)
            {
                DictAgriculture = new Dictionary<string, AgricultureLand>();
            }
            if (DictConstruction == null)
            {
                DictConstruction = new Dictionary<string, ConstructionLand>();
            }
            double sub = 0.0;
            double sum = 0.0;
            for (var i = 0; i < 9; i++)
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
                    Meadow = Data[3],
                    Other = Data[4] + Data[5] + Data[6] + Data[7] + Data[8],
                    Year = Year
                });
            }
            sum += sub;
            sub = 0;
            for (var i = 9; i < 21; i++)
            {
                sub+=Data[i];
            }
            double other = 0.0;
            for (var i = 21; i < 30; i++)
            {
                other+=Data[i];
            }
            sum += sub;
            sum += other;
            if (!DictConstruction.ContainsKey(Year))
            {
                DictConstruction.Add(Year, new ConstructionLand
                {
                    SubTotal = sub,
                    Town = Data[9] + Data[10],
                    MiningLease = Data[11],
                    County = Data[12],
                    Traffic = Data[13] + Data[14] + Data[15] + Data[16] + Data[17] + Data[18] + Data[19],
                    OtherConstruction = Data[20],
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
            this.Name = GetValue(cell);
            cell = row.GetCell(Begin + 2, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            if (cell == null)
            {
                throw new ArgumentException("当检索表2的时候，初次读取行政区划代码数据失败");
            }
            this.Code = GetValue(cell);
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