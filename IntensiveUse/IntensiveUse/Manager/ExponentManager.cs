using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class ExponentManager:ManagerBase
    {
        public void Save(Exponent exponent)
        {
            using (var db = GetIntensiveUseContext())
            {
                Exponent entity = db.Exponents.FirstOrDefault(e => e.RID == exponent.RID && e.Year.ToLower() == exponent.Year.ToLower());
                if (entity == null)
                {
                    db.Exponents.Add(exponent);
                }
                else
                {
                    exponent.ID = entity.ID;
                    db.Entry(entity).CurrentValues.SetValues(exponent);
                }
                db.SaveChanges();
            }
        }
    }
}