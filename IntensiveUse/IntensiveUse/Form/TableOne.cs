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
        public int[] Serials = { 12, 13, 14, 15, 16, 17, 18, 20, 21 }; 
        
        public string Regimentatio { get; set; }
        public int Count{get;set;}
        public Dictionary<string, People> DictPeople { get; set; }
        public Dictionary<string, Economy> DictEconomy { get; set; }
        
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
                value=GetValue(cell).Replace("年","");
                if (string.IsNullOrEmpty(value))
                {
                    break;
                }
                if(!VerificationYear(value)){
                    Flag=false;
                }else{
                    GainForValue(sheet, Count, value);//
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
                Core.ExcelManager.Save(DictEconomy, ID);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("保存更新人口数据时发生错误："+ex.ToString());
            }
            
            //Console.ReadLine();

        }
        public void GainForValue(ISheet sheet,int SerialNumber,string Year)
        {
            double[] data = new double[9];
            int x = 0;
            foreach(int j in Serials)
            {
                IRow row = RowGet(sheet, j);
                ICell cell = row.GetCell(SerialNumber, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                double.TryParse(GetValue(cell), out data[x]);
                x++;
            }
            if (DictPeople == null)
            {
                DictPeople = new Dictionary<string, People>();
            }
            if (DictEconomy == null)
            {
                DictEconomy = new Dictionary<string, Economy>();
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
             
        }

        private void  GetMessage(IRow row)
        {
            ICell cell = row.GetCell(2,MissingCellPolicy.CREATE_NULL_AS_BLANK);
            if (cell == null)
            {
                throw new ArgumentException("在读取表格的时候，未获取城市名相关信息，无法进行下一步数据归档，请填写城市名");
            }
            this.Name = GetValue(cell);
            cell = row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            this.Regimentatio = GetValue(cell);
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