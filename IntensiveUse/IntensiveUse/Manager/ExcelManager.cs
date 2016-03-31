using IntensiveUse.Form;
using IntensiveUse.Helper;
using IntensiveUse.Models;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;
using System.Xml;

namespace IntensiveUse.Manager
{
    public class ExcelManager:ManagerBase
    {
        private static readonly object syncRoot = new object();
        private XmlDocument configXml { get; set; }
        private XmlDocument CityConfigXml { get; set; }
        public ExcelManager()
        {
            configXml = new XmlDocument();
            CityConfigXml = new XmlDocument();
            configXml.Load(ConfigurationManager.AppSettings["TABLE_FILE_PATH"]);
            CityConfigXml.Load(ConfigurationManager.AppSettings["CITY_FILE_PATH"]);
        }

        public IWorkbook ADownLoad(OutputExcel type,int year,string province,string belongCity,string name)
        {
            var tempfile = GetExcelPath(type.ToString()).GetAbsolutePath();
            var indexs = GetDigits(type.ToString());
            if (indexs == null)
            {
                throw new ArgumentException("未定义表格中单元格的精度问题");
            }
            ISchedule load = null;
            switch (type)
            {
                case OutputExcel.附表1A1:
                    load = new ScheduleAOne();
                    break;
                case OutputExcel.附表1A2:
                    load = new ScheduleATwo();
                    break;
                case OutputExcel.附表1A3:
                    load = new ScheduleAThree();
                    break;
                case OutputExcel.附表1A4:
                    load = new ScheduleAFour();
                    break;
                case OutputExcel.附表1A5:
                    load = new ScheduleAFive();
                    break;
                case OutputExcel.附表1A6:
                    load = new ScheduleASix();
                    break;
                case OutputExcel.附表1A7:
                    load = new ScheduleASeven();
                    break;
                case OutputExcel.附表1A8:
                    load = new ScheduleAEight();
                    break;
                case OutputExcel.附表1A9:
                    load = new ScheduleANine();
                    break;
                case OutputExcel.附表1A10:
                    load = new ScheduleATen();
                    break;
                case OutputExcel.附表1A11:
                    load = new ScheduleAEleven();
                    break;
                case OutputExcel.附表1A12:
                    load = new ScheduleATwelve();
                    break;
                case OutputExcel.附表1A13:
                    load = new ScheduleAThirteen();
                    break;
                case OutputExcel.附表1A14:
                    load = new ScheduleAFourteen();
                    break;
                case OutputExcel.附表1B1:
                    load = new ScheduleAOne();
                    break;
                case OutputExcel.附表1B2:
                    load = new ScheduleAThree();
                    break;
                case OutputExcel.附表1B3:
                    load = new ScheduleBThree();
                    break;
                case OutputExcel.附表1B4:
                    load = new ScheduleAFive();
                    break;
                case OutputExcel.附表1B5:
                    load = new ScheduleASix();
                    break;
                case OutputExcel.附表1B6:
                    load = new ScheduleASeven();
                    break;
                default: break;
            }
            IWorkbook workbook = load.AWrite(tempfile, Core, year, province, belongCity, name, indexs);
            return workbook;

        }

