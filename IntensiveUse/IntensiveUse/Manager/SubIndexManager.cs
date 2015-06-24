using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class SubIndexManager:ManagerBase
    {
        public void Delete(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                var entity = db.SubIndexs.FirstOrDefault(e => e.Year == Year && e.RID == ID);
                if (entity != null)
                {
                    db.SubIndexs.Remove(entity);
                    db.SaveChanges();
                }
            }
        }
    }
}