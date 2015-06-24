using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class LandSupplyManager:ManagerBase
    {
        public List<double> Get(int Year, int ID)
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
        public double[] GetSupply(int Year, int ID)
        {
            LandSupply landsupply = SearchForLandSupply(Year, ID);
            return new double[] { landsupply.Sum, landsupply.Append, landsupply.Stock };
        }

        public double[] GetRatify(int Year, int ID)
        {
            Ratify ratify = SearchForRatify(Year, ID);
            return new double[] { ratify.Area, ratify.Already };
        }

        public double ULAPI1(int Year, int ID)
        {
            double constructionSum = 0.0;
            double landsupplySum = 0.0;
            for (var i = 2; i >= 0; i--)
            {
                LandSupply landsupply=SearchForLandSupply((Year-i),ID);
                constructionSum += landsupply.Stock;
                landsupplySum += landsupply.Sum;
            }
            if (Math.Abs(landsupplySum - 0) > 0.001)
            {
                return constructionSum / landsupplySum * 100;
            }
                
            return 0.00;
        }

        public double ULAPI2(int Year, int ID)
        {
            double alreadySum = 0.0;
            double AreaSum = 0.0;
            for (var i = 3; i > 0; i--)
            {
                Ratify ratify = SearchForRatify((Year - i), ID);
                alreadySum += ratify.Already;
                AreaSum += ratify.Area;
            }
            if (Math.Abs(AreaSum - 0) > 0.001)
            {
                return alreadySum / AreaSum * 100;
            }
            return 0.00;
        }

        public void Delete(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                var entity = db.LandSupplys.FirstOrDefault(e => e.Year == Year && e.RID == ID);
                if (entity != null)
                {
                    db.LandSupplys.Remove(entity);
                    db.SaveChanges();
                }
            }
        }

    }
}