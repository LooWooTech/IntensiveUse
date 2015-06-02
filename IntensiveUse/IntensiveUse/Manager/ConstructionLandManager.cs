using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class ConstructionLandManager:ManagerBase
    {
        public Queue<double> TranslateOfPEUII(PEUII PE)
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
        public Queue<double> TranslateOfPEGCI(PEGCI PEG)
        {
            Queue<double> queue = new Queue<double>();
            Gain(PEG.PGCI.PGCI1, ref queue);
            queue.Enqueue(PEG.PGCI.PopulationGrowth);
            Gain(PEG.EGCI.EGCI1, ref queue);
            Gain(PEG.EGCI.EGCI2, ref queue);
            Gain(PEG.EGCI.EGCI3, ref queue);
            queue.Enqueue(PEG.EGCI.Economy);
            queue.Enqueue(PEG.GCI);
            return queue;
        }

        public Queue<double> TranslateOfPEEI(PEEI PEEI)
        {
            Queue<double> queue = new Queue<double>();
            Gain(PEEI.PEI.PEI1, ref queue);
            queue.Enqueue(PEEI.PEI.PopulationSite);
            Gain(PEEI.EEI.EEI1, ref queue);
            queue.Enqueue(PEEI.EEI.Economy);
            queue.Enqueue(PEEI.EI);
            return queue;
        }

        public Queue<double> TranslateOfAPIUL(APIUL APIUL)
        {
            Queue<double> queue = new Queue<double>();
            Gain(APIUL.ULAPI.ULAPI1, ref queue);
            Gain(APIUL.ULAPI.ULAPI2, ref queue);
            queue.Enqueue(APIUL.ULAPI.Performance);
            queue.Enqueue(APIUL.API);
            return queue;
        }
        public Queue<double> TranslateOfUGEAA(UGEAA UGEAA)
        {
            Queue<double> queue = new Queue<double>();
            Gain(UGEAA, ref queue);
            return queue;
        }

        

        public PEUII AcquireOfPEUII(int Year, int ID,int CID)
        {
            PEUII value = new PEUII();
            value.PUII = AcquireOfPUII(Year,ID,CID);
            value.EUII = AcquireOfEUII(Year, ID,CID);
            SubIndex subindex = SearchForSubIndex(Year, CID);
            value.UII = value.PUII.PopulationDensity * subindex.PUII + value.EUII.Economy * subindex.EUII;
            return value;
        }
        public PUII AcquireOfPUII(int Year, int ID,int CID)
        {
            PUII entity = new PUII();
            IIBase baseValue = PUII1(Year, ID,CID);
            entity.II = baseValue;
            Exponent exponent = SearchForExponent(Year, CID, IdealType.Weight);
            entity.PopulationDensity = baseValue.TargetStandard * exponent.PGCI;
            return entity;

        }

        public IIBase PUII1(int Year, int ID,int CID)
        {
            People People = SearchForPeople(Year, ID);
            ConstructionLand construction = SearchForConstruction(Year, ID);
            Exponent IExponent = SearchForExponent(Year, CID, IdealType.Value);
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
            Exponent exponent = SearchForExponent(Year, CID, IdealType.Weight);
            entity.Economy = entity.EUII1.TargetStandard * exponent.EUII1 + entity.EUII2.TargetStandard * exponent.EUII2;
            return entity;
        }


        public IIBase EUII1(int Year, int ID,int CID)
        {
            IIBase baseValue = new IIBase();
            double sum = 0.0;
            for (var i = 2; i >= 0; i--)
            {
                Economy economy = SearchForEconomy((Year - i), ID);
                sum += economy.Aggregate;
            }
            sum = sum / 3;
            ConstructionLand construction = SearchForConstruction(Year, ID);
            if (Math.Abs(construction.SubTotal - 0) > 0.001)
            {
                baseValue.Status = sum * 100 / construction.SubTotal;
            }
            Exponent exponent = SearchForExponent(Year, CID, IdealType.Value);
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
            Economy economy = SearchForEconomy(Year, ID);
            ConstructionLand construction = SearchForConstruction(Year, ID);
            if (Math.Abs(construction.SubTotal - 0) > 0.001)
            {
                baseValue.Status = economy.Current * 100 / construction.SubTotal;
            }
            Exponent exponent = SearchForExponent(Year, CID, IdealType.Value);
            if (Math.Abs(exponent.EUII2 - 0) > 0.001)
            {
                baseValue.StandardInit = baseValue.Status / exponent.EUII2;
            }
            baseValue.TargetStandard = baseValue.StandardInit;
            return baseValue;
        }


        public PEGCI AcquireOfPEGCI(int Year, int ID, int CID)
        {
            PEGCI entity = new PEGCI();
            entity.PGCI = AcquireOfPGCI(Year, ID, CID);
            entity.EGCI = AcquireOfEGCI(Year, ID, CID);
            SubIndex subindex = SearchForSubIndex(Year, CID);
            entity.GCI = entity.PGCI.PopulationGrowth * subindex.PGCI + entity.EGCI.Economy * subindex.EGCI;
            return entity;
        }

        public PGCI AcquireOfPGCI(int Year, int ID,int CID)
        {
            PGCI entity = new PGCI();
            entity.PGCI1 = PGCI1(Year, ID, CID);
            Exponent exponent = SearchForExponent(Year, CID, IdealType.Weight);
            if (Math.Abs(exponent.PGCI - 0) > 0.001)
            {
                entity.PopulationGrowth = entity.PGCI1.TargetStandard / exponent.PGCI;
            }
            return entity;
        }

        public IIBase PGCI1(int Year, int ID,int CID)
        {
            IIBase BaseValue = new IIBase();
            NewConstruction newconstruction = SearchForNewConstruction(Year,ID);
            People people1 = SearchForPeople(Year,ID);
            People people2 = SearchForPeople((Year-1),ID);
            if (Math.Abs(people1.PermanentSum - people2.PermanentSum) > 0.001)
            {
                BaseValue.Status = newconstruction.Town / (people1.PermanentSum - people2.PermanentSum);
            }
            Exponent exponent = SearchForExponent(Year, CID,IdealType.Value);
            if (Math.Abs(exponent.PGCI - 0) > 0.001)
            {
                BaseValue.StandardInit = BaseValue.Status / exponent.PGCI;
            }
            if (Math.Abs(BaseValue.StandardInit - 0) > 0.001)
            {
                BaseValue.TargetStandard = 1 / BaseValue.StandardInit;
            }
            return BaseValue;
        }

        public EGCI AcquireOfEGCI(int Year, int ID, int CID)
        {
            EGCI entity = new EGCI();
            entity.EGCI1 = EGCI1(Year, ID, CID);
            entity.EGCI2 = EGCI2(Year, ID, CID);
            entity.EGCI3 = EGCI3(Year, ID, CID);
            Exponent exponent = SearchForExponent(Year, CID, IdealType.Weight);
            entity.Economy = entity.EGCI1.TargetStandard * exponent.EGCI1 + entity.EGCI2.TargetStandard * exponent.EGCI2 + entity.EGCI3.TargetStandard * exponent.EGCI3;
            return entity;
        }

        public IIBase EGCI1(int Year, int ID, int CID)
        {
            IIBase basevalue = new IIBase();
            Economy economy1 = SearchForEconomy(Year,ID);//14
            Economy economy2 = SearchForEconomy((Year-1),ID);//13
            ConstructionLand construction1 = SearchForConstruction(Year,ID);//14
            ConstructionLand construction2 = SearchForConstruction((Year - 1), ID);//13
            if (Math.Abs(construction2.SubTotal - 0) > 0.001 && Math.Abs(economy1.Compare - 0) > 0.001)
            {
                basevalue.Status = (construction2.SubTotal * economy1.Compare - economy2.Compare * construction1.SubTotal) / construction2.SubTotal / economy1.Compare * 100;
                //basevalue.Status = (construction2.SubTotal / economy2.Compare - construction1.SubTotal / economy1.Compare) / (construction2.SubTotal / economy1.Compare) * 100;
            }
            Exponent exponent = SearchForExponent(Year, CID, IdealType.Value);
            if (Math.Abs(exponent.EGCI1 - 0) > 0.001)
            {
                basevalue.StandardInit = basevalue.Status / exponent.EGCI1;
            }
            basevalue.TargetStandard = basevalue.StandardInit;
            return basevalue;
        }

        public IIBase EGCI2(int Year, int ID, int CID)
        {
            IIBase basevalue = new IIBase();
            NewConstruction newconstruction = SearchForNewConstruction(Year, ID);
            Economy economy = SearchForEconomy(Year, ID);
            if (Math.Abs(economy.Compare - 0) > 0.001)
            {
                basevalue.Status = newconstruction.Construction * 10000 / economy.Compare;
            }
            Exponent exponent = SearchForExponent(Year, ID, IdealType.Value);
            if (Math.Abs(exponent.EGCI2 - 0) > 0.001)
            {
                basevalue.StandardInit = basevalue.Status / exponent.EGCI2;
            }
            if (Math.Abs(basevalue.StandardInit - 0) > 0.001)
            {
                basevalue.TargetStandard = 1 / basevalue.StandardInit;
            }
            return basevalue;
        }

        public IIBase EGCI3(int Year, int ID, int CID)
        {
            IIBase basevalue = new IIBase();
            NewConstruction newconstruction = SearchForNewConstruction(Year, ID);
            Economy economy = SearchForEconomy(Year, ID);
            if (Math.Abs(economy.Aggregate - 0) > 0.001)
            {
                basevalue.Status = newconstruction.Construction * 10000 / economy.Aggregate;
            }
            Exponent exponet = SearchForExponent(Year, CID, IdealType.Value);
            if (Math.Abs(exponet.EGCI3 - 0) > 0.001)
            {
                basevalue.StandardInit = basevalue.Status / exponet.EGCI3;
            }
            if (Math.Abs(basevalue.StandardInit - 0) > 0.001)
            {
                basevalue.TargetStandard = 1 / basevalue.StandardInit;
            }
            return basevalue;
        }

        public PEEI AcquireOfPEEI(int Year, int ID, int CID)
        {
            PEEI entity = new PEEI();
            entity.PEI = AcquireOfPEI(Year, ID, CID);
            entity.EEI = AcquireOfEEI(Year, ID, CID);
            SubIndex subindex = SearchForSubIndex(Year, CID);
            entity.EI = entity.PEI.PopulationSite * subindex.PEI + entity.EEI.Economy * subindex.EEI;
            return entity;
        }

        public PEI AcquireOfPEI(int Year, int ID, int CID)
        {
            PEI entity = new PEI();
            entity.PEI1 = PEII1(Year, ID, CID);
            Exponent exponent = SearchForExponent(Year, CID, IdealType.Weight);
            entity.PopulationSite = entity.PEI1.TargetStandard * exponent.PEI1;
            return entity;
        }

        public IIBase PEII1(int Year, int ID, int CID)
        {
            IIBase basevalue = new IIBase();
            People people1 = SearchForPeople(Year, ID);//14
            People people2 = SearchForPeople((Year-3),ID);//11
            ConstructionLand construction1 = SearchForConstruction(Year,ID);//14
            ConstructionLand construction2 = SearchForConstruction((Year-3),ID);//11
            if (Math.Abs(people2.PermanentSum - 0) > 0.001&&Math.Abs(construction1.Town+construction1.MiningLease-construction1.County-construction2.Town-construction2.MiningLease-construction2.County)>0.001)
            {
                basevalue.Status = (people1.PermanentSum - people2.PermanentSum) * (construction2.Town + construction2.MiningLease + construction2.County) / people2.PermanentSum / (construction1.Town + construction1.MiningLease + construction1.County - construction2.Town - construction2.MiningLease - construction2.County);
            }
            Exponent exponent = SearchForExponent(Year, CID,IdealType.Value);
            if (Math.Abs(exponent.PEI1 - 0) > 0.001)
            {
                basevalue.StandardInit = basevalue.Status / exponent.PEI1;
            }
            basevalue.TargetStandard = basevalue.StandardInit;
            return basevalue;
        }

        public EEI AcquireOfEEI(int Year, int ID, int CID)
        {
            EEI entity = new EEI();
            entity.EEI1 = EEI1(Year,ID,CID);
            Exponent exponent = SearchForExponent(Year, CID, IdealType.Weight);
            entity.Economy = entity.EEI1.TargetStandard * exponent.EEI;
            return entity;
        }

        public IIBase EEI1(int Year, int ID, int CID)
        {
            IIBase basevalue = new IIBase();
            Economy economy1 = SearchForEconomy(Year, ID);
            Economy economy2 = SearchForEconomy((Year - 3), ID);
            ConstructionLand construction1 = SearchForConstruction(Year,ID);
            ConstructionLand construction2 = SearchForConstruction((Year - 3), ID);
            if (Math.Abs(economy2.Compare - 0) > 0.001&&Math.Abs(construction1.SubTotal-construction2.SubTotal)>0.001)
            {
                basevalue.Status = (economy1.Compare - economy2.Compare) * construction2.SubTotal / economy2.Compare / (construction1.SubTotal - construction2.SubTotal);
            }
            Exponent exponent = SearchForExponent(Year, CID, IdealType.Value);
            if (Math.Abs(exponent.EEI - 0) > 0.001)
            {
                basevalue.StandardInit = basevalue.Status / exponent.EEI;
            }
            basevalue.TargetStandard = basevalue.StandardInit;
            return basevalue;
        }

        public APIUL AcquireOfAPIUL(int Year, int ID, int CID)
        {
            APIUL entity = new APIUL();
            entity.ULAPI = AcquireOfULAPI(Year, ID, CID);
            SubIndex subindex = SearchForSubIndex(Year, CID);
            entity.API = entity.ULAPI.Performance * subindex.ULAPI;
            return entity;
        }

        public ULAPI AcquireOfULAPI(int Year, int ID, int CID)
        {
            ULAPI entity = new ULAPI();
            entity.ULAPI1 = ULAPI1(Year,ID,CID);
            entity.ULAPI2 = ULAPI2(Year, ID, CID);
            Exponent exponent = SearchForExponent(Year, CID, IdealType.Weight);
            entity.Performance = entity.ULAPI1.TargetStandard * exponent.ULAPI1 + entity.ULAPI2.TargetStandard * exponent.ULAPI2;
            return entity;
        }

        public IIBase ULAPI1(int Year, int ID, int CID)
        {
            IIBase baseValue = new IIBase();
            double SumStock = 0.0;
            double Sum = 0.0;
            for (var i = 2; i >= 0; i--)
            {
                LandSupply landsupply = SearchForLandSupply((Year - i), ID);
                Sum += landsupply.Sum;
                SumStock += landsupply.Stock;
            }
            if (Math.Abs(Sum - 0) > 0.001)
            {
                baseValue.Status = SumStock / Sum * 100;
            }
            Exponent exponent = SearchForExponent(Year, CID, IdealType.Value);
            if (Math.Abs(exponent.ULAPI1 - 0) > 0.001)
            {
                baseValue.StandardInit = baseValue.Status / exponent.ULAPI1;
            }
            baseValue.TargetStandard = baseValue.StandardInit;
            return baseValue;
        }

        public IIBase ULAPI2(int Year, int ID, int CID)
        {
            IIBase basevalue = new IIBase();
            double SumArea = 0.0;
            double SumAlready = 0.0;
            for (var i = 3; i > 0; i--)
            {
                Ratify ratify = SearchForRatify((Year - i), ID);
                SumArea += ratify.Area;
                SumAlready += ratify.Already;
            }
            basevalue.Status = SumAlready / SumArea * 100;
            Exponent exponent = SearchForExponent(Year, CID, IdealType.Value);
            if (Math.Abs(exponent.ULAPI2 - 0) > 0.001)
            {
                basevalue.StandardInit = basevalue.Status / exponent.ULAPI2;
            }
            basevalue.TargetStandard = basevalue.StandardInit;
            return basevalue;
        }

        public UGEAA AcquireOfUGEAA(int Year, int ID, int CID)
        {
            UGEAA entity = new UGEAA();
            PEUII PEUII = AcquireOfPEUII(Year, ID, CID);
            PEGCI PEGCI = AcquireOfPEGCI(Year, ID, CID);
            PEEI PEEI = AcquireOfPEEI(Year, ID, CID);
            APIUL APIUL = AcquireOfAPIUL(Year, ID, CID);
            entity.UII = PEUII.UII;
            entity.GCI = PEGCI.GCI;
            entity.EI = PEEI.EI;
            entity.API = APIUL.API;
            IndexWeight indexWeight = SearchForIndexWeight(Year, CID);
            entity.CombinedIndex = entity.UII * indexWeight.UII + entity.GCI * indexWeight.GCI + entity.EI * indexWeight.EI + entity.API * indexWeight.API;
            return entity;
        }
    }
}