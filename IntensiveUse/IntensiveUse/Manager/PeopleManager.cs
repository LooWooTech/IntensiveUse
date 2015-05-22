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


    }
}