using IntensiveUse.Helper;
using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class PeopleManager:ManagerBase
    {
        public void Save(Dictionary<int, People> DICT, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                foreach (var item in DICT.Keys)
                {
                    People temp = db.Peoples.FirstOrDefault(e => e.Year == item && e.RID == ID);
                    if (temp == null)
                    {
                        temp = DICT[item];
                        db.Peoples.Add(temp);
                    }
                    else
                    {
                        DICT[item].ID = temp.ID;
                        db.Entry(temp).CurrentValues.SetValues(DICT[item]);
                    }
                    db.SaveChanges();
                }
            }
        }


        public double[] Get(int Year, int ID)
        {
            People people = SearchForPeople(Year, ID);
            return new double[] { people.PermanentSum, people.Town, people.County, people.HouseHold, people.Agriculture, people.NonFram };   
        }
    
        public List<string> Statistics(int ID,int Year)
        {
            List<string> list = new List<string>();
            foreach (Division item in Enum.GetValues(typeof(Division)))
            {
                List<string> temp = Gain(Year, ID, item);
                if (temp == null)
                {
                    throw new ArgumentException("未获取"+item.ToString()+"的数据");
                }
                foreach (var entity in temp)
                {
                    list.Add(entity);
                }
            }
            return list;
          
        }

        public List<string> Gain(int Year,int ID,Division divison)
        {
            Situation[] situation;
            switch (divison)
            {
                case Division.Total:
                    situation = PEI1(Year,ID);
                    double coefficient = 0.0;
                    if (Math.Abs(situation[1].Extent - 0) > 0.001)
                    {
                        coefficient = situation[0].Extent / situation[1].Extent;
                    }
                    return new List<string>(){
                            situation[0].Increment.ToString("0.00"),
                            situation[0].Extent.ToString("0.00")+"%",
                            situation[1].Increment.ToString("0.00"),
                            situation[1].Extent.ToString("0.00")+"%",
                            coefficient.ToString("0.00"),
                            ""
                        };
                case Division.Town:
                    situation = PEI2(Year,ID);
                    double coefficient1 = 0.0;
                    if (Math.Abs(situation[1].Extent - 0) > 0.001)
                    {
                        coefficient1 = situation[0].Extent / situation[1].Extent;
                    }
                    return new List<string>(){
                            situation[0].Increment.ToString("0.00"),
                            situation[0].Extent.ToString("0.00")+"%",
                            situation[1].Increment.ToString("0.00"),
                            situation[1].Extent.ToString("0.00")+"%",
                            coefficient1.ToString("0.00")
                        };
                case Division.County:
                    situation = PEI3(Year,ID);
                    double buffer1 = 0.0, buffer2 = 0.0;
                    if (Math.Abs(situation[2].Extent - 0) > 0.001)
                    {
                        buffer1 = situation[0].Extent / situation[2].Extent;
                        buffer2 = situation[1].Extent / situation[2].Extent;
                    }
                    return new List<string>(){
                            situation[0].Increment.ToString("0.00"),
                            situation[0].Extent.ToString("0.00")+"%",
                            situation[1].Increment.ToString("0.00"),
                            situation[1].Extent.ToString("0.00")+"%",
                            situation[2].Increment.ToString("0.00"),
                            situation[2].Extent.ToString("0.00")+"%",
                            buffer1.ToString("0.00"),
                            buffer2.ToString("0.00")
                        };
                default: break;
            }
            return null;     
        }

        public Situation[] PEI1(int Year,int ID)
        {
            Situation[] situation=new Situation[2];
            int em = Year;
            People people1 = SearchForPeople(em, ID);
            ConstructionLand construction1 = SearchForConstruction(em, ID);
            em = (Year - 3);
            People People2 = SearchForPeople(em, ID);
            ConstructionLand construction2 = SearchForConstruction(em, ID);
            
            situation[0] = new Situation()
            {
                Increment = people1.PermanentSum - People2.PermanentSum
            };
            situation[1] = new Situation()
            {
                Increment = construction1.Town + construction1.MiningLease + construction1.County - construction2.Town - construction2.MiningLease - construction2.County
            };
            if (Math.Abs(People2.PermanentSum - 0) > 0.001)
            {
                situation[0].Extent = situation[0].Increment / People2.PermanentSum * 100;
            }
            if (Math.Abs(construction2.Town + construction2.MiningLease + construction2.County - 0) > 0.001)
            {
                situation[1].Extent = situation[1].Increment / (construction2.Town + construction2.MiningLease + construction2.County) * 100;
            }
            return situation;
        }

        public Situation[] PEI2(int Year, int ID)
        {
            Situation[] situation=new Situation[2];
            int em = Year;
            People people1 = SearchForPeople(em, ID);
            ConstructionLand construction1 = SearchForConstruction(em, ID);
            em = Year - 3;
            People People2 = SearchForPeople(em, ID);
            ConstructionLand construction2 = SearchForConstruction(em, ID);
            situation[0] = new Situation()
            {
                Increment = people1.Town - People2.Town
            };
            situation[1] = new Situation()
            {
                Increment = construction1.Town + construction1.MiningLease - construction2.Town - construction2.MiningLease
            };
            if (Math.Abs(People2.Town - 0) > 0.001)
            {
                situation[0].Extent = situation[0].Increment / People2.Town * 100;
            }
            if (Math.Abs(construction2.Town + construction2.MiningLease - 0) > 0.001)
            {
                situation[1].Extent = situation[1].Increment / (construction2.Town + construction2.MiningLease) * 100;
            }
            return situation;
        }

        public Situation[] PEI3(int Year, int ID)
        {
            Situation[] situation=new Situation[3];
            int em = Year;
            People people1 = SearchForPeople(em, ID);
            ConstructionLand construction1 = SearchForConstruction(em, ID);
            em = Year - 3;
            People People2 = SearchForPeople(em, ID);
            ConstructionLand construction2 = SearchForConstruction(em, ID);
            situation[0] = new Situation()
            {
                Increment = people1.County - People2.County
            };
            situation[1] = new Situation()
            {
                Increment = people1.Agriculture - People2.Agriculture
            };
            situation[2] = new Situation()
            {
                Increment = construction1.County - construction2.County
            };
            if (Math.Abs(People2.County - 0) > 0.001)
            {
                situation[0].Extent = situation[0].Increment / People2.County * 100;
            }
            if (Math.Abs(People2.Agriculture - 0) > 0.001)
            {
                situation[1].Extent = situation[1].Increment / People2.Agriculture * 100;
            }
            if (Math.Abs(construction2.County - 0) > 0.001)
            {
                situation[2].Extent = situation[2].Increment / construction2.County * 100;
            }
            return situation;
        }

        public double GetPUII(int Year, int ID)
        {
            People people = SearchForPeople(Year,ID);
            ConstructionLand construction = SearchForConstruction(Year, ID);
            if (Math.Abs(construction.Town + construction.MiningLease + construction.County - 0) > 0.001)
            {
                return people.PermanentSum * 10000 / (construction.Town + construction.MiningLease + construction.County) * 100;
            }
            return 0.00;
        }


        public double GetPEI(int Year, int ID)
        {
            Situation[] situation = PEI1(Year,ID);
            if (situation.Count() != 2)
            {
                throw new ArgumentException("在获取PEI1的时候，获取失败");
            }
            if (Math.Abs(situation[1].Extent - 0) > 0.001)
            {
                return situation[0].Extent / situation[1].Extent;
            }
            return 0.00;
        }

        public double GetEEI(int Year, int ID)
        {
            Situation[] values = Core.EconmoyManager.EEI(Year,ID);
            if (values.Count() != 2)
            {
                throw new ArgumentException("获取EEI数据失败");
            }
            if (Math.Abs(values[1].Extent - 0) > 0.001)
            {
                return values[0].Extent / values[1].Extent;
            }
            return 0.00;
        }

        public void  Delete(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                var entity = db.Peoples.FirstOrDefault(e => e.Year == Year && e.RID == ID);
                if (entity != null)
                {
                    db.Peoples.Remove(entity);
                    db.SaveChanges();
                }
                
            }
        }



     



    }
}