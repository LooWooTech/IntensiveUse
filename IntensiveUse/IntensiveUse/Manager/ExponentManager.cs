using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class ExponentManager:ManagerBase
    {
        public int Add(Exponent exponent)
        {
            using (var db = GetIntensiveUseContext())
            {
                db.Exponents.Add(exponent);
                db.SaveChanges();
                return exponent.ID;
            }
        }
    }
}