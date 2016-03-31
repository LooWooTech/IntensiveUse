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
        //private List<Region> _regions { get; set; }
        private Dictionary<string,Region> _dictRegions { get; set; }
        public Dictionary<string,Dictionary<int,People>> DictPeople { get; set; }
        public Dictionary<string,Dictionary<int,Economy>> DictEconomy { get; set; }
        public Dictionary<string,Dictionary<int,AgricultureLand>> DictArgicultureLand { get; set; }
        public Dictionary<string,Dictionary<int,ConstructionLand>> DictConstructionLand { get; set; }
        public Dictionary<string,Dictionary<int,NewConstruction>> DictNewConstruction { get; set; }
        public Dictionary<string,Dictionary<int,LandSupply>> DictLandSupply { get; set; }
        public Dictionary<string,Dictionary<int,Ratify>> DictRatify { get; set; }
        public Dictionary<string,Dictionary<int,Superior>> DictSuperior { get; set; }
        public TableNationWide()
        {
            //_regions = new List<Region>();
            _dictRegions = new Dictionary<string, Region>();
            DictPeople = new Dictionary<string, Dictionary<int, People>>();
            DictEconomy = new Dictionary<string, Dictionary<int, Economy>>();
            DictArgicultureLand = new Dictionary<string, Dictionary<int, AgricultureLand>>();
            DictConstructionLand = new Dictionary<string, Dictionary<int, ConstructionLand>>();
            DictNewConstruction = new Dictionary<string, Dictionary<int, NewConstruction>>();
            DictLandSupply = new Dictionary<string, Dictionary<int, LandSupply>>();
            DictRatify = new Dictionary<string, Dictionary<int, Ratify>>();
            DictSuperior = new Dictionary<string, Dictionary<int, Models.Superior>>();
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
                Evalutaor=names[3],
                //Evalutaor = (Evaluator)Enum.Parse(typeof(Evaluator), names[3]),
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
        private void GainForValue(IRow row,int Year,string key)
        {
            Console.WriteLine(string.Format("年份：{0} 的人口数据",Year));
            #region    人口数据
            var people = GetPeople(row, Year);
            if (people != null)
            {
                if (!DictPeople.ContainsKey(key))
                {
                    DictPeople.Add(key, new Dictionary<int, People>() { { Year, people } });
                }
                else
                {
                    if (!DictPeople[key].ContainsKey(Year))
                    {
                        DictPeople[key].Add(Year, people);
                    }
                }
            }
            #endregion
            Console.WriteLine(string.Format("年份：{0} 的生产值数据", Year));
            #region  生产值
            var economy = GetEconomy(row, Year);
            if (economy != null)
            {
                if (!DictEconomy.ContainsKey(key))
                {
                    DictEconomy.Add(key, new Dictionary<int, Economy>() { { Year, economy } });
                }
                else
                {
                    if (!DictEconomy[key].ContainsKey(Year))
                    {
                        DictEconomy[key].Add(Year, economy);
                    }
                }
            }
            #endregion
            Console.WriteLine(string.Format("年份：{0} 的农用地数据", Year));
            #region  农用地
            var agricultureland = GetAgricultureLand(row, Year);
            if (agricultureland != null)
            {
                if (!DictArgicultureLand.ContainsKey(key))
                {
                    DictArgicultureLand.Add(key, new Dictionary<int, AgricultureLand>() { { Year, agricultureland } });
                }
                else
                {
                    if (!DictArgicultureLand[key].ContainsKey(Year))
                    {
                        DictArgicultureLand[key].Add(Year, agricultureland);
                    }
                }
            }
            #endregion
            Console.WriteLine(string.Format("年份：{0} 的建设用地面积数据", Year));
            #region  建设用地面积
            var constructionLand = GetConstructionLand(row, Year);
            if (constructionLand != null)
            {
                if (!DictConstructionLand.ContainsKey(key))
                {
                    DictConstructionLand.Add(key, new Dictionary<int, ConstructionLand>() { { Year, constructionLand } });
                }
                else
                {

                    if (!DictConstructionLand[key].ContainsKey(Year))
                    {
                        DictConstructionLand[key].Add(Year, constructionLand);
                    }
                }
            }
            #endregion
            Console.WriteLine(string.Format("年份：{0} 的新增建设用地数据", Year));
            #region  新增建设用地
            var newConstruction = GetNewConstruction(row, Year);
            if (newConstruction != null)
            {

                if (!DictNewConstruction.ContainsKey(key))
                {
                    DictNewConstruction.Add(key, new Dictionary<int, NewConstruction>() { { Year, newConstruction } });
                }
                else
                {
                    if (!DictNewConstruction[key].ContainsKey(Year))
                    {
                        DictNewConstruction[key].Add(Year, newConstruction);
                    }
                }
            }
            #endregion
            Console.WriteLine(string.Format("年份：{0} 的土地供应量数据", Year));
            #region 土地供应量
            var landSupply = GetLandSupply(row, Year);
            if (landSupply != null)
            {
                if (!DictLandSupply.ContainsKey(key))
                {
                    DictLandSupply.Add(key, new Dictionary<int, LandSupply>() { { Year, landSupply } });
                }else
                {
                    if (!DictLandSupply[key].ContainsKey(Year))
                    {
                        DictLandSupply[key].Add(Year, landSupply);
                    }
                }
            }
            #endregion
            Console.WriteLine(string.Format("年份：{0} 的批准批次土地面积数据", Year));
            #region 批准批次土地面积
            var ratify = GetRatify(row, Year);
            if (ratify != null)
            {
                if (!DictRatify.ContainsKey(key))
                {
                    DictRatify.Add(key, new Dictionary<int, Ratify>() { { Year, ratify } });
                }
                else
                {
                    if (!DictRatify[key].ContainsKey(Year))
                    {
                        DictRatify[key].Add(Year, ratify);
                    }
                }
            }
            #endregion
            Console.WriteLine(string.Format("年份：{0} 的上级相关数据", Year));
            #region 上级相关数据

            var superior = GetSuperior(row, Year);
            if (superior != null)
            {
                if (!DictSuperior.ContainsKey(key))
                {
                    DictSuperior.Add(key, new Dictionary<int, Models.Superior>() { { Year, superior } });
                }
                else
                {
                    if (!DictSuperior[key].ContainsKey(Year))
                    {
                        DictSuperior[key].Add(Year, superior);
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
                //row = RowGet(sheet, count);//开始获取行对象Row
                row = sheet.GetRow(count);
                if (row == null)
                {
                    break;
                }
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
                    Console.WriteLine(string.Format("获得地区：{0} 省份：{1} 所在地级市：{2} 区域名称：{3}  区域代码：{4}", region.Zone, region.Province, region.BelongCity, region.Name, region.Code));
                    var key = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", region.Code, region.BelongCity,region.Name,region.Zone,region.Province,region.FactorCode,region.Evalutaor,region.Degree);
                    if (!string.IsNullOrEmpty(key) && !_dictRegions.ContainsKey(key))
                    {
                        _dictRegions.Add(key, region);
                    }
                    //if (!_regions.Contains(region))
                    //{
                    //    _regions.Add(region);
                    //}
                    int year = 0;
                    if(int.TryParse(value,out year))//获取int类型的年份数据
                    {
                        Console.WriteLine(string.Format("读取年份为{0}的相关数据", year));
                        GainForValue(row, year, key);
                        count++;
                    }
                }
            }
        }
        public void Save(ManagerCore core)
        {
            Save(core.ExcelManager);
        }

        public void Save(ExcelManager excelManager)
        {
            foreach (var region in _dictRegions)
            {
                Console.WriteLine(string.Format("开始导入地区：{0} 省份：{1} 所在地级市：{2} 区域名称：{3}  区域代码：{4}", region.Value.Zone, region.Value.Province, region.Value.BelongCity, region.Value.Name, region.Value.Code));
                var regionID = excelManager.GetID(region.Value);

                #region  保存People数据
                if (DictPeople.ContainsKey(region.Key))
                {
                    excelManager.Save(DictPeople[region.Key].Select(e => new People
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
                if (DictEconomy.ContainsKey(region.Key))
                {
                    excelManager.Save(DictEconomy[region.Key].Select(e => new Economy
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
                if (DictArgicultureLand.ContainsKey(region.Key))
                {
                    excelManager.Save(DictArgicultureLand[region.Key].Select(e => new AgricultureLand
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
                if (DictConstructionLand.ContainsKey(region.Key))
                {
                    excelManager.Save(DictConstructionLand[region.Key].Select(e => new ConstructionLand
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
                if (DictNewConstruction.ContainsKey(region.Key))
                {
                    excelManager.Save(DictNewConstruction[region.Key].Select(e => new NewConstruction
                    {
                        Construction = e.Value.Construction,
                        Town = e.Value.Town,
                        CID = regionID,
                        Year = e.Value.Year
                    }).ToList(), regionID);
                }
                #endregion

                #region 土地供应量数据
                if (DictLandSupply.ContainsKey(region.Key))
                {
                    excelManager.Save(DictLandSupply[region.Key].Select(e => new LandSupply
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
                if (DictRatify.ContainsKey(region.Key))
                {
                    excelManager.Save(DictRatify[region.Key].Select(e => new Ratify
                    {
                        Area = e.Value.Area,
                        Already = e.Value.Already,
                        Year = e.Value.Year,
                        RID = regionID
                    }).ToList(), regionID);
                }
                #endregion

                #region 保存上一级数据
                if (DictSuperior.ContainsKey(region.Key))
                {
                    excelManager.Save(DictSuperior[region.Key].Select(e => new Superior
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

        public void Save(FileManager fileManager)
        {
            foreach (var region in _dictRegions)
            {
                Console.WriteLine(string.Format("开始导入地区：{0} 省份：{1} 所在地级市：{2} 区域名称：{3}  区域代码：{4}", region.Value.Zone, region.Value.Province, region.Value.BelongCity, region.Value.Name, region.Value.Code));
                var regionID = fileManager.GetID(region.Value);

                #region  保存People数据
                if (DictPeople.ContainsKey(region.Key))
                {
                    fileManager.Save(DictPeople[region.Key].Select(e => new People
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
                if (DictEconomy.ContainsKey(region.Key))
                {
                    fileManager.Save(DictEconomy[region.Key].Select(e => new Economy
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
                if (DictArgicultureLand.ContainsKey(region.Key))
                {
                    fileManager.Save(DictArgicultureLand[region.Key].Select(e => new AgricultureLand
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
                if (DictConstructionLand.ContainsKey(region.Key))
                {
                    fileManager.Save(DictConstructionLand[region.Key].Select(e => new ConstructionLand
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
                if (DictNewConstruction.ContainsKey(region.Key))
                {
                    fileManager.Save(DictNewConstruction[region.Key].Select(e => new NewConstruction
                    {
                        Construction = e.Value.Construction,
                        Town = e.Value.Town,
                        CID = regionID,
                        Year = e.Value.Year
                    }).ToList(), regionID);
                }
                #endregion

                #region 土地供应量数据
                if (DictLandSupply.ContainsKey(region.Key))
                {
                    fileManager.Save(DictLandSupply[region.Key].Select(e => new LandSupply
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
                if (DictRatify.ContainsKey(region.Key))
                {
                    fileManager.Save(DictRatify[region.Key].Select(e => new Ratify
                    {
                        Area = e.Value.Area,
                        Already = e.Value.Already,
                        Year = e.Value.Year,
                        RID = regionID
                    }).ToList(), regionID);
                }
                #endregion

                #region 保存上一级数据
                if (DictSuperior.ContainsKey(region.Key))
                {
                    fileManager.Save(DictSuperior[region.Key].Select(e => new Superior
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