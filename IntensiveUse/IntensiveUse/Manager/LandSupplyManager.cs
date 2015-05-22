using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class LandSupplyManager:ManagerBase
    {
        public List<double> Get(string Year, int ID)
        {
            double[] data1 = GetSupply(Year,ID);
            double[] data2 = GetRatify(Year,ID);
            List<double> Data = new List<double>();
            foreach (var item in data1)
            {
                Data.Add(item);
            }
            foreach (var item in data2)
            {
                Data.Add(item);
            }
            return Data;
        }
        public double[] GetSupply(string Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                LandSupply landsupply = db.LandSupplys.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == Year.ToLower());
                if (landsupply == null)
                {
                    landsupply = new LandSupply();
                }
                return new double[] { landsupply.Sum, landsupply.Append, landsupply.Stock };
            }
        }

        public double[] GetRatify(string Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                Ratify ratify = db.Ratifys.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == Year.ToLower());
                if (ratify == null)
                {
                    ratify = new Ratify();
                }
                return new double[] { ratify.Area, ratify.Already };
            }
        }
    }
}