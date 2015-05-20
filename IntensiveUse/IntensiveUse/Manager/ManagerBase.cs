using IntensiveUse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntensiveUse.Manager
{
    public class ManagerBase
    {
        protected ManagerCore Core = ManagerCore.Instance;

        protected IUDbContext GetIntensiveUseContext()
        {
            var db = new IUDbContext();
            db.Database.Connection.Open();
            return db;
        }
    }
}