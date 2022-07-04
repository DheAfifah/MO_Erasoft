using MO_Erasoft.Models;

namespace MO_Erasoft
{
    using System.Data.Entity;

    public partial class ERADbContext : DbContext
    {
        public DbSet<ARF01> STOKINFO { get; set; }

        public ERADbContext() : base("name=ERADbContext") { }
        public ERADbContext(string dbSourceRisa) : base("name=ERADbContext") { }
    }
}