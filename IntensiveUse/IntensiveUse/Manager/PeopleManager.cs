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
        public void Save(Dictionary<string, People> DICT, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                foreach (var item in DICT.Keys)
                {
                    People temp = db.Peoples.FirstOrDefault(e => e.Year.ToLower() == item.ToLower() && e.RID == ID);
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

        public double[] Get(string Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                People people = db.Peoples.FirstOrDefault(e=>e.RID==ID&&e.Year.ToLower()==Year.ToLower());
                if (people == null)
                {
                    people = new People();
                }
                return new double[] { people.PermanentSum, people.Town, people.County, people.HouseHold, people.Agriculture, people.NonFram };
            }
            
        }
    
        public List<string> Statistics(int ID)
        {
            int Year = DateTime.Now.Year;
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
            using (var db = GetIntensiveUseContext())
            {
                string em = Year.ToString();
                People people1 = db.Peoples.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == em.ToLower());
                if (people1 == null)
                {
                    people1 = new People();
                }
                ConstructionLand construction1 = db.Constructions.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == em.ToLower());
                if (construction1 == null)
                {
                    construction1 = new ConstructionLand();
                }
                em = (Year - 3).ToString();
                People People2 = db.Peoples.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == em.ToLower());
                if (People2 == null)
                {
                    People2 = new People();
                }
                ConstructionLand construction2 = db.Constructions.FirstOrDefault(e => e.RID == ID && e.Year.ToLower() == em.ToLower());
                if (construction2 == null)
                {
                    construction2 = new ConstructionLand();
                }
                Situation[] situation;
                switch (divison)
                {
                    case Division.Total:
                        situation=new Situation[2];
                        double coefficient = 0.0;
                        situation[0] = new Situation()
                        {
                            Increment = people1.PermanentSum - People2.PermanentSum
                        };
                        situation[1]=new Situation(){
                            Increment=construction1.Town + construction1.MiningLease+construction1.County - construction2.Town - construction2.MiningLease-construction2.County
                        };
                        if (Math.Abs(People2.PermanentSum - 0) > 0.001)
                        {
                            situation[0].Extent = situation[0].Increment / People2.PermanentSum * 100;
                        }
                        if (Math.Abs(construction2.Town + construction2.MiningLease+construction2.County - 0) > 0.001)
                        {
                            situation[1].Extent=situation[1].Increment/(construction2.Town + construction2.MiningLease+construction2.County) * 100;
                        }
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
                        situation=new Situation[2];
                        double coefficient1 = 0.0;
                        situation[0]=new Situation(){
                            Increment=people1.Town - People2.Town
                        };
                        situation[1]=new Situation(){
                            Increment=construction1.Town + construction1.MiningLease - construction2.Town - construction2.MiningLease
                        };
                        if (Math.Abs(People2.Town - 0) > 0.001)
                        {
                            situation[0].Extent=situation[0].Increment / People2.Town * 100;
                        }
                        if (Math.Abs(construction2.Town + construction2.MiningLease - 0) > 0.001)
                        {
                            situation[1].Extent=situation[1].Increment / (construction2.Town + construction2.MiningLease) * 100;
                        }
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
                        situation = new Situation[3];
                        double buffer1=0.0, buffer2=0.0;
                        situation[0]=new Situation(){
                            Increment=people1.County - People2.County
                        };
                        situation[1]=new Situation(){
                            Increment=people1.Agriculture - People2.Agriculture
                        };
                        situation[2] = new Situation()
                        {
                            Increment = construction1.County - construction2.County
                        };
                        if (Math.Abs(People2.County - 0) > 0.001)
                        {
                            situation[0].Extent = situation[0].Increment / People2.County*100;
                        }
                        if (Math.Abs(People2.Agriculture - 0) > 0.001)
                        {
                            situation[1].Extent = situation[1].Increment / People2.Agriculture * 100;
                        }
                        if (Math.Abs(construction2.County - 0) > 0.001)
                        {
                            situation[2].Extent = situation[2].Increment / construction2.County * 100;
                        }
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
        }


    }
}