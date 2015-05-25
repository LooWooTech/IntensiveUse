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
            using (var db = GetIntensiveUseContext())
            {
                Economy economy = db.Economys.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == Year.ToLower());
                if (economy == null)
                {
                    economy = new Economy();
                }
                return new double[] { economy.Current, economy.Compare, economy.Aggregate };
            }
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
            using (var db = GetIntensiveUseContext())
            {
                string em = Year.ToString();
                Economy economy1 = db.Economys.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == em.ToLower());
                if (economy1 == null)
                {
                    economy1 = new Economy();
                }
                ConstructionLand construction1 = db.Constructions.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == em.ToLower());
                if (construction1 == null)
                {
                    construction1 = new ConstructionLand();
                }
                em = (Year - 3).ToString();
                Economy economy2 = db.Economys.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == em.ToLower());
                if (economy2 == null)
                {
                    economy2 = new Economy();
                }
                ConstructionLand construction2 = db.Constructions.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == em.ToLower());
                if (construction2 == null)
                {
                    construction2 = new ConstructionLand();
                }
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
        }
    }
}