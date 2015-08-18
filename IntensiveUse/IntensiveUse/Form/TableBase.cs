﻿using IntensiveUse.Manager;
using IntensiveUse.Models;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace IntensiveUse.Form
{
    public class TableBase
    {
        public string Name { get; set; }
        protected string City { get; set; }
        protected int SID { get; set; }

        private Dictionary<string, Queue<double>> DictValues { get; set; }

        public Dictionary<string, Queue<double>> GetDictValue()
        {
            return DictValues;
        }

        public string GetName()
        {
            return Name;
        }

        public string GetCity()
        {
            return City;
        }

        protected List<EconomyChange> Change { get; set; }

        public List<EconomyChange> GetChange()
        {
            return Change;
        }

        public TableBase()
        {
            if (DictValues == null)
            {
                DictValues = new Dictionary<string, Queue<double>>();
            }
        }
        

        private  Regex _yearRe = new Regex(@"^20[0-9]{2}", RegexOptions.Compiled);
        public  bool VerificationYear(string value)
        {
            return _yearRe.IsMatch(value);
        }

        

        public IRow RowGet(ISheet sheet,int ID)
        {
            IRow row = sheet.GetRow(ID);
            if (row == null)
            {
                throw new ArgumentException("未获取相关EXCEL表格行，请核对表格数据，如有冲突请联系相关人员");
            }
            return row;
        }

        public virtual void Superior(ManagerCore Core)
        {
            if (string.IsNullOrEmpty(this.City))
            {
                this.City = this.Name == "银川市" ? "宁夏回族自治区" : "浙江省";
            }
            this.SID = Core.ExcelManager.GetID(this.City);
        }
    }
}