using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class NewConstructionManager:ManagerBase
    {
        public void  Delete(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                var entity = db.NewConstructions.FirstOrDefault(e => e.Year == Year && e.CID == ID);
                if (entity != null)
                {
                    db.NewConstructions.Remove(entity);
                    db.SaveChanges();
                }
            }
        }

    }
}