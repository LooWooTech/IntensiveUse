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
    public class TableNationWide:TableBase
    {
        private const int _start = 1;
        private const int _yearIndex = 8;
        private List<Region> _regions { get; set; }
        public Dictionary<Region,Dictionary<int,People>> DictPeople { get; set; }
        public Dictionary<Region,Dictionary<int,Economy>> DictEconomy { get; set; }
        public Dictionary<Region,Dictionary<int,AgricultureLand>> DictArgicultureLand { get; set; }
        public Dictionary<Region,Dictionary<int,ConstructionLand>> DictConstructionLand { get; set; }
        public Dictionary<Region,Dictionary<int,NewConstruction>> DictNewConstruction { get; set; }
        public Dictionary<Region,Dictionary<int,LandSupply>> DictLandSupply { get; set; }
        public Dictionary<Region,Dictionary<int,Ratify>> DictRatify { get; set; }
        public Dictionary<Region,Dictionary<int,Superior>> DictSuperior { get; set; }
        public TableNationWide()
        {
            _regions = new List<Region>();
            DictPeople = new Dictionary<Region, Dictionary<int, People>>();
            DictEconomy = new Dictionary<Region, Dictionary<int, Economy>>();
            DictArgicultureLand = new Dictionary<Region, Dictionary<int, AgricultureLand>>();
            DictConstructionLand = new Dictionary<Region, Dictionary<int, ConstructionLand>>();
            DictNewConstruction = new Dictionary<Region, Dictionary<int, NewConstruction>>();
            DictLandSupply = new Dictionary<Region, Dictionary<int, LandSupply>>();
            DictRatify = new Dictionary<Region, Dictionary<int, Ratify>>();
            DictSuperior = new Dictionary<Region, Dictionary<int, Models.Superior>>();
        }

        /// <summary>
        /// 获取地区 省份 所在地级市  区域评价对象 要素代码  区域名称 区域代码  区域级别相关信息
        /// </summary>
        /// <param name="row">数据所在的行</param>
        /// <returns></returns>
        private Region GetCanton(IRow row)
        {
            string[] names = new string[8];
            for(var i = 0; i < 8; i++)
            {
                names[i] = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
            }
            return new Region
            {
                Zone = names[0],
                Province = names[1],
                BelongCity = names[2],
                Evalutaor = (Evaluator)Enum.Parse(typeof(Evaluator), names[3]),
                FactorCode = names[4],
                Name = names[5],
                Code = names[6],
                Degree = names[7]
            };
        }
        /// <summary>
        /// 获取double数组
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="start">起始列号</param>
        /// <param name="End">结束列号</param>
        /// <returns></returns>
        private double[] GetDoubleValues(IRow row,int start,int End)
        {
            var values = new double[End - start];
            double temp = .0;
            var str = string.Empty;
            for(var i = start; i< End; i++)
            {
                str = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                if(double.TryParse(str,out temp))
                {
                    values[i - start] = temp;
                }
            }
            return values;
        }
        private People GetPeople(IRow row,int year)
        {
            var values = GetDoubleValues(row, 9, 15);
            if (values.Count() != 6) return null;
            return new People()
            {
                PermanentSum = values[0],
                Town = values[1],
                County = values[2],
                HouseHold = values[3],
                Agriculture = values[4],
                NonFram = values[5],
                Year = year
            };
        }
        private Economy GetEconomy(IRow row,int year)
        {
            var values = GetDoubleValues(row, 15, 18);
            if (values.Count() != 3) return null;
            return new Economy
            {
                Current = values[0],
                Compare = values[1],
                Aggregate = values[2],
                Year=year
            };
        }
        private AgricultureLand GetAgricultureLand(IRow row,int year)
        {
            var values = GetDoubleValues(row, 18, 24);
            if (values.Count() != 6) return null;
            return new AgricultureLand
            {
                Subtotal = values[0],
                Arable = values[1],
                Garden = values[2],
                Forest = values[3],
                Meadow = values[4],
                Other = values[5],
                Year = year
            };
        }
        private ConstructionLand GetConstructionLand(IRow row,int year)
        {
            var values = GetDoubleValues(row, 24, 34);
            if (values.Count() != 10) return null;
            return new ConstructionLand
            {
                SubTotal = values[0],
                TowCouConstruction = values[1],
                TownMiningLease = values[2],
                Town = values[3],
                MiningLease = values[4],
                County = values[5],
                Traffic = values[6],
                OtherConstruction = values[7],
                Other = values[8],
                Sum = values[9],
                Year=year
            };
        }
        private NewConstruction GetNewConstruction(IRow row,int year)
        {
            var values = GetDoubleValues(row, 34, 36);
            if (values.Count() != 2) return null;
            return new NewConstruction
            {
                Construction = values[0],
                Town = values[1],
                Year = year
            };
        }
        private LandSupply GetLandSupply(IRow row,int year)
        {
            var values = GetDoubleValues(row, 36, 39);
            if (values.Count() != 3) return null;
            return new LandSupply
            {
                Sum = values[0],
                Append = values[1],
                Stock = values[2],
                Year = year
            };
        }
        private Ratify GetRatify(IRow row,int year)
        {
            var values = GetDoubleValues(row, 39, 41);
            if (values.Count() != 2) return null;
            return new Ratify
            {
                Area = values[0],
                Already = values[1],
                Year = year
            };
        }
        private Superior GetSuperior(IRow row,int year)
        {
            var values = GetDoubleValues(row, 41, 47);
            if (values.Count() != 6) return null;
            return new Models.Superior
            {
                ProvinceCurrent = values[0],
                ProvinceCompare = values[1],
                ProvinceConstruction = values[2],
                CityConstruction = values[3],
                CityCurrent = values[4],
                CityCompare = values[5],
                Year = year
            };
        }
        /// <summary>
        /// 获取每个地区的
        /// </summary>
        /// <param name="row"></param>
        /// <param name="Year"></param>
        /// <param name="canton"></param>
        private void GainForValue(IRow row,int Year,Region region)
        {
            #region    人口数据
            var people = GetPeople(row, Year);
            if (people != null)
            {
                if (!DictPeople.ContainsKey(region))
                {
                    DictPeople.Add(region, new Dictionary<int, People>() { { Year, people } });
                }
                else
                {
                    if (!DictPeople[region].ContainsKey(Year))
                    {
                        DictPeople[region].Add(Year, people);
                    }
                }
            }
            #endregion

            #region  生产值
            var economy = GetEconomy(row, Year);
            if (economy != null)
            {
                if (!DictEconomy.ContainsKey(region))
                {
                    DictEconomy.Add(region, new Dictionary<int, Economy>() { { Year, economy } });
                }
                else
                {
                    if (!DictEconomy[region].ContainsKey(Year))
                    {
                        DictEconomy[region].Add(Year, economy);
                    }
                }
            }
            #endregion

            #region  农用地
            var agricultureland = GetAgricultureLand(row, Year);
            if (agricultureland != null)
            {
                if (!DictArgicultureLand.ContainsKey(region))
                {
                    DictArgicultureLand.Add(region, new Dictionary<int, AgricultureLand>() { { Year, agricultureland } });
                }
                else
                {
                    if (!DictArgicultureLand[region].ContainsKey(Year))
                    {
                        DictArgicultureLand[region].Add(Year, agricultureland);
                    }
                }
            }
            #endregion

            #region  建设用地面积
            var constructionLand = GetConstructionLand(row, Year);
            if (constructionLand != null)
            {
                if (!DictConstructionLand.ContainsKey(region))
                {
                    DictConstructionLand.Add(region, new Dictionary<int, ConstructionLand>() { { Year, constructionLand } });
                }
                else
                {

                    if (!DictConstructionLand[region].ContainsKey(Year))
                    {
                        DictConstructionLand[region].Add(Year, constructionLand);
                    }
                }
            }
            #endregion

            #region  新增建设用地
            var newConstruction = GetNewConstruction(row, Year);
            if (newConstruction != null)
            {

                if (!DictNewConstruction.ContainsKey(region))
                {
                    DictNewConstruction.Add(region, new Dictionary<int, NewConstruction>() { { Year, newConstruction } });
                }
                else
                {
                    if (!DictNewConstruction[region].ContainsKey(Year))
                    {
                        DictNewConstruction[region].Add(Year, newConstruction);
                    }
                }
            }
            #endregion

            #region 土地供应量
            var landSupply = GetLandSupply(row, Year);
            if (landSupply != null)
            {
                if (!DictLandSupply.ContainsKey(region))
                {
                    DictLandSupply.Add(region, new Dictionary<int, LandSupply>() { { Year, landSupply } });
                }else
                {
                    if (!DictLandSupply[region].ContainsKey(Year))
                    {
                        DictLandSupply[region].Add(Year, landSupply);
                    }
                }
            }
            #endregion

            #region 批准批次土地面积
            var ratify = GetRatify(row, Year);
            if (ratify != null)
            {
                if (!DictRatify.ContainsKey(region))
                {
                    DictRatify.Add(region, new Dictionary<int, Ratify>() { { Year, ratify } });
                }
                else
                {
                    if (!DictRatify[region].ContainsKey(Year))
                    {
                        DictRatify[region].Add(Year, ratify);
                    }
                }
            }
            #endregion

            #region 上级相关数据

            var superior = GetSuperior(row, Year);
            if (superior != null)
            {
                if (!DictSuperior.ContainsKey(region))
                {
                    DictSuperior.Add(region, new Dictionary<int, Models.Superior>() { { Year, superior } });
                }
                else
                {
                    if (!DictSuperior[region].ContainsKey(Year))
                    {
                        DictSuperior[region].Add(Year, superior);
                    }
                }
            }

            #endregion
        }
        public void Gain(string filePath)
        {
            ISheet sheet = ExcelHelper.Open(filePath);
            var row = RowGet(sheet, _start);
            ICell cell = null;
            var flag = true;
            string value = string.Empty;
            int count = _start;
            while (flag)
            {
                row = RowGet(sheet, count);//开始获取行对象Row
                cell = row.GetCell(_yearIndex, MissingCellPolicy.CREATE_NULL_AS_BLANK);//获取关键字年份的信息
                value = ExcelHelper.GetValue(cell).Replace("年", "");
                if (string.IsNullOrEmpty(value))
                {
                    break;
                }
                if (!VerificationYear(value))//填写的年份数据不正确
                {
                    flag = false;
                }
                else
                {
                    var region = GetCanton(row);
                    if (!_regions.Contains(region))
                    {
                        _regions.Add(region);
                    }
                    int year = 0;
                    if(int.TryParse(value,out year))//获取int类型的年份数据
                    {
                        GainForValue(row, year, region);
                        count++;
                    }
                }
            }
        }
        public void Save(ManagerCore core)
        {
            foreach(var region in _regions)
            {
                var regionID = core.ExcelManager.GetID(region);
                #region  保存People数据
                if (DictPeople.ContainsKey(region))
                {
                    core.ExcelManager.Save(DictPeople[region].Select(e => new People
                    {
                        PermanentSum = e.Value.PermanentSum,
                        Town = e.Value.Town,
                        County = e.Value.County,
                        HouseHold = e.Value.HouseHold,
                        Agriculture = e.Value.Agriculture,
                        NonFram = e.Value.NonFram,
                        Year = e.Value.Year,
                        RID = regionID
                    }).ToList(), regionID);
                }
                #endregion

                #region 保存生产总值数据
                if (DictEconomy.ContainsKey(region))
                {
                    core.ExcelManager.Save(DictEconomy[region].Select(e => new Economy
                    {
                        Current = e.Value.Current,
                        Compare = e.Value.Compare,
                        Aggregate = e.Value.Aggregate,
                        Year = e.Value.Year,
                        RID = regionID
                    }).ToList(), regionID);
                }
                #endregion

                #region 保存农用地数据
                if (DictArgicultureLand.ContainsKey(region))
                {
                    core.ExcelManager.Save(DictArgicultureLand[region].Select(e => new AgricultureLand
                    {
                        Subtotal = e.Value.Subtotal,
                        Arable = e.Value.Arable,
                        Garden = e.Value.Garden,
                        Forest = e.Value.Forest,
                        Meadow = e.Value.Meadow,
                        Other = e.Value.Other,
                        Year = e.Value.Year,
                        RID = regionID
                    }).ToList(), regionID);
                }
                #endregion

                #region 保存建设用地
                if (DictConstructionLand.ContainsKey(region))
                {
                    core.ExcelManager.Save(DictConstructionLand[region].Select(e => new ConstructionLand
                    {
                        SubTotal = e.Value.SubTotal,
                        TowCouConstruction = e.Value.TowCouConstruction,
                        TownMiningLease = e.Value.TownMiningLease,
                        Town = e.Value.Town,
                        MiningLease = e.Value.MiningLease,
                        County = e.Value.County,
                        Traffic = e.Value.Traffic,
                        OtherConstruction = e.Value.OtherConstruction,
                        Other = e.Value.Other,
                        Sum = e.Value.Sum,
                        Year = e.Value.Year,
                        RID = regionID
                    }).ToList(), regionID);
                }
                #endregion

                #region 保存新增建设用地数据
                if (DictNewConstruction.ContainsKey(region))
                {
                    core.ExcelManager.Save(DictNewConstruction[region].Select(e => new NewConstruction
                    {
                        Construction = e.Value.Construction,
                        Town = e.Value.Town,
                        CID = regionID,
                        Year = e.Value.Year
                    }).ToList(), regionID);
                }
                #endregion

                #region 土地供应量数据
                if (DictLandSupply.ContainsKey(region))
                {
                    core.ExcelManager.Save(DictLandSupply[region].Select(e => new LandSupply
                    {
                        Sum = e.Value.Sum,
                        Append = e.Value.Append,
                        Stock = e.Value.Stock,
                        UnExploit = e.Value.UnExploit,
                        Year = e.Value.Year,
                        RID = regionID
                    }).ToList(), regionID);
                }
                #endregion

                #region 批准批次土地供应面积
                if (DictRatify.ContainsKey(region))
                {
                    core.ExcelManager.Save(DictRatify[region].Select(e => new Ratify
                    {
                        Area = e.Value.Area,
                        Already = e.Value.Already,
                        Year = e.Value.Year,
                        RID = regionID
                    }).ToList(), regionID);
                }
                #endregion

                #region 保存上一级数据
                if (DictSuperior.ContainsKey(region))
                {
                    core.ExcelManager.Save(DictSuperior[region].Select(e => new Superior
                    {
                        ProvinceCurrent = e.Value.ProvinceCurrent,
                        ProvinceCompare = e.Value.ProvinceCompare,
                        ProvinceConstruction = e.Value.ProvinceConstruction,
                        CityConstruction = e.Value.CityConstruction,
                        CityCurrent = e.Value.CityCurrent,
                        CityCompare = e.Value.CityCompare,
                        Year = e.Value.Year,
                        RID = regionID
                    }).ToList(), regionID);
                }
                #endregion

            }
        }
    }
}