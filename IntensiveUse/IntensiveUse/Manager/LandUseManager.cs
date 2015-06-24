using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class LandUseManager:ManagerBase
    {

        public List<double> Get(int Year, int ID)
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

        public double[] GetAgriculture(int Year, int ID)
        {
            AgricultureLand agricultureland = SearchForAgriculture(Year, ID);
            return new double[] { agricultureland.Subtotal, agricultureland.Arable, agricultureland.Garden, agricultureland.Forest, agricultureland.Meadow, agricultureland.Other };
        }
        public double[] GetConstruction(int Year, int ID)
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

        public double[] GetNewConstruction(int Year, int ID)
        {
            NewConstruction newconstruction = SearchForNewConstruction(Year, ID);
            return new double[] { newconstruction.Construction, newconstruction.Town };
        }

        public double GetPGCI(int Year, int ID)
        {
            People people1 = SearchForPeople(Year,ID);
            People people2 = SearchForPeople((Year - 1), ID);
            NewConstruction newConstruction = SearchForNewConstruction(Year, ID);
            if (Math.Abs(people1.PermanentSum - people2.PermanentSum) > 0.001)
            {
                return newConstruction.Town / (people1.PermanentSum - people2.PermanentSum);
            }
            return 0.00;
        }

        public double[] GetUII(int Year, int ID)
        {
            double[] values=Core.EconmoyManager.GetEUII(Year,ID);
            if (values.Count() != 2)
            {
                throw new ArgumentException("获取EUII数据的时候，发生数据错误");
            }
            return new double[]{
                Core.PeopleManager.GetPUII(Year,ID),
                values[0],
                values[1]
            };
        }

        public double[] GetGCI(int Year,int ID)
        {
            double[] values=Core.EconmoyManager.GetEGCI(Year,ID);
            if (values.Count() != 3)
            {
                throw new ArgumentException("在获取EGCI的时候，获取数据失败");
            }
            return new double[]{
                GetPGCI(Year,ID),
                values[0],
                values[1],
                values[2]
            };
        }

        public double[] GetEI(int Year, int ID)
        {
            return new double[]{
                Core.PeopleManager.GetPEI(Year,ID),
                Core.PeopleManager.GetEEI(Year,ID)
            };
        }

        public double[] GetULAPI(int Year, int ID)
        {
            return new double[]{
                Core.LandSupplyManager.ULAPI1(Year,ID),
                Core.LandSupplyManager.ULAPI2(Year,ID)
            };
        }


        public Queue<double> CreateExponentQueue(int Year, int ID)
        {
            Queue<double> Data = new Queue<double>();
            double[] UII = GetUII(Year, ID);
            foreach (var item in UII)
            {
                Data.Enqueue(item);
            }
            double[] GCI = GetGCI(Year, ID);
            foreach (var item in GCI)
            {
                Data.Enqueue(item);
            }
            double[] EI = GetEI(Year, ID);
            foreach (var item in EI)
            {
                Data.Enqueue(item);
            }
            double[] API = GetULAPI(Year, ID);
            foreach (var item in API)
            {
                Data.Enqueue(item);
            }
            return Data;
        }


        public void Delete(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                var entity = db.Ratifys.FirstOrDefault(e => e.Year == Year && e.RID == ID);
                if (entity != null)
                {
                    db.Ratifys.Remove(entity);
                    db.SaveChanges();
                }
            }
        }
    }
}