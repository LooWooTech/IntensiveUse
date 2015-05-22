using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class LandUseManager:ManagerBase
    {

        public List<double> Get(string Year, int ID)
        {
            double[] data1 = GetAgriculture(Year,ID);
            double[] data2 = GetConstruction(Year, ID);
            double[] data3 = GetNewConstruction(Year,ID);
            int count = data1.Count() + data2.Count() + data3.Count();
            List<double> Data = new List<double>();
            foreach (var item in data1)
            {
                Data.Add(item);
            }
            foreach (var item in data2)
            {
                Data.Add(item);
            }
            foreach (var item in data3)
            {
                Data.Add(item);
            }
            return Data;
        }

        public double[] GetAgriculture(string Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                AgricultureLand agricultureland = db.Agricultures.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == Year.ToLower());
                if (agricultureland == null)
                {
                    agricultureland = new AgricultureLand();
                }
                return new double[] { agricultureland.Subtotal, agricultureland.Arable, agricultureland.Garden, agricultureland.Forest, agricultureland.Meadow, agricultureland.Other };
            }
        }
        public double[] GetConstruction(string Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                ConstructionLand constructionland = db.Constructions.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == Year.ToLower());
                if (constructionland == null)
                {
                    constructionland = new ConstructionLand();
                }
                return new double[]
                {
                    constructionland.SubTotal,
                    constructionland.County+constructionland.Town+constructionland.MiningLease,
                    constructionland.Town+constructionland.MiningLease,
                    constructionland.Town,
                    constructionland.MiningLease,
                    constructionland.County,
                    constructionland.Traffic,
                    constructionland.OtherConstruction,
                    constructionland.Other,
                    constructionland.Sum
                };
            }
        }

        public double[] GetNewConstruction(string Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                NewConstruction newconstruction = db.NewConstructions.FirstOrDefault(e => e.CID == ID && e.Year.ToLower() == Year.ToLower());
                if (newconstruction == null)
                {
                    newconstruction = new NewConstruction();
                }
                return new double[] { newconstruction.Construction, newconstruction.Town };
            }
        }
    }
}