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
    }
}