        /// <summary>
        /// 适用于区域评价下载
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Year"></param>
        /// <param name="City"></param>
        /// <param name="Distict"></param>
        /// <returns></returns>
        public IWorkbook DownLoad(OutputExcel Type,int Year,string City,string Distict)
        {
            string TempFile = GetExcelPath(Type.ToString()).GetAbsolutePath();
            int[] Indexs=GetDigits(Type.ToString());
            if(Indexs==null)
            {
                throw new ArgumentException("未定义表格中单元格的精度问题，请联系服务器管理员！");
            }
            ISchedule load = null;
            switch (Type)
            {
                case OutputExcel.附表1A1:
                    load = new ScheduleAOne();
                    break;
                case OutputExcel.附表1A2:
                    load = new ScheduleATwo();
                    break;
                case OutputExcel.附表1A3:
                    load = new ScheduleAThree();
                    break;
                case OutputExcel.附表1A4:
                    load = new ScheduleAFour();
                    break;
                case OutputExcel.附表1A5:
                    load = new ScheduleAFive();
                    break;
                case OutputExcel.附表1A6:
                    load = new ScheduleASix();
                    break;
                case OutputExcel.附表1A7:
                    load = new ScheduleASeven();
                    break;
                case OutputExcel.附表1A8:
                    load = new ScheduleAEight();
                    break;
                case OutputExcel.附表1A9:
                    load = new ScheduleANine();
                    break;
                case OutputExcel.附表1A10:
                    load = new ScheduleATen();
                    break;
                case OutputExcel.附表1A11:
                    load = new ScheduleAEleven();
                    break;
                case OutputExcel.附表1A12:
                    load = new ScheduleATwelve();
                    break;
                case OutputExcel.附表1A13:
                    load = new ScheduleAThirteen();
                    break;
                case OutputExcel.附表1A14:
                    load = new ScheduleAFourteen();
                    break;
                case OutputExcel.附表1B1:
                    load = new ScheduleAOne();
                    break;
                case OutputExcel.附表1B2:
                    load = new ScheduleAThree();
                    break;
                case OutputExcel.附表1B3:
                    load = new ScheduleBThree();
                    break;
                case OutputExcel.附表1B4:
                    load = new ScheduleAFive();
                    break;
                case OutputExcel.附表1B5:
                    load = new ScheduleASix();
                    break;
                case OutputExcel.附表1B6:
                    load = new ScheduleASeven();
                    break;
                default: break;
            }
            IWorkbook workbook = load.Write(TempFile, Core,Year, City,Distict,Indexs);
            return workbook;
        }
        public string GetExcelPath(string excelName)
        {
            var node = configXml.SelectSingleNode("/Tables/Table[@Title='" + excelName + "']/File");
            if (node == null)
            {
                throw new ArgumentException("配置文件中未获取"+excelName+"相关信息");
            }
            return node.Attributes["Path"].Value;
        }
        public int[] GetDigits(string excelName)
        {
            var nodes = configXml.SelectNodes("/Tables/Table[@Title='" + excelName + "']/Digits/Digit");
            
            if (nodes != null)
            {
                int[] Indexs = new int[nodes.Count];
                for (var i = 0; i < nodes.Count; i++)
                {
                    int.TryParse(nodes[i].Attributes["Type"].Value, out Indexs[i]);
                }
                return Indexs;
            }
            return null;
        }
        public bool Exit(string Name)
        {
            var node = CityConfigXml.SelectSingleNode("/Citys/Province/City[@Name='" + Name + "']");
            if (node != null)
            {
                return true;
            }
            var nodes = CityConfigXml.SelectNodes("/Citys/Province/City/Division");
            string val = string.Empty;
            for (var i = 0; i < nodes.Count; i++)
            {
                var n = nodes[i];
                val = n.Attributes["Name"].Value;
                if (val.ToLower() == Name.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
        public List<string> GetCity()
        {
            List<string> Citys = new List<string>();
            var nodes = CityConfigXml.SelectNodes("/Citys/Province/City");
            if (nodes == null)
            {
                throw new ArgumentException("地级市配置文件中未获取相关信息");
            }
            for (var i = 0; i < nodes.Count; i++)
            {
                var n = nodes[i];
                Citys.Add(n.Attributes["Name"].Value);
            }
            return Citys;
        }
        public List<string> GetProvinces()
        {
            var list = new List<string>();
            var nodes = CityConfigXml.SelectNodes("/Citys/Province");
            if (nodes == null)
            {
                throw new ArgumentException("配置文件错误");
            }
            for (var i = 0; i < nodes.Count; i++)
            {
                list.Add(nodes[i].Attributes["Name"].Value);
            }
            
            return list;
        }
        public Dictionary<string, List<string>> GetCityDict(string province)
        {
            var dict = new Dictionary<string, List<string>>();
            var nodes = CityConfigXml.SelectNodes("/Citys/Province[@Name='" + province + "']/City");
            if (nodes != null)
            {
                for (var i = 0; i < nodes.Count; i++)
                {
                    var name = nodes[i].Attributes["Name"].Value;
                    if (!dict.ContainsKey(name))
                    {
                        dict.Add(name, GetDistrict(name));
                    }
                }
            }
            return dict;
        }
        public string GetCSuperior(string City)
        {
            var node = CityConfigXml.SelectSingleNode("/Citys/Province/City[@Name='" + City + "']");
            if (node == null)
            {
                throw new ArgumentException("未获取" + City + "所辖区相关信息");
            }
            var pNode = node.ParentNode;
            return pNode.Attributes["Name"].Value;
        }
        public string GetDSuperior(string Distict)
        {
            var node = CityConfigXml.SelectSingleNode("/Citys/Province/City/Division[@Name='" + Distict + "']");
            if (node == null)
            {
                throw new ArgumentException("未获取县级市"+Distict+"相关信息");
            }
            var pNode = node.ParentNode;
            return pNode.Attributes["Name"].Value;
        }
        public Dictionary<string, Dictionary<string, List<string[]>>> GetAllProvinces()
        {
            var dict = new Dictionary<string, Dictionary<string, List<string[]>>>();
            var list = GetProvinces();
            foreach (var item in list)
            {
                if (!dict.ContainsKey(item))
                {
                    dict.Add(item, GetCityDict(item).DictToTable());
                }
            }
            return dict;
        } 
        public List<string> GetDistrict(string City)
        {
            List<string> Disticts = new List<string>();
            var node = CityConfigXml.SelectSingleNode("/Citys/Province/City[@Name='" + City + "']");
            if (node == null)
            {
                throw new ArgumentException("未获取"+City+"所辖区相关信息");
            }
            var nodes = node.SelectNodes("Division");
            for (var i = 0; i < nodes.Count; i++)
            {
                var n = nodes[i];
                Disticts.Add(n.Attributes["Name"].Value);
            }
            return Disticts;
        }
        public string GetStrDistict(string City)
        {
            List<string> Distict = GetDistrict(City);
            string str = string.Empty;
            foreach (var item in Distict)
            {
                if (string.IsNullOrEmpty(str))
                {
                    str += item;
                }
                else
                {
                    str += "、" + item;
                }
            }
            return str;
        }
        public Region Find(string Name)
        {
            using (var db = GetIntensiveUseContext())
            {
                return db.Regions.Where(e => e.Name.ToLower() == Name.ToLower()).FirstOrDefault();
            }
        }
        public Region Find(int id)
        {
            using (var db = GetIntensiveUseContext())
            {
                return db.Regions.Find(id);
            }
        }
        public Region Find(Region region)
        {
            using (var db = GetIntensiveUseContext())
            {
                return db.Regions.Where(e => e.Zone == region.Zone && e.Province == region.Province && e.BelongCity == region.BelongCity && e.Evalutaor == e.FactorCode && e.Name == region.Name && e.Code == region.Code && e.Degree == region.Degree).FirstOrDefault();
            }
        }
        public int Add(Region region)
        {
            using (var db = GetIntensiveUseContext())
            {
                db.Regions.Add(region);
                db.SaveChanges();
            }
            return region.ID;
        }
        //public void Save(Dictionary<string, Economy> DICT, int ID)
        //{
        //    using (var db = GetIntensiveUseContext())
        //    {
        //        foreach (var item in DICT.Keys)
        //        {
        //            Economy temp = db.Economys.FirstOrDefault(e => e.Year.ToLower() == item.ToLower() && e.RID == ID);
        //            if (temp == null)
        //            {
        //                temp = DICT[item];
        //                db.Economys.Add(temp);
        //            }
        //            else
        //            {
        //                DICT[item].ID = temp.ID;
        //                db.Entry(temp).CurrentValues.SetValues(DICT[item]);
        //            }
        //            db.SaveChanges();
        //        }
        //    }
        //}

        //public void Save(Dictionary<string, AgricultureLand> DICT, int ID)
        //{
        //    using (var db = GetIntensiveUseContext())
        //    {
        //        foreach (var item in DICT.Keys)
        //        {
        //            AgricultureLand entity = db.Agricultures.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == item.ToLower());
        //            if (entity == null)
        //            {
        //                entity = DICT[item];
        //                db.Agricultures.Add(entity);
        //            }
        //            else
        //            {
        //                DICT[item].ID = entity.ID;
        //                db.Entry(entity).CurrentValues.SetValues(DICT[item]);
        //            }
        //            db.SaveChanges();
        //        }
        //    }
        //}

        //public void Save(Dictionary<string, ConstructionLand> DICT, int ID)
        //{
        //    using (var db = GetIntensiveUseContext())
        //    {
        //        foreach (var item in DICT.Keys)
        //        {
        //            ConstructionLand entity = db.Constructions.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == item.ToLower());
        //            if (entity == null)
        //            {
        //                entity = DICT[item];
        //                db.Constructions.Add(entity);
        //            }
        //            else
        //            {
        //                DICT[item].ID = entity.ID;
        //                db.Entry(entity).CurrentValues.SetValues(DICT[item]);
        //            }
        //            db.SaveChanges();
        //        }
        //    }
        //}
        //public void Save(Dictionary<string, NewConstruction> DICT, int ID)
        //{
        //    using (var db = GetIntensiveUseContext())
        //    {
        //        foreach (var item in DICT.Keys)
        //        {
        //            NewConstruction entity = db.NewConstructions.FirstOrDefault(e => e.CID == ID && e.Year.ToLower() == item.ToLower());
        //            if (entity == null)
        //            {
        //                entity = DICT[item];
        //                db.NewConstructions.Add(entity);
        //            }
        //            else
        //            {
        //                DICT[item].ID = entity.ID;
        //                db.Entry(entity).CurrentValues.SetValues(DICT[item]);
        //            }
        //            db.SaveChanges();
        //        }
        //    }
        //}

        //public void Save(Dictionary<string, LandSupply> DICT, int ID)
        //{
        //    using (var db = GetIntensiveUseContext())
        //    {
        //        foreach (var item in DICT.Keys)
        //        {
        //            LandSupply entity = db.LandSupplys.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == item.ToLower());
        //            if (entity == null)
        //            {
        //                entity = DICT[item];
        //                db.LandSupplys.Add(entity);
        //            }
        //            else
        //            {
        //                DICT[item].ID = entity.ID;
        //                db.Entry(entity).CurrentValues.SetValues(DICT[item]);
        //            }
        //            db.SaveChanges();
        //        }
        //    }
        //}


        //public void Save(Dictionary<string, Ratify> DICT, int ID)
        //{
        //    using (var db = GetIntensiveUseContext())
        //    {
        //        foreach (var item in DICT.Keys)
        //        {
        //            Ratify entity = db.Ratifys.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == item.ToLower());
        //            if (entity == null)
        //            {
        //                entity = DICT[item];
        //                db.Ratifys.Add(entity);
        //            }
        //            else
        //            {
        //                DICT[item].ID = entity.ID;
        //                db.Entry(entity).CurrentValues.SetValues(DICT[item]);
        //            }
        //            db.SaveChanges();
        //        }
        //    }
        //}
        public void Save<T>(Dictionary<int, T> DICT, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                foreach (var item in DICT.Keys)
                {
                    T temp = DICT[item];
                    if (temp is Ratify)
                    {
                        Ratify m = temp as Ratify;
                        Ratify entity = db.Ratifys.FirstOrDefault(e => e.RID == ID && e.Year == item);
                        if (entity == null)
                        {
                            entity = m;
                            db.Ratifys.Add(entity);
                        }
                        else
                        {
                            m.ID = entity.ID;
                            db.Entry(entity).CurrentValues.SetValues(m);
                        }
                    }
                    else if (temp is AgricultureLand)
                    {
                        AgricultureLand m = temp as AgricultureLand;
                        AgricultureLand entity = db.Agricultures.FirstOrDefault(e => e.RID == ID && e.Year == item);
                        if (entity == null)
                        {
                            entity = m;
                            db.Agricultures.Add(entity);
                        }
                        else
                        {
                            m.ID = entity.ID;
                            db.Entry(entity).CurrentValues.SetValues(m);
                        }
                    }
                    else if (temp is ConstructionLand)
                    {
                        ConstructionLand m = temp as ConstructionLand;
                        ConstructionLand entity = db.Constructions.FirstOrDefault(e => e.RID == ID && e.Year == item);
                        if (entity == null)
                        {
                            entity = m;
                            db.Constructions.Add(entity);
                        }
                        else
                        {
                            m.ID = entity.ID;
                            db.Entry(entity).CurrentValues.SetValues(m);
                        }
                    }
                    else if (temp is LandSupply)
                    {
                        LandSupply m = temp as LandSupply;
                        LandSupply entity = db.LandSupplys.FirstOrDefault(e => e.RID == ID && e.Year == item);
                        if (entity == null)
                        {
                            entity = m;
                            db.LandSupplys.Add(entity);
                        }
                        else
                        {
                            m.ID = entity.ID;
                            db.Entry(entity).CurrentValues.SetValues(m);
                        }
                    }else if(temp is NewConstruction){
                        NewConstruction m = temp as NewConstruction;
                        NewConstruction entity = db.NewConstructions.FirstOrDefault(e => e.CID == ID && e.Year == item);
                        if (entity == null)
                        {
                            entity = m;
                            db.NewConstructions.Add(entity);
                        }
                        else
                        {
                            m.ID = entity.ID;
                            db.Entry(entity).CurrentValues.SetValues(m);
                        }
                    }
                    else if (temp is Economy)
                    {
                        Economy m = temp as Economy;
                        Economy entity = db.Economys.FirstOrDefault(e => e.RID == ID && e.Year == item);
                        if (entity == null)
                        {
                            entity = m;
                            db.Economys.Add(entity);
                        }
                        else
                        {
                            m.ID = entity.ID;
                            db.Entry(entity).CurrentValues.SetValues(m);
                        }
                    }
                    else if (temp is People)
                    {
                        People m = temp as People;
                        People entity = db.Peoples.FirstOrDefault(e => e.RID == ID && e.Year== item);
                        if (entity == null)
                        {
                            entity = m;
                            db.Peoples.Add(entity);
                        }
                        else
                        {
                            m.ID = entity.ID;
                            db.Entry(entity).CurrentValues.SetValues(m);
                        }
                    }

                    db.SaveChanges();
                }
            }
        }
        public void Save<T>(List<T> list,int rid)
        {
            using (var db = GetIntensiveUseContext())
            {
                foreach(var t in list)
                {
                    if(t is People)
                    {
                        People m = t as People;
                        var entry = db.Peoples.FirstOrDefault(e => e.RID == rid && e.Year == m.Year);
                        if (entry == null)
                        {
                            db.Peoples.Add(m);
                        }
                        else
                        {
                            m.ID = entry.ID;
                            db.Entry(entry).CurrentValues.SetValues(m);
                        }
                    }else if(t is Economy)
                    {
                        var m = t as Economy;
                        var entry = db.Economys.FirstOrDefault(e => e.RID == rid && e.Year == m.Year);
                        if (entry == null)
                        {
                            db.Economys.Add(m);
                        }
                        else
                        {
                            m.ID = entry.ID;
                            db.Entry(entry).CurrentValues.SetValues(m);
                        }
                    }else if( t is AgricultureLand)
                    {
                        var m = t as AgricultureLand;
                        var entry = db.Agricultures.FirstOrDefault(e => e.RID == rid && e.Year == m.Year);

                        if (entry == null)
                        {
                            db.Agricultures.Add(m);
                        }
                        else
                        {
                            m.ID = entry.ID;
                            db.Entry(entry).CurrentValues.SetValues(m);
                        }
                    }else if( t is ConstructionLand)
                    {
                        var m = t as ConstructionLand;
                        var entry = db.Constructions.FirstOrDefault(e => e.RID == rid && e.Year == m.Year);
                        if (entry == null)
                        {
                            db.Constructions.Add(m);
                        }
                        else
                        {
                            m.ID = entry.ID;
                            db.Entry(entry).CurrentValues.SetValues(m);
                        }
                    }else if(t is NewConstruction)
                    {
                        var m = t as NewConstruction;
                        var entry = db.NewConstructions.FirstOrDefault(e => e.CID == rid && e.Year == m.Year);
                        if (entry == null)
                        {
                            db.NewConstructions.Add(m);
                        }
                        else
                        {
                            m.ID = entry.ID;
                            db.Entry(entry).CurrentValues.SetValues(m);
                        }
                    }else if(t is LandSupply)
                    {
                        var m = t as LandSupply;
                        var entry = db.LandSupplys.FirstOrDefault(e => e.RID == rid && e.Year == m.Year);
                        if (entry == null)
                        {
                            db.LandSupplys.Add(m);
                        }
                        else
                        {
                            m.ID = entry.ID;
                            db.Entry(entry).CurrentValues.SetValues(m);
                        }
                    }else if(t is Ratify)
                    {
                        var m = t as Ratify;
                        var entry = db.Ratifys.FirstOrDefault(e => e.RID == rid && e.Year == m.Year);
                        if (entry == null)
                        {
                            db.Ratifys.Add(m);
                        }
                        else
                        {
                            m.ID = entry.ID;
                            db.Entry(entry).CurrentValues.SetValues(m);
                        }
                    }else if(t is Superior)
                    {
                        var m = t as Superior;
                        var entry = db.Superiors.FirstOrDefault(e => e.RID == rid && e.Year == m.Year);
                        if (entry == null)
                        {
                            db.Superiors.Add(m);
                        }
                        else
                        {
                            m.ID = entry.ID;
                            db.Entry(entry).CurrentValues.SetValues(m);
                        }
                    }

                    db.SaveChanges();
                }
            }
        }
        public int GetID(string Name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                return 0;
            }
            Region region = Find(Name);
            if (region == null)
            {
                return Add(new Region { Name = Name });
            }
            else
            {
                return region.ID;
            }
        }
        public int GetID(Region region)
        {
            if (region == null) return 0;
            var entry = Find(region);
            if (entry == null)
            {
                return Add(region);
            }
            else
            {
                return entry.ID;
            }
        }
        public int GetSuperiorCID(string City)
        {
            var superior = GetCSuperior(City);
            return GetID(superior);
        }

        public int GetSuperiorDID(string Distict)
        {
            var superior = GetDSuperior(Distict);
            return GetID(superior);
        }

        public void  Delete(int Year, int ID)
        {
            try
            {
                Core.PeopleManager.Delete(Year, ID);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("删除人口数据失败！请确保是否重复删除"+ex.ToString());
            }
            try
            {
                Core.EconmoyManager.Delete(Year, ID);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("删除经济数据失败！请确保是否重复删除"+ex.ToString());
            }
            try
            {
                Core.AgricultureManager.Delete(Year, ID);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("删除土地利用现状数据——农用地数据失败!请确保是否重复删除"+ex.ToString());
            }

            try
            {
                Core.ConstructionLandManager.Delete(Year, ID);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("删除土地利用现状数据——建设用地失败！请确保是否重复删除"+ex.ToString());
            }

            try
            {
                Core.NewConstructionManager.Delete(Year, ID);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("删除土地利用现状数据——新增建设用地失败！请确保是否删除" + ex.ToString());
            }

            try
            {
                Core.LandSupplyManager.Delete(Year, ID);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("删除土地供应数据——土地供应量数据失败！请确保是否重复删除" + ex.ToString());
            }

            try
            {
                Core.LandUseManager.Delete(Year, ID);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("删除土地供应数据——批准批次土地供应数据失败！请确保是否重复删除" + ex.ToString());
            }

            try
            {
                Core.ExponentManager.Delete(Year, ID);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("删除指标权重以及指标理想值失败！"+ex.ToString());
            }


            try
            {
                Core.IndexManager.Delete(Year, ID);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("删除指数权重数据失败！"+ex.ToString());
            }

            try
            {
                Core.SubIndexManager.Delete(Year, ID);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("删除分指数权重数据失败！"+ex.ToString());
            }

            try
            {
                Core.StatisticsManager.Delete(Year, ID);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("删除上传记录失败"+ex.ToString());
            }

           // return true;
        }

        public DataSet GetDataSet(Exponent exponent,LandUseChange landUseChange,int year,Region region)
        {
            var dataSet = new DataSet();
            var people1 = SearchForPeople(year, region.ID);//14年的人口数据
            var people2 = SearchForPeople(year - 3, region.ID);//11年的人口数据
            var construction1 = SearchForConstruction(year, region.ID);
            var construction2 = SearchForConstruction(year - 3, region.ID);
            #region  获取 土地利用趋势类型 人口
            if (people1.PermanentSum > people2.PermanentSum)
            {
                if (construction1.TowCouConstruction <= construction2.TowCouConstruction)
                {
                    dataSet.People = Tendency.内涵挖潜型;
                }
                else if (exponent.PEI1 > 1)
                {
                    dataSet.People = Tendency.集约趋势型;
                }
                else if (Math.Abs(exponent.PEI1 - 1) < 0.000001)
                {
                    dataSet.People = Tendency.相对稳定型;
                }
                else
                {
                    dataSet.People= Tendency.粗放趋势型;
                }
                
            }
            else if (Math.Abs(people1.PermanentSum - people2.PermanentSum) < 0.0001)
            {
                if (construction1.TowCouConstruction > construction2.TowCouConstruction)
                {
                    dataSet.People= Tendency.粗放趋势型;
                }
                else if (Math.Abs(construction1.TowCouConstruction - construction2.TowCouConstruction) < 0.000001)
                {
                    dataSet.People= Tendency.相对稳定型;
                }
                else
                {
                    dataSet.People= Tendency.集约趋势型;
                }
            }
            else
            {
                if (construction1.TowCouConstruction >= construction2.TowCouConstruction)
                {
                    dataSet.People= Tendency.粗放趋势型;
                }
                else if (exponent.PEI1 > 1)
                {
                    dataSet.People= Tendency.粗放趋势型;
                }
                else if (Math.Abs(exponent.PEI1 - 1) < 0.000001)
                {
                    dataSet.People= Tendency.相对稳定型;
                }
                else
                {
                    dataSet.People= Tendency.集约趋势型;
                }
            }
            #endregion
            double r, rmax = .0;
            var superior1 = SearchForSuperior(year, region.ID);
            var superior2 = SearchForSuperior(year - 3, region.ID);
            var economy1 = SearchForEconomy(year, region.ID);
            var economy2 = SearchForEconomy(year - 3, region.ID);
            if (region.Evalutaor.Trim() == "地级以上城市辖区整体")
            {
                r = ((superior1.ProvinceCompare - superior2.ProvinceCompare) / superior2.ProvinceCompare) / ((superior1.ProvinceConstruction - superior2.ProvinceConstruction) / superior2.ProvinceConstruction);
            }
            else
            {
                r = ((superior1.CityCompare - superior2.CityCompare) / superior2.CityCompare) / ((superior1.CityConstruction - superior2.CityConstruction) / superior2.CityConstruction);
            }
            rmax = r > 1 ? r : 1;

            #region  获取土地利用趋势类型 经济

            if (economy1.Compare > economy2.Compare)
            {
                if (construction1.SubTotal <= construction2.SubTotal)
                {
                    dataSet.Economy = Tendency.内涵挖潜型;
                }else if ((landUseChange.EEI1 > rmax && landUseChange.ECI1 >= 1) || ((Math.Abs(landUseChange.EEI1 - rmax) < 0.00001) && landUseChange.ECI1 > 1))
                {
                    dataSet.Economy = Tendency.集约趋势型;
                }else if ((Math.Abs(landUseChange.EEI1 - rmax) < 0.000001) && (Math.Abs(landUseChange.ECI1 - 1) < 0.00001))
                {
                    dataSet.Economy = Tendency.相对稳定型;
                }
                else
                {
                    dataSet.Economy = Tendency.粗放趋势型;
                }
            }else if (Math.Abs(economy1.Compare - economy2.Compare) < 0.0001)
            {
                if (construction1.SubTotal < construction2.SubTotal)
                {
                    dataSet.Economy = Tendency.集约趋势型;
                }else if (Math.Abs(construction1.SubTotal - construction2.SubTotal) < 0.000001)
                {
                    dataSet.Economy = Tendency.相对稳定型;
                }
                else
                {
                    dataSet.Economy = Tendency.粗放趋势型;
                }
            }
            else
            {
                if (construction1.SubTotal >= construction2.SubTotal)
                {
                    dataSet.Economy = Tendency.粗放趋势型;
                }else if ((landUseChange.EEI1 > rmax) || (landUseChange.ECI1 > 1))
                {
                    dataSet.Economy = Tendency.粗放趋势型;
                }else if ((Math.Abs(landUseChange.EEI1 - rmax) < 0.000001) && (Math.Abs(landUseChange.ECI1 - 1) < 0.000001))
                {
                    dataSet.Economy = Tendency.相对稳定型;
                }
                else
                {
                    dataSet.Economy = Tendency.集约趋势型;
                }
            }
            #endregion
            return dataSet;
        }

    }
}