using IntensiveUse.Helper;
using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class EconomyManager:ManagerBase
    {
        public double[] Get(string Year, int ID)
        {
            Economy economy = SearchForEconomy(Year, ID);
            return new double[] { economy.Current, economy.Compare, economy.Aggregate };
        }

        public List<double> Gain(int Year,int ID,Situation[] Citys)
        {
            if (Citys.Count() != 2)
            {
                throw new ArgumentException("未读取到上一级行政区相关数据");
            }
            Situation[] values = Find(Year,ID);
            if (values == null||values.Count()!=2)
            {
                throw new ArgumentException("读取数据失败");
            }
            double m = 0.0,n=0.0;
            if (Math.Abs(values[1].Extent - 0) > 0.001)
            {
                m = values[0].Extent / values[1].Extent;
            }
            if (Math.Abs(Citys[0].Increment - 0) > 0.001 && Math.Abs(Citys[1].Increment - 0) > 0.001&&Math.Abs(values[0].Increment-0)>0.001&&Math.Abs(values[1].Increment-0)>0.001)
            {
                n = values[0].Increment * Citys[1].Increment / Citys[0].Increment / values[1].Increment;
            }
            return new List<double>(){
                values[0].Increment,
                values[0].Extent,
                values[1].Increment,
                values[1].Extent,
                m,
                n
            };
        }

        public Situation[] Find(int Year, int ID)
        {
            string em = Year.ToString();
            Economy economy1 = SearchForEconomy(em, ID);
            ConstructionLand construction1 = SearchForConstruction(em, ID);
            em = (Year - 3).ToString();
            Economy economy2 = SearchForEconomy(em, ID);
            ConstructionLand construction2 = SearchForConstruction(em, ID);
            Situation[] situation = new Situation[2];
            situation[0] = new Situation()
            {
                Increment = economy1.Compare - economy2.Compare
            };
            situation[1] = new Situation()
            {
                Increment = construction1.SubTotal - construction2.SubTotal
            };
            if (Math.Abs(economy2.Compare - 0) > 0.001)
            {
                situation[0].Extent = situation[0].Increment / economy2.Compare * 100;
            }
            if (Math.Abs(construction2.SubTotal - 0) > 0.001)
            {
                situation[1].Extent = situation[1].Increment / construction2.SubTotal * 100;
            }
            return situation;
        }

        public double[] GetEUII(int Year, int ID)
        {
            double sum = 0.0;
            string em=string.Empty;
            Economy economy = null;
            for (var i = 2; i >= 0; i--)
            {
                em = (Year - i).ToString();
                economy = SearchForEconomy(em, ID);
                sum += economy.Aggregate;
            }
            sum = sum / 3;
            em=Year.ToString();
            ConstructionLand construction = SearchForConstruction(em,ID);
            economy = SearchForEconomy(em, ID);
            double[] values = new double[2];
            if (Math.Abs(construction.SubTotal - 0) > 0.001)
            {
                values[0] = sum * 100 / construction.SubTotal;
                values[1] = economy.Current * 100 / construction.SubTotal;
            }
            return values;
        }
    }
}