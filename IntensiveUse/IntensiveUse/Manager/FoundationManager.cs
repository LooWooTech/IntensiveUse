using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class FoundationManager:ManagerBase
    {
        public void Save(Foundation foundation)
        {
            using (var db = GetIntensiveUseContext())
            {
                Foundation entity = db.Foundations.FirstOrDefault(e => e.RID == foundation.RID && e.Year == foundation.Year);
                if (entity == null)
                {
                    db.Foundations.Add(foundation);
                }
                else
                {
                    foundation.ID = entity.ID;
                    db.Entry(entity).CurrentValues.SetValues(foundation);
                }
                db.SaveChanges();
            }
        }
    }
}