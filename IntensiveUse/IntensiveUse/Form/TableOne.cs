using IntensiveUse.Helper;
using IntensiveUse.Manager;
using IntensiveUse.Models;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;

namespace IntensiveUse.Form
{
    public class TableOne:TableBase,IForm
    {
        private const int Start = 4;
        private const int Begin = 12;
        public int[] Serials = { 12, 13, 14, 15, 16, 17, 18, 20, 21,22,24 }; 
        
        public string Regimentatio { get; set; }
        public int Count{get;set;}
        public Dictionary<int, People> DictPeople { get; set; }
        public Dictionary<int, Economy> DictEconomy { get; set; }
        public Dictionary<int, Economy> DictSuperior { get; set; }
        public TableOne(string Name,string City)
        {
            this.Name = Name;
            this.City = City;
            if (DictPeople == null)
            {
                DictPeople = new Dictionary<int, People>();
            }
            if (DictEconomy == null)
            {
                DictEconomy = new Dictionary<int, Economy>();
            }

            if (DictSuperior == null)
            {
                DictSuperior = new Dictionary<int, Economy>();
            }
        }
        
        public void Gain(string FilePath)
        {
            ISheet sheet = ExcelHelper.Open(FilePath);
            IRow row = RowGet(sheet, 0);
            GetMessage(row);
            row = RowGet(sheet,1);
            bool Flag=true;
            ICell cell=null;
            string value=string.Empty;
            this.Count=Start;
            while(Flag)
            {
                cell=row.GetCell(Count,MissingCellPolicy.CREATE_NULL_AS_BLANK);
                value=ExcelHelper.GetValue(cell).Replace("年","");
                int m = 0;
                int.TryParse(value, out m);
                if (string.IsNullOrEmpty(value))
                {
                    break;
                }
                if(!VerificationYear(value)){
                    Flag=false;
                }else{
                    GainForValue(sheet, Count, m);//
                    Count++;
                }
            }  
        }

        public void Save(ManagerCore Core)
        {
            int ID = Core.ExcelManager.GetID(this.Name);
            Update(ID);
            try
            {
                Core.PeopleManager.Save(DictPeople,ID);
                Core.CommonManager.UpDate(DictPeople, ID);
                Core.ExcelManager.Save(DictEconomy, ID);
                Core.CommonManager.UpDate(DictEconomy, ID);

                Superior(Core);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("保存更新人口数据时发生错误："+ex.ToString());
            }

        }



        public void Superior(ManagerCore Core)
        {
            if (string.IsNullOrEmpty(this.City))
            {
                this.City = this.Name == "银川市" ? "宁夏回族自治区" : "浙江省";
            }
            int ID = Core.ExcelManager.GetID(this.City);
            foreach (var item in DictSuperior.Values)
            {
                item.RID = ID;
            }
            Change = Core.EconmoyManager.Superior(DictSuperior);
        }
        public void GainForValue(ISheet sheet,int SerialNumber,int Year)
        {
            double[] data = new double[11];
            int x = 0;
            foreach(int j in Serials)
            {
                IRow row = RowGet(sheet, j);
                ICell cell = row.GetCell(SerialNumber, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                double.TryParse(ExcelHelper.GetValue(cell), out data[x]);
                x++;
            }
            
            if (!DictPeople.ContainsKey(Year))
            {
                DictPeople.Add(Year, new People
                {
                    PermanentSum = data[0],
                    Town = data[1],
                    County = data[2],
                    HouseHold = data[3],
                    Agriculture = data[4],
                    NonFram = data[5],
                    Year=Year
                });
            }
            if (!DictEconomy.ContainsKey(Year))
            {
                DictEconomy.Add(Year, new Economy
                {
                    Current = data[6],
                    Compare = data[7],
                    Aggregate = data[8],
                    Year=Year
                });
            }

            if (!DictSuperior.ContainsKey(Year))
            {
                DictSuperior.Add(Year, new Economy
                {
                    Current = data[9],
                    Compare = data[10],
                    Year=Year
                });
            }
             
        }

        private void  GetMessage(IRow row)
        {
            ICell cell = row.GetCell(2,MissingCellPolicy.CREATE_NULL_AS_BLANK);
            if (cell == null)
            {
                throw new ArgumentException("在读取表格的时候，未获取城市名相关信息，无法进行下一步数据归档，请填写城市名");
            }
            //this.Name = ExcelHelper.GetValue(cell);
            cell = row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            this.Regimentatio = ExcelHelper.GetValue(cell);
        }

        public void Update(int ID)
        {
            foreach (var item in DictPeople.Values)
            {
                item.RID = ID;
            }
            foreach (var item in DictEconomy.Values)
            {
                item.RID = ID;
            }    
            
        }




        
        


    }
}