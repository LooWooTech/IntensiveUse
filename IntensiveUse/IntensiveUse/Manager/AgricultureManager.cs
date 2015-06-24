﻿using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class AgricultureManager:ManagerBase
    {
        public void  Delete(int Year, int ID)
        {
            using (var db = GetIntensiveUseContext())
            {
                var entity = db.Agricultures.FirstOrDefault(e => e.Year == Year && e.RID == ID);
                if (entity != null)
                {
                    db.Agricultures.Remove(entity);
                    db.SaveChanges();
                }
            }
        }
    }
}