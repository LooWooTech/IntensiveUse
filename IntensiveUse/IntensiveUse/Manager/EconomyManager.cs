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

        public double[] GetEGCI(int Year, int ID)
        {
            double[] values = new double[3];
            Economy economy1 = SearchForEconomy((Year - 1).ToString(), ID);//13年
            Economy economy2 = SearchForEconomy(Year.ToString(), ID);//14年
            ConstructionLand construction1=SearchForConstruction((Year-1).ToString(),ID);
            ConstructionLand construction2=SearchForConstruction(Year.ToString(),ID);
            if (Math.Abs(economy1.Compare - 0) > 0.001 && Math.Abs(economy2.Compare - 0) > 0.001)
            {
                values[0] = (construction1.SubTotal / economy1.Compare - construction2.SubTotal / economy2.Compare) * economy1.Compare / construction1.SubTotal / 100;
            }
            NewConstruction newconstruction = SearchForNewConstruction(Year.ToString(), ID);
            if (Math.Abs(economy2.Compare - economy1.Compare) > 0.001)
            {
                values[1] = newconstruction.Construction * 10000 / (economy2.Compare - economy1.Compare);
            }
            if (Math.Abs(economy2.Aggregate - 0) > 0.001)
            {
                values[2] = newconstruction.Construction * 10000 / economy2.Aggregate;
            }
            return values;
        }


        public Situation[] EEI(int Year, int ID)
        {
            Situation[] situation=new Situation[2];
            Economy economy1 = SearchForEconomy(Year.ToString(), ID);
            Economy economy2 = SearchForEconomy((Year - 3).ToString(), ID);
            ConstructionLand construction1 = SearchForConstruction(Year.ToString(),ID);
            ConstructionLand construction2 = SearchForConstruction((Year - 3).ToString(), ID);
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
                situation[0].Extent = situation[0].Increment / economy2.Compare*100;
            }
            if (Math.Abs(construction2.SubTotal - 0) > 0.001)
            {
                situation[1].Extent = situation[0].Increment / construction2.SubTotal * 100;
            }
            return situation;
        }


        public EUII Acquire(int Year, int ID)
        {
            EUII entity = new EUII();
            entity.EUII1 = EUII1(Year, ID);
            entity.EUII2 = EUII2(Year, ID);
            Exponent exponent = SearchForExponent(Year.ToString(), ID, IdealType.Weight);
            entity.Economy = entity.EUII1.TargetStandard * exponent.EUII1 + entity.EUII2.TargetStandard * exponent.EUII2;
            return entity;
        }


        public IIBase EUII1(int Year, int ID)
        {
            IIBase baseValue = new IIBase();
            double sum = 0.0;
            for (var i = 2; i >= 0; i--)
            {
                Economy economy = SearchForEconomy((Year - i).ToString(), ID);
                sum += economy.Aggregate;
            }
            sum = sum / 3;
            ConstructionLand construction = SearchForConstruction(Year.ToString(),ID);
            if (Math.Abs(construction.SubTotal - 0) > 0.001)
            {
                baseValue.Status = sum * 100 / construction.SubTotal;
            }
            Exponent exponent = SearchForExponent(Year.ToString(),ID,IdealType.Value);
            if (Math.Abs(exponent.EUII1 - 0) > 0.001)
            {
                baseValue.StandardInit = baseValue.Status / exponent.EUII1;
            }
            baseValue.TargetStandard = baseValue.StandardInit;
            return baseValue;
        }

        public IIBase EUII2(int Year, int ID)
        {
            IIBase baseValue = new IIBase();
            Economy economy = SearchForEconomy(Year.ToString(), ID);
            ConstructionLand construction = SearchForConstruction(Year.ToString(),ID);
            if (Math.Abs(construction.SubTotal - 0) > 0.001)
            {
                baseValue.Status = economy.Current * 100 / construction.SubTotal;
            }
            Exponent exponent = SearchForExponent(Year.ToString(), ID, IdealType.Value);
            if (Math.Abs(exponent.EUII2 - 0) > 0.001)
            {
                baseValue.StandardInit = baseValue.Status / exponent.EUII2;
            }
            baseValue.TargetStandard = baseValue.StandardInit;
            return baseValue;
        }
    }
}