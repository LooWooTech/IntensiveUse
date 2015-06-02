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

        protected People SearchForPeople(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                People people = db.Peoples.FirstOrDefault(e => e.RID == ID && e.Year == Year);
                if (people == null)
                {
                    people = new People();
                }
                return people;
            }
        }

        public ConstructionLand SearchForConstruction(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                ConstructionLand construction = db.Constructions.FirstOrDefault(e => e.RID == ID && e.Year == Year);
                if (construction == null)
                {
                    construction = new ConstructionLand();
                }
                return construction;
            }
        }

        public Economy SearchForEconomy(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                Economy economy = db.Economys.FirstOrDefault(e => e.RID == ID && e.Year == Year);
                if (economy == null)
                {
                    economy = new Economy();
                }
                return economy;
            }
        }

        protected AgricultureLand SearchForAgriculture(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                AgricultureLand agriculture = db.Agricultures.FirstOrDefault(e => e.RID == ID && e.Year == Year);
                if (agriculture == null)
                {
                    agriculture = new AgricultureLand();
                }
                return agriculture;
            }
        }

        protected NewConstruction SearchForNewConstruction(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                NewConstruction newConstruction = db.NewConstructions.FirstOrDefault(e => e.CID == ID && e.Year == Year);
                if (newConstruction == null)
                {
                    newConstruction = new NewConstruction();
                }
                return newConstruction;
            }
        }

        protected LandSupply SearchForLandSupply(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                LandSupply landsupply = db.LandSupplys.FirstOrDefault(e => e.RID == ID && e.Year == Year);
                if (landsupply == null)
                {
                    landsupply = new LandSupply();
                }
                return landsupply;
            }
        }

        protected Ratify SearchForRatify(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                Ratify ratify = db.Ratifys.FirstOrDefault(e => e.RID == ID && e.Year == Year);
                if (ratify == null)
                {
                    ratify = new Ratify();
                }
                return ratify;
            }
        }

        public Exponent SearchForExponent(int Year, int ID, IdealType Type)
        {
            using (var db = GetIntensiveUseContext())
            {
                Exponent entity = db.Exponents.FirstOrDefault(e => e.Year == Year && e.RID == ID && e.Type == Type);
                if (entity == null)
                {
                    entity = new Exponent();
                }
                return entity;
            }
        }


        public SubIndex SearchForSubIndex(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                SubIndex entity = db.SubIndexs.FirstOrDefault(e => e.RID == ID && e.Year == Year);
                if (entity == null)
                {
                    entity = new SubIndex();
                }
                return entity;
            }
        }

        public IndexWeight SearchForIndexWeight(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                IndexWeight entity = db.IndexWeights.FirstOrDefault(e => e.RID == ID && e.Year == Year);
                if (entity == null)
                {
                    entity = new IndexWeight();
                }
                return entity;
            }
        }

        public void Gain<T>(T Complex, ref Queue<double> queue)
        {
            System.Reflection.PropertyInfo[] propList = typeof(T).GetProperties();
            double val = 0.0;

            foreach (var item in propList)
            {
                if (item.PropertyType.Equals(typeof(double)))
                {
                    val = 0.0;
                    double.TryParse(item.GetValue(Complex, null).ToString(), out val);
                    queue.Enqueue(val);
                }
            }
        }

        public Foundation SearchForFoundation(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                Foundation entity = db.Foundations.FirstOrDefault(e => e.RID == ID && e.Year == Year);
                if (entity == null)
                {
                    entity = new Foundation();
                }
                return entity;
            }
        }
    }
}