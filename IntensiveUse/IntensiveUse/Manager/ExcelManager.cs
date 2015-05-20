using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace IntensiveUse.Manager
{
    public class ExcelManager:ManagerBase
    {
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
                        
                        //db.Entry(DICT[item]).State = EntityState.Modified;
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
                        //db.Entry(DICT[item]).State = EntityState.Modified;
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