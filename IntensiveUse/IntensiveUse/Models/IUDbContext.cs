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
        public DbSet<NewConstruction> NewConstructions { get; set; }
        public DbSet<LandSupply> LandSupplys { get; set; }
        public DbSet<Ratify> Ratifys { get; set; }
        public DbSet<Exponent> Exponents { get; set; }
        public DbSet<IndexWeight> IndexWeights { get; set; }
        public DbSet<SubIndex> SubIndexs { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<Foundation> Foundations { get; set; }
        public DbSet<Superior> Superiors { get; set; }
    }
}