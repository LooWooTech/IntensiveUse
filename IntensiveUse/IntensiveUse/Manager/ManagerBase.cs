using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class ManagerBase
    {
        protected ManagerCore Core = ManagerCore.Instance;

        protected IUDbContext GetIntensiveUseContext()
        {
            var db = new IUDbContext();
            db.Database.Connection.Open();
            return db;
        }

        protected People SearchForPeople(string Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                People people = db.Peoples.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == Year.ToLower());
                if (people == null)
                {
                    people = new People();
                }
                return people;
            }
        }

        public ConstructionLand SearchForConstruction(string Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                ConstructionLand construction = db.Constructions.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == Year.ToLower());
                if (construction == null)
                {
                    construction = new ConstructionLand();
                }
                return construction;
            }
        }

        public Economy SearchForEconomy(string Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                Economy economy = db.Economys.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == Year.ToLower());
                if (economy == null)
                {
                    economy = new Economy();
                }
                return economy;
            }
        }

        protected AgricultureLand SearchForAgriculture(string Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                AgricultureLand agriculture = db.Agricultures.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == Year.ToLower());
                if (agriculture == null)
                {
                    agriculture = new AgricultureLand();
                }
                return agriculture;
            }
        }

        protected NewConstruction SearchForNewConstruction(string Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                NewConstruction newConstruction = db.NewConstructions.FirstOrDefault(e => e.CID == ID && e.Year.ToLower() == Year.ToLower());
                if (newConstruction == null)
                {
                    newConstruction = new NewConstruction();
                }
                return newConstruction;
            }
        }

        protected LandSupply SearchForLandSupply(string Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                LandSupply landsupply = db.LandSupplys.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == Year.ToLower());
                if (landsupply == null)
                {
                    landsupply = new LandSupply();
                }
                return landsupply;
            }
        }

        protected Ratify SearchForRatify(string Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                Ratify ratify = db.Ratifys.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == Year.ToLower());
                if (ratify == null)
                {
                    ratify = new Ratify();
                }
                return ratify;
            }
        }
    }
}