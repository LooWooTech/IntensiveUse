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
            AgricultureLand agricultureland = SearchForAgriculture(Year, ID);
            return new double[] { agricultureland.Subtotal, agricultureland.Arable, agricultureland.Garden, agricultureland.Forest, agricultureland.Meadow, agricultureland.Other };
        }
        public double[] GetConstruction(string Year, int ID)
        {
            ConstructionLand constructionland = SearchForConstruction(Year, ID);
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

        public double[] GetNewConstruction(string Year, int ID)
        {
            NewConstruction newconstruction = SearchForNewConstruction(Year, ID);
            return new double[] { newconstruction.Construction, newconstruction.Town };
        }

        public double GetPGCI(int Year, int ID)
        {
            People people1 = SearchForPeople(Year.ToString(),ID);
            People people2 = SearchForPeople((Year - 1).ToString(), ID);
            NewConstruction newConstruction = SearchForNewConstruction(Year.ToString(), ID);
            if (Math.Abs(people1.PermanentSum - people2.PermanentSum) > 0.001)
            {
                return newConstruction.Town / (people1.PermanentSum - people2.PermanentSum);
            }
            return 0.00;
        }
    }
}