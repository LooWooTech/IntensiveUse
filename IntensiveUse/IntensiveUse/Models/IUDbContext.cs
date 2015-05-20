using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IntensiveUse.Models
{
    public class IUDbContext:DbContext
    {
        public IUDbContext() : base("IU") { }
        public IUDbContext(string connectionString) : base(connectionString) { }

        public DbSet<UploadFile> UploadFiles { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<People> Peoples { get; set; }
        public DbSet<Economy> Economys { get; set; }
        public DbSet<AgricultureLand> Agricultures { get; set; }
        public DbSet<ConstructionLand> Constructions { get; set; }
    }
}