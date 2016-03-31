using IntensiveUse.Helper;
using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class SuperiorManager:ManagerBase
    {
      
        /// <summary>
        /// 返回两个Situation变量
        /// </summary>
        /// <param name="year"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public Situation[] AFind(int year,Region region)
        {
            bool flag = region.BelongCity == region.Name;
            int em = year;
            Superior superior1 = SearchForSuperior(year, region.ID);
            Superior superior2 = SearchForSuperior(year - 3, region.ID);
            Situation[] situations = new Situation[2];
            situations[0] = new Situation()
            {
                Increment = flag ? superior1.ProvinceCompare - superior2.ProvinceCompare : superior1.CityCompare - superior2.CityCompare
            };
            situations[1] = new Situation()
            {
                Increment = flag ? superior1.ProvinceConstruction - superior2.ProvinceConstruction : superior1.CityConstruction - superior2.CityConstruction
            };

            return situations;

        }
    }
}