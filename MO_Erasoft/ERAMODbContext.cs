using MO_Erasoft.Models;

namespace MO_Erasoft
{
    using System.Data.Entity;

    public partial class ERAMODbContext : DbContext
    {
        public ERAMODbContext() : base("name=ERAMODbContext"){ }
        public ERAMODbContext(string dbSourceMO) : base("name=ERAMODbContext") { }

        //public virtual DbSet<STF02> STF02 { get; set; }
        //public virtual DbSet<STF02E> STF02E { get; set; }
        //public virtual DbSet<STT04A> STT04A { get; set; }
        //public virtual DbSet<STT04B> STT04B { get; set; }
        //public virtual DbSet<STT01A> STT01A { get; set; }
        //public virtual DbSet<STT01B> STT01B { get; set; }
        //public virtual DbSet<STF18> STF18 { get; set; }
        public virtual DbSet<ARF01> ARF01 { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<STF02>()
            //        .HasMany(e => e.STF02B)
            //        .WithRequired(e => e.STF02)
            //        .WillCascadeOnDelete(false);

            //modelBuilder.Entity<STF02>()
            //        .HasOptional(e => e.STF02C)
            //        .WithRequired(e => e.STF02);

            //modelBuilder.Entity<STF02>()
            //        .HasMany(e => e.STF02D)
            //        .WithRequired(e => e.STF02)
            //        .HasForeignKey(e => e.UNIT)
            //        .WillCascadeOnDelete(false);

            //modelBuilder.Entity<STF02>()
            //        .HasOptional(e => e.STF02F)
            //        .WithRequired(e => e.STF02);

            //modelBuilder.Entity<STT04A>()
            //    .HasMany(e => e.STT04B)
            //    .WithRequired(e => e.STT04A)
            //    .HasForeignKey(e => new { e.NOBUK, e.Gud })
            //    .WillCascadeOnDelete(false);
            
        }

    }
}