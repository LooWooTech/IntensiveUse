using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class StatisticsManager:ManagerBase
    {

        public Dictionary<int, Queue<bool>> Gain(string City)
        {
            List<Statistic> List = Statistic(City);
            return TranlateOFStatistic(List);
        }
        public List<Statistic> Statistic(string City)
        {
            using (var db = GetIntensiveUseContext())
            {
                Region region = db.Regions.FirstOrDefault(e => e.Name.ToLower()==City.ToLower());
                if (region == null)
                {
                    return new List<Statistic>();
                }
                return db.Statistics.Where(e => e.RID == region.ID).OrderBy(e => e.Year).ToList();
            }
        }

        public Dictionary<int, Queue<bool>> TranlateOFStatistic(List<Statistic> List)
        {
            Dictionary<int, Queue<bool>> Value = new Dictionary<int, Queue<bool>>();
            foreach (var item in List)
            {
                Queue<bool> m = new Queue<bool>();
                System.Reflection.PropertyInfo[] propList = typeof(Statistic).GetProperties();
                foreach (var entity in propList)
                {
                    if (entity.PropertyType.Equals(typeof(bool)))
                    {
                        bool val = false;
                        bool.TryParse(entity.GetValue(item, null).ToString(), out val);
                        m.Enqueue(val);
                    }
                }
                if (!Value.ContainsKey(item.Year))
                {
                    Value.Add(item.Year, m);
                }
            }
            return Value;
        }

        public void Delete(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                var entity = db.Statistics.FirstOrDefault(e => e.Year == Year && e.RID == ID);
                if (entity != null)
                {
                    db.Statistics.Remove(entity);
                    db.SaveChanges();
                }
            }
        }
    }
}