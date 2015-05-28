using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class ConstructionLandManager:ManagerBase
    {
        public Queue<double> Translate(PEUII PE)
        {
            Queue<double> queue = new Queue<double>();
            Gain(PE.PUII.II, ref queue);
            queue.Enqueue(PE.PUII.PopulationDensity);
            Gain(PE.EUII.EUII1, ref queue);
            Gain(PE.EUII.EUII2, ref queue);
            queue.Enqueue(PE.EUII.Economy);
            queue.Enqueue(PE.UII);
            return queue;
        }


        public void Gain(IIBase Complex,ref Queue<double> queue)
        {
            System.Reflection.PropertyInfo[] propList = typeof(IIBase).GetProperties();
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

        public PEUII AcquireOfPEUII(int Year, int ID,int CID)
        {
            PEUII value = new PEUII();
            value.PUII = AcquireOfPUII(Year,ID,CID);
            value.EUII = AcquireOfEUII(Year, ID,CID);
            SubIndex subindex = SearchForSubIndex(Year.ToString(), CID);
            value.UII = value.PUII.PopulationDensity * subindex.PUII + value.EUII.Economy * subindex.EUII;
            return value;
        }
        public PUII AcquireOfPUII(int Year, int ID,int CID)
        {
            PUII entity = new PUII();
            IIBase baseValue = PUII1(Year, ID,CID);
            entity.II = baseValue;
            Exponent exponent = SearchForExponent(Year.ToString(), CID, IdealType.Weight);
            entity.PopulationDensity = baseValue.TargetStandard * exponent.PGCI;
            return entity;

        }

        public IIBase PUII1(int Year, int ID,int CID)
        {
            People People = SearchForPeople(Year.ToString(), ID);
            ConstructionLand construction = SearchForConstruction(Year.ToString(), ID);
            Exponent IExponent = SearchForExponent(Year.ToString(), CID, IdealType.Value);
            IIBase baseValue = new IIBase();
            if (Math.Abs(construction.Town + construction.MiningLease + construction.County) > 0.001)
            {
                baseValue.Status = People.PermanentSum * 100 * 100 / (construction.Town + construction.MiningLease + construction.County);
            }
            if (Math.Abs(IExponent.PGCI - 0) > 0.001)
            {
                baseValue.StandardInit = baseValue.Status / IExponent.PGCI;
            }
            if (Math.Abs(baseValue.StandardInit - 0) > 0.001)
            {
                baseValue.TargetStandard = 1 / baseValue.StandardInit;
            }
            return baseValue;
        }

        public EUII AcquireOfEUII(int Year, int ID,int CID)
        {
            EUII entity = new EUII();
            entity.EUII1 = EUII1(Year, ID,CID);
            entity.EUII2 = EUII2(Year, ID,CID);
            Exponent exponent = SearchForExponent(Year.ToString(), CID, IdealType.Weight);
            entity.Economy = entity.EUII1.TargetStandard * exponent.EUII1 + entity.EUII2.TargetStandard * exponent.EUII2;
            return entity;
        }


        public IIBase EUII1(int Year, int ID,int CID)
        {
            IIBase baseValue = new IIBase();
            double sum = 0.0;
            for (var i = 2; i >= 0; i--)
            {
                Economy economy = SearchForEconomy((Year - i).ToString(), ID);
                sum += economy.Aggregate;
            }
            sum = sum / 3;
            ConstructionLand construction = SearchForConstruction(Year.ToString(), ID);
            if (Math.Abs(construction.SubTotal - 0) > 0.001)
            {
                baseValue.Status = sum * 100 / construction.SubTotal;
            }
            Exponent exponent = SearchForExponent(Year.ToString(), CID, IdealType.Value);
            if (Math.Abs(exponent.EUII1 - 0) > 0.001)
            {
                baseValue.StandardInit = baseValue.Status / exponent.EUII1;
            }
            baseValue.TargetStandard = baseValue.StandardInit;
            return baseValue;
        }

        public IIBase EUII2(int Year, int ID,int CID)
        {
            IIBase baseValue = new IIBase();
            Economy economy = SearchForEconomy(Year.ToString(), ID);
            ConstructionLand construction = SearchForConstruction(Year.ToString(), ID);
            if (Math.Abs(construction.SubTotal - 0) > 0.001)
            {
                baseValue.Status = economy.Current * 100 / construction.SubTotal;
            }
            Exponent exponent = SearchForExponent(Year.ToString(), CID, IdealType.Value);
            if (Math.Abs(exponent.EUII2 - 0) > 0.001)
            {
                baseValue.StandardInit = baseValue.Status / exponent.EUII2;
            }
            baseValue.TargetStandard = baseValue.StandardInit;
            return baseValue;
        }

        public PGCI AcquireOfPGCI(int Year, int ID)
        {
            PGCI entity = new PGCI();

            return null;
        }

        public IIBase PGCI1(int Year, int ID)
        {
            IIBase BaseValue = new IIBase();
            NewConstruction newconstruction = SearchForNewConstruction(Year.ToString(),ID);
            People people1 = SearchForPeople(Year.ToString(),ID);
            People people2 = SearchForPeople((Year-1).ToString(),ID);
            if (Math.Abs(people1.PermanentSum - people2.PermanentSum) > 0.001)
            {
                BaseValue.Status = newconstruction.Town / (people1.PermanentSum - people2.PermanentSum);
            }
            Exponent exponent = SearchForExponent(Year.ToString(), ID,IdealType.Value);
            return BaseValue;
        }
    }
}