using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace covid19_api.Models
{
    public partial class COVIDInfo : DbContext
    {
        public COVIDInfo() : base("name=COVIDInfo") { }

        public DbSet<Ontario> Ontarios { get; set; }
        public DbSet<Canada> Canadas { get; set; }
        public DbSet<World> Worlds { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        
    }
}