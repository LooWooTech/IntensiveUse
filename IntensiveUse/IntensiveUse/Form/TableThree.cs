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
    public class TableThree:TableBase,IForm
    {
        public const int Start = 1;
        public const int Begin = 3;
        public string Regimentatio { get; set; }
        public Dictionary<int, NewConstruction> DictNewConstruction { get; set; }
        public Dictionary<int, LandSupply> DictLandSupply { get; set; }
        public Dictionary<int, Ratify> DictRatify { get; set; }
        public Dictionary<int, ConstructionLand> DictConstructionLand { get; set; }
        public TableThree(string Name,string City)
        {
            this.Name = Name;
            this.City = City;
            if (DictNewConstruction == null)
            {
                DictNewConstruction = new Dictionary<int, NewConstruction>();
            }
            if (DictLandSupply == null)
            {
                DictLandSupply = new Dictionary<int, LandSupply>();
            }
            if (DictRatify == null)
            {
                DictRatify = new Dictionary<int, Ratify>();
            }
            if (DictConstructionLand == null)
            {
                DictConstructionLand = new Dictionary<int, ConstructionLand>();
            }
        }
        public void Gain(string FilePath)
        {
            ISheet sheet = ExcelHelper.Open(FilePath);
            IRow row = RowGet(sheet, Start);
            GetMessage(row);
            row = RowGet(sheet, 2);
            bool Flag = true;
            ICell cell;
            int Count = Begin;
            string value = string.Empty;
            while (Flag)
            {
                cell = row.GetCell(Count, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                value = ExcelHelper.GetValue(cell).Replace("年", "");
                int m = 0;
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
                    if (int.TryParse(value, out m))
                    {
                        GainForValue(sheet, Count, m);
                    }
                    
                    Count++;
                }
            }
        }
        public void Save(ManagerCore Core)
        {
            int ID = Core.ExcelManager.GetID(this.Name);
            Update(ID);
            try {
                Core.ExcelManager.Save(DictNewConstruction,ID);
                Core.CommonManager.UpDate(DictNewConstruction, ID);
                Core.ExcelManager.Save(DictLandSupply, ID);
                Core.CommonManager.UpDate(DictLandSupply, ID);
                Core.ExcelManager.Save(DictRatify, ID);
                Core.CommonManager.UpDate(DictRatify, ID);
                Superior(Core);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("保存更新数据时发生错误："+ex.ToString());
            }
        }

        public override void Superior(ManagerCore Core)
        {
            base.Superior(Core);
            foreach (var item in DictConstructionLand.Values)
            {
                item.RID = this.SID;
            }

            Change = Core.ConstructionLandManager.Superior(DictConstructionLand);
        }

        public void GainForValue(ISheet sheet, int SerialNumber, int Year)
        {
            double[] Data=new double[9];
            for (var i = 0; i < 9; i++)
            {
                IRow row = RowGet(sheet, i + Start + 2);
                ICell cell = row.GetCell(SerialNumber, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                double.TryParse(ExcelHelper.GetValue(cell),out Data[i]);
            }
           

            if (!DictNewConstruction.ContainsKey(Year))
            {
                DictNewConstruction.Add(Year, new NewConstruction
                {
                    Construction = Data[0],
                    Town = Data[1],
                    Year = Year
                });
            }
            if (!DictLandSupply.ContainsKey(Year))
            {
                DictLandSupply.Add(Year, new LandSupply
                {
                    Sum = Data[2],
                    Append = Data[3],
                    Stock = Data[4],
                    UnExploit = Data[5],
                    Year = Year
                });
            }
            if (!DictRatify.ContainsKey(Year))
            {
                DictRatify.Add(Year, new Ratify
                {
                    Area = Data[6],
                    Already = Data[7],
                    Year = Year
                });
            }

            if (!DictConstructionLand.ContainsKey(Year))
            {
                DictConstructionLand.Add(Year, new ConstructionLand
                {
                    SubTotal = Data[8],
                    Year = Year
                });
            }
            
        }

        public void GetMessage(IRow Row)
        {
            ICell cell = Row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            if (cell == null)
            {
                throw new ArgumentException("未获取城市名相关信息，无法进行数据录入工作");
            }
            //this.Name = ExcelHelper.GetValue(cell);
            cell = Row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            this.Regimentatio = ExcelHelper.GetValue(cell);
        }

        public void Update(int ID)
        {
            foreach (var item in DictNewConstruction.Values)
            {
                item.CID = ID;
            }
            foreach (var item in DictLandSupply.Values)
            {
                item.RID = ID;
            }
            foreach (var item in DictRatify.Values)
            {
                item.RID = ID;
            }
        }
    }
}