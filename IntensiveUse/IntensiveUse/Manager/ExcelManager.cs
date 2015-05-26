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

        private XmlDocument configXml;
        private XmlDocument CityConfigXml;
        public ExcelManager()
        {
            configXml = new XmlDocument();
            CityConfigXml = new XmlDocument();
            configXml.Load(ConfigurationManager.AppSettings["TABLE_FILE_PATH"]);
            CityConfigXml.Load(ConfigurationManager.AppSettings["CITY_FILE_PATH"]);
        }
        public IWorkbook DownLoad(OutputExcel Type,int Year,string City)
        {
            string TempFile = GetExcelPath(Type.ToString()).GetAbsolutePath();
            ISchedule load = null;
            switch (Type)
            {
                case OutputExcel.附表1A1:
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
                default: break;
            }
            IWorkbook workbook = load.Write(TempFile, Core,Year, City);
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


        public List<string> GetCity()
        {
            List<string> Citys = new List<string>();
            var nodes = CityConfigXml.SelectNodes("/Citys/City");
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

        public List<string> GetDistrict(string City)
        {
            List<string> Disticts = new List<string>();
            var node = CityConfigXml.SelectSingleNode("/Citys/City[@Name='" + City + "']");
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

        public Region Find(string Name)
        {
            using (var db = GetIntensiveUseContext())
            {
                return db.Regions.Where(e => e.Name.ToLower() == Name.ToLower()).FirstOrDefault();
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

        public void Save<T>(Dictionary<string, T> DICT, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                foreach (var item in DICT.Keys)
                {
                    T temp = DICT[item];
                    if (temp is Ratify)
                    {
                        Ratify m = temp as Ratify;
                        Ratify entity = db.Ratifys.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == item.ToLower());
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
                        AgricultureLand entity = db.Agricultures.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == item.ToLower());
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
                        ConstructionLand entity = db.Constructions.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == item.ToLower());
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
                        LandSupply entity = db.LandSupplys.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == item.ToLower());
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
                        NewConstruction entity = db.NewConstructions.FirstOrDefault(e => e.CID == ID && e.Year.ToLower() == item.ToLower());
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
                        Economy entity = db.Economys.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == item.ToLower());
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
                        People entity = db.Peoples.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == item.ToLower());
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


        public List<string> GetFourYears()
        {
            List<string> Years = new List<string>();
            for (var i = 0; i < 4; i++)
            {
                Years.Add((DateTime.Now.Year - i).ToString());
            }
            return Years;
        }

        public int GetID(string Name)
        {
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
    }
}