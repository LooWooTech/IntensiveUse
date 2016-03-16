using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class CommonManager:ManagerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DICT"> </param>
        /// <param name="ID">Region  ID</param>
        public void UpDate<T>(Dictionary<int, T> DICT, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                foreach (var item in DICT.Keys)
                {
                    T m = DICT[item];
                    Statistic temp = db.Statistics.FirstOrDefault(e => e.RID == ID && e.Year == item);
                    if (temp == null)
                    {
                        temp = new Statistic()
                        {
                            RID = ID,
                            Year = item
                        };
                        db.Statistics.Add(temp);
                    }
                    if (m is People)
                    {
                        temp.People = true;
                    }
                    else if (m is Economy)
                    {
                        temp.Economy = true;
                    }
                    else if (m is AgricultureLand)
                    {
                        temp.Agriculture = true;
                    }
                    else if (m is Ratify)
                    {
                        temp.Ratify = true;
                    }
                    else if (m is ConstructionLand)
                    {
                        temp.ConstructionLand = true;
                    }
                    else if (m is NewConstruction)
                    {
                        temp.NewConstruction = true;
                    }
                    else if (m is LandSupply)
                    {
                        temp.LandSupply = true;
                    }
                    else if (m is IndexWeight)
                    {
                        temp.IndexWeight = true;
                    }
                    else if (m is SubIndex)
                    {
                        temp.SubIndex = true;
                    }
                    else if (m is Exponent)
                    {
                        Exponent n = m as Exponent;
                        if (n.Type == IdealType.Value)
                        {
                            temp.Ideal = true;
                        }
                        else if (n.Type == IdealType.Weight)
                        {
                            temp.Weight = true;
                        }
                    }
                    db.SaveChanges();
                }
            }
        }

       
    }
}