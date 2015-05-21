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
    public class ExcelManager:ManagerBase,IDisposable
    {
        private static readonly object syncRoot = new object();

        private XmlDocument configXml;
        public ExcelManager()
        {
            configXml = new XmlDocument();
            configXml.Load(ConfigurationManager.AppSettings["TABLE_FILE_PATH"]);
        }
        public IWorkbook DownLoad(OutputExcel Type)
        {
            string TempFile = GetExcelPath(Type.ToString());
            IWorkbook workbook = ExcelHelper.OpenWorkbook(TempFile);
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet == null)
            {
                throw new ArgumentException("未获取相关sheet信息");
            }
            Read(ref workbook);
            return workbook;
        }

        public void Read(ref IWorkbook workbook)
        {

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

        public void Save(Dictionary<string, People> DICT,int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                foreach (var item in DICT.Keys)
                {
                    People temp = db.Peoples.FirstOrDefault(e => e.Year.ToLower() == item.ToLower()&&e.RID==ID);
                    if (temp == null)
                    {
                        temp = DICT[item];
                        db.Peoples.Add(temp);
                    }
                    else
                    {
                        DICT[item].ID = temp.ID;
                        db.Entry(temp).CurrentValues.SetValues(DICT[item]);
                    }
                    db.SaveChanges();
                }
            }
        }
        public void Save(Dictionary<string, Economy> DICT, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                foreach (var item in DICT.Keys)
                {
                    Economy temp = db.Economys.FirstOrDefault(e => e.Year.ToLower() == item.ToLower() && e.RID == ID);
                    if (temp == null)
                    {
                        temp = DICT[item];
                        db.Economys.Add(temp);
                    }
                    else
                    {
                        DICT[item].ID = temp.ID;
                        db.Entry(temp).CurrentValues.SetValues(DICT[item]);
                    }
                    db.SaveChanges();
                }
            }
        }

        public void Save(Dictionary<string, AgricultureLand> DICT, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                foreach (var item in DICT.Keys)
                {
                    AgricultureLand entity = db.Agricultures.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == item.ToLower());
                    if (entity == null)
                    {
                        entity = DICT[item];
                        db.Agricultures.Add(entity);
                    }
                    else
                    {
                        DICT[item].ID = entity.ID;
                        db.Entry(entity).CurrentValues.SetValues(DICT[item]);
                    }
                    db.SaveChanges();
                }
            }
        }

        public void Save(Dictionary<string, ConstructionLand> DICT, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                foreach (var item in DICT.Keys)
                {
                    ConstructionLand entity = db.Constructions.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == item.ToLower());
                    if (entity == null)
                    {
                        entity = DICT[item];
                        db.Constructions.Add(entity);
                    }
                    else
                    {
                        DICT[item].ID = entity.ID;
                        db.Entry(entity).CurrentValues.SetValues(DICT[item]);
                    }
                    db.SaveChanges();
                }
            }
        }
        public void Save(Dictionary<string, NewConstruction> DICT, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                foreach (var item in DICT.Keys)
                {
                    NewConstruction entity = db.NewConstructions.FirstOrDefault(e => e.CID == ID && e.Year.ToLower() == item.ToLower());
                    if (entity == null)
                    {
                        entity = DICT[item];
                        db.NewConstructions.Add(entity);
                    }
                    else
                    {
                        DICT[item].ID = entity.ID;
                        db.Entry(entity).CurrentValues.SetValues(DICT[item]);
                    }
                    db.SaveChanges();
                }
            }
        }

        public void Save(Dictionary<string, LandSupply> DICT, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                foreach (var item in DICT.Keys)
                {
                    LandSupply entity = db.LandSupplys.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == item.ToLower());
                    if (entity == null)
                    {
                        entity = DICT[item];
                        db.LandSupplys.Add(entity);
                    }
                    else
                    {
                        DICT[item].ID = entity.ID;
                        db.Entry(entity).CurrentValues.SetValues(DICT[item]);
                    }
                    db.SaveChanges();
                }
            }
        }


        public void Save(Dictionary<string, Ratify> DICT, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                foreach (var item in DICT.Keys)
                {
                    Ratify entity = db.Ratifys.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == item.ToLower());
                    if (entity == null)
                    {
                        entity = DICT[item];
                        db.Ratifys.Add(entity);
                    }
                    else
                    {
                        DICT[item].ID = entity.ID;
                        db.Entry(entity).CurrentValues.SetValues(DICT[item]);
                    }
                    db.SaveChanges();
                }
            }
        }

        //public void Save<T>(Dictionary<string, T> DICT, int ID)
        //{
        //    using (var db = GetIntensiveUseContext())
        //    {
        //        foreach (var item in DICT.Keys)
        //        {
        //            T temp = DICT[item];
        //            if (temp is Ratify)
        //            {
        //                Ratify m = temp as Ratify;
        //                Ratify entity = db.Ratifys.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == item.ToLower());
        //                if (entity == null)
        //                {
        //                    entity = m;
        //                    db.Ratifys.Add(entity);
        //                }
        //                else
        //                {
        //                    m.ID = entity.ID;
        //                    db.Entry(entity).CurrentValues.SetValues(m);
        //                }
        //                db.SaveChanges();

        //            }
        //            else if (temp is AgricultureLand)
        //            {
        //                //AgricultureLand m = temp as AgricultureLand;

        //            }
        //            else if (temp is ConstructionLand)
        //            {

        //            }
        //            else if (temp is LandSupply)
        //            {

        //            }
        //            //object entity;
        //            //switch (DICT.GetType().Name)
        //            //{
        //            //    case "Dictionary<string,Ratify>":
        //            //        entity = db.Ratifys.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == item.ToLower());
        //            //        if (entity == null)
        //            //        {
        //            //           entity = DICT[item] as Ratify;
        //            //           db.Ratifys.Add(entity);
        //            //        }
        //            //        else
        //            //        {
                                
        //            //        }
        //            //        break;
        //            //    default:
        //            //        break;
        //            //}
        //        }
        //    }
        //}


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